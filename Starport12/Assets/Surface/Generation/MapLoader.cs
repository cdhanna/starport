using MapFileCodec;
using Newtonsoft.Json;
using Smallgroup.Starport.Assets.Core.Generation;
using Smallgroup.Starport.Assets.Surface.Generation;
using Smallgroup.Starport.Assets.Surface.Generation.Rules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    public class MapLoader : MonoBehaviour
    {
        //public string mftFilePath;
        //public PatternSet GenerationPatternSet;


        //public string filePath;
        //public MapTilePalett tilePalette;
        //public 
        public static Dictionary<string, long> LayerNameToCode { get; private set; } = new Dictionary<string, long>
        {
            { "rooms", Cell.LAYER_ROOMS },
            { "walkable", Cell.LAYER_WALKABLE },
            { "zones", Cell.LAYER_ZONES }
        };


        public static MapXY LoadFromPattern(CellHandlers handlers, MapPattern pattern)
        {
            return LoadFromMFT(handlers, pattern.PatternData.Raw);

            //var map = new MapXY(handlers);



            //var roomLayer = pattern.Layers.FirstOrDefault(l => l.LayerName.ToLower().Equals("rooms"));
            //var walkableLayer = pattern.Layers.FirstOrDefault(l => l.LayerName.ToLower().Equals("walkable"));
            //for (var y = 0; y < pattern.Height; y++)
            //{
            //    for (var x = 0; x < pattern.Width; x++)
            //    {
            //        var cell = default(Cell);

            //        for (var i = 0; i < pattern.Layers.Count; i++)
            //        {
            //            var layerCode = LayerNameToCode[pattern.Layers[i].LayerName];
            //            var layerData = pattern.Layers[i].Data[y * pattern.Width + x];
            //            cell.CellData[layerCode] = new CellTemplate()
            //            {
            //                Red = layerData.Red,
            //                Green = layerData.Green,
            //                Blue = layerData.Blue,
            //                Alpha = layerData.Alpha
            //            };
                        
            //        }


            //        //if (roomLayer != null)
            //        //{
            //        //    var roomData = roomLayer.Data[y * pattern.Width + x];
            //        //    cell = GenerateCell(palett, roomData.Red, roomData.Green, roomData.Blue, roomData.Alpha);
            //        //    cell.Walkable = true;
            //        //}
            //        //if (walkableLayer != null)
            //        //{
            //        //    var walkableData = roomLayer.Data[y * pattern.Width + x];
            //        //    cell.Walkable = walkableData.Blue > 0;

            //        //}
            //        map.SetCell(new GridXY(x, -y + pattern.Height), cell);

            //    }
            //}
            //map.AutoMap((coord, cell) => handlers.Walkable.Process(cell));

            //return map;
        }


        public static MapXY LoadFromMFT(CellHandlers handlers, byte[] bytes)
        {
            //var bytes = File.ReadAllBytes(mftFilePath);
            var mapFile = MapFileCodec.Converter.FromBytes(bytes);
            return Load(handlers, mapFile);
        }
        
        public static MapXY Load(CellHandlers handlers, MapFile mapFile)
        {

            var map = new MapXY(handlers);

            for (var y = 0; y < mapFile.Height; y++)
            {
                for (var x = 0; x < mapFile.Width; x++)
                {
                    //var roomData = mapFile.GetData("rooms", x, y);
                    //var walkableData = mapFile.GetData("walkable", x, y);

                    var names = mapFile.LayerNames;
                    var datas = mapFile.GetData(x, y);
                    var cell = new Cell();
                    for (var i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        var data = datas[i];
                        var layerCode = default(long);
                        if (LayerNameToCode.TryGetValue(name, out layerCode))
                        {
                            cell.CellData[layerCode] = new CellTemplate()
                            {
                                Red = data.ChannelR,
                                Green = data.ChannelG,
                                Blue = data.ChannelB,
                                Alpha = data.ChannelA,
                            };
                        }
                    }

                    //var cell = GenerateCell(palett, roomData.ChannelR, roomData.ChannelG, roomData.ChannelB, roomData.ChannelA);
                    if (cell != null)
                    {
                        // cell.Walkable = walkableData.ChannelB == 255;
                        map.SetCell(new GridXY(x, -y + mapFile.Height), cell);

                    }
                }
            }

            map.AutoMap((coord, cell) => handlers.Walkable.Process(cell));
            return map;

            return null;
        }

        public static List<GameObject> ApplyRules(MapXY map, PatternSet generationPatterns)
        {
            var globalCtx = new Ctx(null);

            globalCtx.Set(RuleConstants.WALL_OFFSET, .5f);
            globalCtx.Map = map;

            var runner = new GenerationRunner(new string[][]{

                new string[]{ RuleConstants.TAG_FLOOR,
                    RuleConstants.TAG_WALL,
                    RuleConstants.TAG_JOINER,
                    RuleConstants.TAG_CORNER_JOINER,
                    RuleConstants.TAG_LIGHT},
                new string[]{ RuleConstants.TAG_LIGHT, "PATTERN"}

            });

            var allRules = new List<GenerationRule<Ctx>>();

            generationPatterns.Patterns = generationPatterns.Patterns.Where(p => p != null).ToList();

            generationPatterns.Patterns.ForEach(bit =>
            {
                allRules.AddRange(PatternRule.GenerateAllRotations(bit));
                //allRules.AddRange(PatternRule.General(bit));
            });

            var standardRules = new GenerationRule<Ctx>[]{
                new RuleFloor(),
                new RuleFloorFull(),
                new RuleSingleWall(),
                new RuleCornerWall(),
                new RuleHallWall(),
                new RuleDeadEndWall(),

                new RulePillarsLeftAndRight(),
                new RulePillarsTopAndLow(),
                new RulePillarCorner(),

            }.ToList();
            allRules.AddRange(standardRules);


            var actions = runner.Run(globalCtx, map, (ctx, coord) => ctx.SetFromGrid(map, coord),
                    allRules.ToArray());

            actions.ForEach(a => a.Invoke(globalCtx));

            return globalCtx.Ensure("all_generated_objects", new List<GameObject>());
        }
       
        //public static Cell GenerateCell(MapTilePalett tilePalette, byte red, byte green, byte blue, byte alpha)
        //{
        //    var r = red / 255f;
        //    var g = green / 255f;
        //    var b = blue / 255f;
        //    var a = alpha / 255f;
        //    var set = tilePalette.TileSets.FirstOrDefault(t => t.WalkColor.r == r && t.WalkColor.g == g && t.WalkColor.b == b);
        //    if (set != null)
        //    {

        //        return new Cell()
        //        {
        //            Walkable = true,
        //            Code = set.WalkableCode,
        //            DefaultCornerJoinAsset = set.CornerJoinPrefab,
        //            DefaultJoinAsset = set.JoinPrefab,
        //            DefaultWallAsset = set.WallPrefab,
        //            Type = set.name,
        //            ReferenceSet = set,
        //            DefaultFloorAsset = set.FloorPrefab,
        //        };
        //    }
        //    else return null;
        //}

        


        //class MapData
        //{
        //    [JsonProperty("basic")]
        //    public MapDataBasic Basic { get; set; }

        //    public class MapDataBasic
        //    {
                
        //        [JsonProperty("data")]
        //        public string[] Data { get; set; }

               
        //        //class 
        //    }
        //}
    }
}
