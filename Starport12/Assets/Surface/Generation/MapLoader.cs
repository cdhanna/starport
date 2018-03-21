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
        public string mftFilePath;
        public PatternSet GenerationPatternSet;


        public string filePath;
        public MapTilePalett tilePalette;
        //public 

        public MapXY LoadFromPattern(MapPattern pattern)
        {
            var map = new MapXY();

            var roomLayer = pattern.Layers.FirstOrDefault(l => l.LayerName.ToLower().Equals("rooms"));
            var walkableLayer = pattern.Layers.FirstOrDefault(l => l.LayerName.ToLower().Equals("walkable"));
            for (var y = 0; y < pattern.Height; y++)
            {
                for (var x = 0; x < pattern.Width; x++)
                {
                    var cell = default(Cell);
                    if (roomLayer != null)
                    {
                        var roomData = roomLayer.Data[y * pattern.Width + x];
                        cell = GenerateCell((byte)(roomData.r * 255), (byte)(roomData.g * 255), (byte)(roomData.b * 255), (byte)(roomData.a * 255));
                        cell.Walkable = true;
                    }
                    if (walkableLayer != null)
                    {
                        var walkableData = roomLayer.Data[y * pattern.Width + x];
                        cell.Walkable = walkableData.b > 0;

                    }
                    map.SetCell(new GridXY(x, -y + pattern.Height), cell);

                }
            }
            map.AutoMap((coord, cell) => cell.Walkable);

            return map;
        }

        public MapXY LoadFromMFT()
        {
            var bytes = File.ReadAllBytes(mftFilePath);
            var mapFile = MapFileCodec.Converter.FromBytes(bytes);
            var map = new MapXY();

            for (var y = 0; y < mapFile.Height; y++)
            {
                for (var x = 0; x < mapFile.Width; x++)
                {
                    var roomData = mapFile.GetData("rooms", x, y);
                    var walkableData = mapFile.GetData("walkable", x, y);
                    var cell = GenerateCell(roomData.ChannelR, roomData.ChannelG, roomData.ChannelB, roomData.ChannelA);
                    if (cell != null)
                    {
                        cell.Walkable = walkableData.ChannelB == 255;
                        map.SetCell(new GridXY(x, -y + mapFile.Height), cell);

                    }
                }
            }

            map.AutoMap( (coord, cell) => cell.Walkable);
            return map;
        }

        public static void ApplyRules(MapXY map, PatternSet generationPatterns)
        {
            var globalCtx = new Ctx(null);

            globalCtx.Set(RuleConstants.WALL_OFFSET, .5f);


            var runner = new GenerationRunner(new string[][]{

                new string[]{ RuleConstants.TAG_FLOOR,
                    RuleConstants.TAG_WALL,
                    RuleConstants.TAG_JOINER,
                    RuleConstants.TAG_CORNER_JOINER,
                    RuleConstants.TAG_LIGHT},
                new string[]{ RuleConstants.TAG_LIGHT, "PATTERN"}

            });

            var rules = new List<GenerationRule<Ctx>>();
                generationPatterns.Patterns = generationPatterns.Patterns.Where(p => p != null).ToList();

                generationPatterns.Patterns.ForEach(bit =>
                {
                    rules.AddRange(PatternRule.General(bit));
                });

            var rules2 = new GenerationRule<Ctx>[]{
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
            rules.AddRange(rules2);


            var actions = runner.Run(globalCtx, map, (ctx, coord) => ctx.SetFromGrid(map, coord),
                    rules.ToArray());

            actions.ForEach(a => a.Invoke(globalCtx));
        }
       
        public Cell GenerateCell(byte red, byte green, byte blue, byte alpha)
        {
            var r = red / 255f;
            var g = green / 255f;
            var b = blue / 255f;
            var a = alpha / 255f;
            var set = tilePalette.TileSets.FirstOrDefault(t => t.WalkColor.r == r && t.WalkColor.g == g && t.WalkColor.b == b);
            if (set != null)
            {

                return new Cell()
                {
                    Walkable = true,
                    Code = set.WalkableCode,
                    DefaultCornerJoinAsset = set.CornerJoinPrefab,
                    DefaultJoinAsset = set.JoinPrefab,
                    DefaultWallAsset = set.WallPrefab,
                    Type = set.name,
                    ReferenceSet = set,
                    DefaultFloorAsset = set.FloorPrefab,
                };
            }
            else return null;
        }

        


        class MapData
        {
            [JsonProperty("basic")]
            public MapDataBasic Basic { get; set; }

            public class MapDataBasic
            {
                
                [JsonProperty("data")]
                public string[] Data { get; set; }

               
                //class 
            }
        }
    }
}
