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
            { "zones", Cell.LAYER_ZONES },
            { "replace", Cell.LAYER_REPLACE }
        };


        public static MapXY LoadFromPattern(CellHandlers handlers, MapPattern pattern)
        {
            return LoadFromMFT(handlers, pattern.PatternData.Raw);

        }


        public static MapXY LoadFromMFT(CellHandlers handlers, byte[] bytes)
        {
            //var bytes = File.ReadAllBytes(mftFilePath);
            var mapFile = MapFileCodec.Converter.FromBytes(bytes);
            return Load(handlers, mapFile);
        }
        
        public static RuleAppliedResults InsetMap(MapXY map, int totalWidth, int totalHeight)
        {
            var newMap = new MapXY(map.Handlers);
            var mapWidth = map.HighestX - map.LowestX;
            var mapHeight = map.HighestY - map.LowestY;

            var xGap = (totalWidth - mapWidth) / 2;
            var yGap = (totalHeight - mapHeight) / 2;
            var left = map.LowestX - xGap;
            var right = map.HighestX + xGap;
            var top = map.LowestY - yGap;
            var low = map.HighestY + yGap;
            for (var y = top; y < low; y++)
            {
               
                for (var x = left; x < right; x++)
                {
                    if (x >= map.LowestX && x <= map.HighestX && y >= map.LowestY && y <= map.HighestY)
                    {
                        continue;
                    }
                    var coord = new GridXY(x, y);
                    var cell = new Cell();
                    cell.CellData[Cell.LAYER_ROOMS] = new CellTemplate()
                    {
                        Red = 128,
                        Green = 128,
                        Blue = 128,
                        Alpha = 255,
                        HadData = true
                    };
                    cell.CellData[Cell.LAYER_WALKABLE] = new CellTemplate()
                    {
                        Red = 0,
                        Green = 0,
                        Blue = 0,
                        Alpha = 255,
                        HadData = true
                    };
                    newMap.SetCell(coord, cell);

                }
            }

            map.AutoMap((coord, c) => map.Handlers.Walkable.Process(c));

            var results = ApplyRules(null, newMap, new PatternSet(), new List<SuperRule>(), null);

            return results;

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
                                HadData = true,
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

        public static RuleAppliedResults ApplyRules(Ctx globalCtx, MapXY map, PatternSet generationPatterns, List<SuperRule> additionalRules, IEnumerable<GridXY> coordinatesToApply)
        {
            if (additionalRules == null)
            {
                additionalRules = new List<SuperRule>();
            }
            if (coordinatesToApply == null)
            {
                coordinatesToApply = map.Coordinates;
            }
            if (globalCtx == null)
            {

                globalCtx = new Ctx(null);

                globalCtx.Set(RuleConstants.WALL_OFFSET, .5f);
                globalCtx.Set("HardWalls", false);
                globalCtx.Map = map;
            }

            var runner = new GenerationRunner(new string[][]{

                new string[]{ RuleConstants.TAG_FLOOR,
                    RuleConstants.TAG_WALL,
                    RuleConstants.TAG_JOINER,
                    RuleConstants.TAG_CORNER_JOINER,
                    RuleConstants.TAG_LIGHT},
                new string[]{ RuleConstants.TAG_LIGHT, "PATTERN"},
                new string[]{ "SPAWN" }

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
            allRules.AddRange(additionalRules.Select(r => r.Rule).ToArray());

            var actions = runner.Run(globalCtx, coordinatesToApply, (ctx, coord) => ctx.SetFromGrid(map, coord),
                    allRules.ToArray());

            actions.ForEach(a => a.Invoke(globalCtx));

            var allObjects = globalCtx.Ensure("all_generated_objects", new List<GameObject>());

            return new RuleAppliedResults()
            {
                GeneratedObjects = allObjects,
                Global = globalCtx,
                Output = map
            };
        }
       
        
    }
}
