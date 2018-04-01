//using Smallgroup.Starport.Assets.Core.Generation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

using System.Collections.Generic;
using Smallgroup.Starport.Assets.Core.Generation;
using MapFileCodec;
using System;
using System.Linq;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class PatternRule : GenerationRule<Ctx>
    {
        public static List<PatternRule> GenerateAllRotations(MapPattern pattern)
        {
            var all = new List<PatternRule>();

            all.Add(new PatternRule(pattern, 0));
            all.Add(new PatternRule(pattern, 1));
            all.Add(new PatternRule(pattern, 2));
            all.Add(new PatternRule(pattern, 3));

            /* 123
             * 456
             * 789
             * 
             * 
             * 
             */

            return all;
        }

        public static MapFile Rotate(MapFile input)
        {
            var output = new MapFile()
            {
                LayerCount = input.LayerCount,
                LayerData = input.LayerData,
                LayerNames = input.LayerNames,
                Name = input.Name,
                Width = input.Height,
                Height = input.Width,
            };

            var nextData = new List<CellData[]>();
            for (var l = 0; l < input.LayerCount; l++)
            {
                var line = new List<CellData>();
                for (var index = 0; index < input.LayerData[l].Length; index++)
                {
                    var coef = (input.Height - 1 ) - (index % input.Height);
                    var index2 = input.Width *coef + (index / input.Height);


                    var x = index2 % input.Width;
                    var y = index2 / input.Width;
                    

                    //y2 = (input.Height - 1) - y; x2 = x;



                    try
                    {
                        //var node = input.LayerData[l][y2 * output.Height + x2];
                        var node = input.GetData(l, x, y);
                        line.Add(node);
                    } catch (Exception ex)
                    {

                    }
                }

                nextData.Add(line.ToArray());
            }
            output.LayerData = nextData.ToArray();
           
            //            var output = new List<string>();

            //            var maxCol = 0;

            //            for (var i = 0; i < input.Length; i++)
            //            {
            //                maxCol = Math.Max(maxCol, input[i].Length);
            //            }

            //            for (var i = 0; i < maxCol; i++)
            //            {
            //                var row = "";
            //                for (var j = input.Length - 1; j > -1; j--)
            //                {
            //                    row += input[j][i];
            //                }
            //                output.Add(row);
            //            }

            //            return output.ToArray();

            return output;
        }


        public MapPattern Pattern { get; private set; }
        public MapFile MapFile { get; private set; }
        public Quaternion Quaternion { get; private set; }


        public PatternRule(MapPattern pattern, int rotationCount)
        {
            Tag = "PATTERN";
            Pattern = pattern;
            MapFile = Converter.FromBytes(pattern.PatternData.Raw);
            for (var i = 0; i < rotationCount; i++)
            {
                MapFile = Rotate(MapFile);
            }
            Quaternion = Quaternion.Euler(0, rotationCount * 90, 0);
        }

        public int RuleSize { get { return MapFile.Width * MapFile.Height; } }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            var results = new List<bool>();
            for (var x = 0; x < MapFile.Width; x++)
            {
                for (var y = 0; y < MapFile.Height; y++)
                {
                    var expectedData = MapFile.GetData(x, MapFile.Height-1 - y);
                    var neighbor = ctx.GetNeighborCtx(x, y);
                    if (neighbor == null)
                    {
                        return new bool[] { false };
                    }
                    var actualData = neighbor.Cell.CellData;

                    for (var i = 0; i < expectedData.Length; i++)
                    {
                        var outCode = default(long);
                        var name = MapFile.LayerNames[i];
                        if (MapLoader.LayerNameToCode.TryGetValue(name, out outCode))
                        {
                            if (outCode == Cell.LAYER_REPLACE)
                            {
                                continue;
                            }
                            var actual = actualData[outCode];
                            var expected = expectedData[i];
                            var match = 
                                    neighbor.PatternCount <= RuleSize
                                &&  actual.Equals(expected);
                            if (match)
                            {
                                results.Add(true);
                            } else
                            {
                                return new bool[] { false };
                            }
                        }
                    }
                }
            }

            return results.ToArray();
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();


            var position = ctx.Get<Vector3>(RuleConstants.CELL_WORLD_POS);
            //var offset = new Vector3(MapFile.Width - 1, 0, -(MapFile.Height - 1)) * ctx.CellUnitWidth / 2;
            var offset = new Vector3(
                MapFile.Width - 1,
                0,
                MapFile.Height - 1
                ) * ctx.CellUnitWidth/2;
            var action = new CreateObjectAction(Pattern.gameObject, position + offset, Quaternion);

            output.Add(action);
            var handler = new ReplacementHandler();
            var hasReplacementLayer = MapFile.LayerNames.Contains("replace");

            for (var x = 0; x < MapFile.Width; x++)
            {
                for (var y = 0; y < MapFile.Height; y++)
                {

                    var neighbor = ctx.GetNeighborCtx(x, y);

                    var cellTemplate = new CellTemplate();

                    if (hasReplacementLayer)
                    {

                        var cellData = MapFile.GetData("replace", x, y);
                        cellTemplate = new CellTemplate()
                        {
                            Alpha = cellData.ChannelA,
                            Red = cellData.ChannelR,
                            Green = cellData.ChannelG,
                            Blue = cellData.ChannelB,
                            HadData = true
                        };
                    }


                    if (neighbor != null)
                    {
                        if (Pattern.ReplacementPattern && handler.Interp(cellTemplate))
                        {
                            //  neighbor.PatternCount = RuleSize;


                            var neighborActions = neighbor.Ensure("pattern_clear_actions", new List<Action>());
                            neighborActions.ForEach(clear => clear());
                            neighborActions.Clear();
                            neighborActions.Add(() =>
                           {
                               ctx.GetActions()[this].Remove(action);
                           });

                        

                            var rule2Actions = neighbor.GetActions();
                            rule2Actions.Values.ToList().ForEach(actions =>
                            {
                                actions.OfType<CreateObjectAction>()
                                    .ToList()
                                    .ForEach(a => actions.Remove(a));
                            });
                        }
                    }
                }
            }

            return output;
        }
    }


}
//    public static class PatternRule
//    {

//        private static string[] Rotate90(string[] input)
//        {
//            var output = new List<string>();

//            var maxCol = 0;

//            for (var i = 0; i < input.Length; i++)
//            {
//                maxCol = Math.Max(maxCol, input[i].Length);
//            }

//            for (var i = 0; i < maxCol; i++)
//            {
//                var row = "";
//                for (var j = input.Length - 1; j > -1; j--)
//                {
//                    row += input[j][i];
//                }
//                output.Add(row);
//            }

//            return output.ToArray();
//        }

//        public static List<MapPatternLayer> RotateBy90(List<MapPatternLayer> layers)
//        {
//            var output = new List<MapPatternLayer>();

//            var maxCol = 0;
//            for (var i = 0; i < layers.Count; i++)
//            {
//                maxCol = Math.Max(maxCol, layers[i].Data.Count);
//            }

//            for (var i = 0; i < maxCol; i++)
//            {
//                var row = new MapPatternLayer()
//                {
//                    LayerName = layers[i].LayerName,
//                    Data = new List<CellTemplate>()
//                };
//                for (var j = layers.Count - 1; j > -1; j--)
//                {
//                    var target = layers[j].Data[i];
//                    row.Data.Add(target);
//                    //row += input[j][i];
//                }
//                output.Add(row);
//            }

//            return output;
//        }


//        //public static List<PatternReplaceRule> General(string prefabName, string[] data)
//        //{
//        //    var set = new List<PatternReplaceRule>();
//        //    var originalSize = new Vector2(data[0].Length, data.Length);
//        //    set.Add(new PatternReplaceRule(data, prefabName, 0, new Vector2(0, 0)));
//        //    for (var i = 1; i <4; i++)
//        //    {

//        //        data = Rotate90(data);
//        //        set.Add(new PatternReplaceRule(data, prefabName, 90 * i, originalSize));
//        //    }

//        //    return set;
//        //}

//        public static List<PatternReplaceRule> General(MapPattern prefab)
//        {
//            var set = new List<PatternReplaceRule>();
//            var data = prefab.Layers;
//            var originalSize = new Vector2(data[0].Length, data.Length);
//            set.Add(new PatternReplaceRule(data, prefab.gameObject, 0, new Vector2(0, 0)));
//            if (data.Length > 1 || data[0].Length > 1)
//            {

//                for (var i = 1; i < 4; i++)
//                {

//                    data = Rotate90(data);
//                    set.Add(new PatternReplaceRule(data, prefab.gameObject, 90 * i, originalSize));
//                }

//            }
//            return set;
//        }
//    }

//    public class PatternReplaceRule :  GenerationRule<Ctx>
//    {

//        //private string[] data;
//        //private string prefabName;
//        //private Quaternion quaternion;
//        //private int degree;
//        //private Vector2 anchor;
//        //private GameObject bit;

//        private MapPattern Bit;
//        private List<MapPatternLayer> Layers;
//        private Quaternion Quaternion;

//        public PatternReplaceRule(MapPattern pattern, int rotation)
//        {
//            var rotatedPattern = pattern.Layers;
//            for (var i = 0; i < rotation; i++)
//            {
//                rotatedPattern = PatternRule.RotateBy90(rotatedPattern);
//            }
//            Layers = rotatedPattern;
//            Bit = pattern;
//            Quaternion = Quaternion.Euler(0, rotation * 90, 0);
//        }

//        //public PatternReplaceRule(string[] _data, string _prefabName, int _degree, Vector2 _anchor)
//        //{
//        //    degree = _degree;
//        //    anchor = _anchor;
//        //    Debug.Log("CREATED RULE " + _data.Length + "," + _data[0].Length + "  : " + -degree);
//        //    quaternion = Quaternion.Euler(0, _degree, 0);
//        //    data = _data;
//        //    prefabName = _prefabName;
//        //    Tag = "PATTERN";
//        //}

//        //public PatternReplaceRule(string[] _data, GameObject _bit, int _degree, Vector2 _anchor)
//        //{
//        //    degree = _degree;
//        //    anchor = _anchor;
//        //    Debug.Log("CREATED RULE " + _data.Length + "," + _data[0].Length + "  : " + -degree);
//        //    quaternion = Quaternion.Euler(0, _degree, 0);
//        //    data = _data;
//        //    bit = _bit;
//        //    Tag = "PATTERN";
//        //}


//        public override bool[] EvaluateConditions(Ctx ctx)
//        {
//            var ruleSize = Layers.Count * Layers[0].Data.Count;

//            var pattern = new List<bool>();
//            for (var i = 0; i < Layers.Count; i++)
//            {
//                for (var j = 0; j < Layers[i].Data.Count; j++)
//                {
//                    var neighbor = ctx.GetNeighborCtx((int)j, (int)-i);



//                    var matched = neighbor != null
//                        && neighbor.PatternCount <= ruleSize
//                        && neighbor.Cell.Code == data[i][j];

//                    pattern.Add(matched);
//                    //if (!matched)
//                    //{
//                    //    return new bool[] { false };
//                    //}
//                    //if (neighbor != null && data[i][j] != neighbor.Cell.Code)
//                    //{
//                    //    //pattern.Add(new Vector2(i, j));
//                    //    pattern.Add(neighbor != null && !neighbor.Patterned);
//                    //}
//                    //else
//                    //{
//                    //    Debug.Log("Adding must be null at " + i + "," + j + " for " + ctx.X + "," + ctx.Y + " and cell is " + (neighbor == null));
//                    //    pattern.Add(neighbor == null);
//                    //}
//                }
//            }

//            return pattern.ToArray();
//            //return pattern.Select(v => ctx.GetNeighborCtx((int)v.x, (int)v.y) != null).ToArray();

//            //return new bool[] { true };
//        }

//        public override List<GenerationAction> Execute(Ctx ctx)
//        {
//            var output = new List<GenerationAction>();
//            //Debug.Log("Pattern match");
//            var size = data.Length * data[0].Length;

//            var position = ctx.Get<Vector3>(RuleConstants.CELL_WORLD_POS);
//            var offset = new Vector3(data[0].Length - 1, 0, -(data.Length - 1)) * ctx.CellUnitWidth / 2;

//            var action = new CreateObjectAction(Bit, position + offset, Quaternion);

//            output.Add(action);


//            for (var i = 0; i < data.Length; i++)
//            {
//                for (var j = 0; j < data[i].Length; j++)
//                {

//                    var neighbor = ctx.GetNeighborCtx(j ,-i);
//                    if(neighbor != null)
//                    {
//                        neighbor.PatternCount = size;
//                        var neighborActions = neighbor.Ensure("pattern_clear_actions", new List<Action>());
//                        neighborActions.ForEach(clear => clear());
//                        neighborActions.Clear();
//                        neighborActions.Add(() =>
//                       {
//                           ctx.GetActions()[this].Remove(action);
//                       });


//                        var rule2Actions = neighbor.GetActions();
//                        //Debug.Log("Existing Actions " + rule2Actions.Count);
//                        rule2Actions.Values.ToList().ForEach(actions =>
//                        {
//                            actions.OfType<CreateObjectAction>()
//                                //.Where(a => a.PrefabPath.Equals("light_main1"))
//                                .ToList()
//                                .ForEach(a => actions.Remove(a));
//                        });
//                    }
//                }
//            }



//            return output;
//        }

//    }
//}
