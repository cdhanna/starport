using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{

    public static class PatternRule
    {

        private static string[] Rotate90(string[] input)
        {
            var output = new List<string>();

            var maxCol = 0;

            for (var i = 0; i < input.Length; i++)
            {
                maxCol = Math.Max(maxCol, input[i].Length);
            }
            
            for (var i = 0; i < maxCol; i++)
            {
                var row = "";
                for (var j = input.Length - 1; j > -1; j--)
                {
                    row += input[j][i];
                }
                output.Add(row);
            }

            return output.ToArray();
        }

        public static List<PatternReplaceRule> General(string prefabName,  params string[] data)
        {
            var set = new List<PatternReplaceRule>();

            set.Add(new PatternReplaceRule(data, prefabName, 0));
            for (var i = 1; i < 4; i++)
            {

                data = Rotate90(data);
                set.Add(new PatternReplaceRule(data, prefabName, 90 * i));
            }

            return set;
        }
    }

    public class PatternReplaceRule :  GenerationRule<Ctx>
    {

        private string[] data;
        private string prefabName;
        private Quaternion quaternion;
        private int degree;

        public PatternReplaceRule(string[] _data, string _prefabName, int _degree)
        {
            degree = _degree;
            quaternion = Quaternion.Euler(0, _degree, 0);
            data = _data;
            prefabName = _prefabName;
            Tag = "PATTERN";
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            
            var pattern = new List<bool>();
            for (var i = 0; i < data.Length; i++)
            {
                for (var j = 0; j < data[i].Length; j++)
                {
                    var neighbor = ctx.GetNeighborCtx((int)i, (int)j);
                    var matched = neighbor != null
                        && !neighbor.Patterned
                        && neighbor.Cell.Code == data[i][j];
                    pattern.Add(matched);
                    if (!matched)
                    {
                        return pattern.ToArray();
                    }
                    //if (neighbor != null && data[i][j] != neighbor.Cell.Code)
                    //{
                    //    //pattern.Add(new Vector2(i, j));
                    //    pattern.Add(neighbor != null && !neighbor.Patterned);
                    //}
                    //else
                    //{
                    //    Debug.Log("Adding must be null at " + i + "," + j + " for " + ctx.X + "," + ctx.Y + " and cell is " + (neighbor == null));
                    //    pattern.Add(neighbor == null);
                    //}
                }
            }

            return pattern.ToArray();
            //return pattern.Select(v => ctx.GetNeighborCtx((int)v.x, (int)v.y) != null).ToArray();

            //return new bool[] { true };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();
            Debug.Log("Pattern match");

            for (var i = 0; i < data.Length; i++)
            {
                for (var j = 0; j < data[i].Length; j++)
                {
                    if (data[i][j] == '0')
                    {
                        ///pattern.Add(new Vector2(i, j));

                    }
                    var neighbor = ctx.GetNeighborCtx(i, j);
                    if(neighbor != null)
                    {
                        neighbor.Patterned = true;
                        var rule2Actions = neighbor.GetActions();
                        Debug.Log("Existing Actions " + rule2Actions.Count);
                        rule2Actions.Values.ToList().ForEach(actions =>
                        {
                            actions.OfType<CreateObjectAction>()
                                //.Where(a => a.PrefabPath.Equals("light_main1"))
                                .ToList()
                                .ForEach(a => actions.Remove(a));
                        });
                    }
                }
            }
                        var coord = new GridXY(ctx.X, ctx.Y);

            var position = ctx.Get<Vector3>(RuleConstants.CELL_WORLD_POS);

            var offset = new Vector3(0, 0, 0);
            //var width = ctx.CellUnitWidth * (Math.Max(data[0].Length, data.Length) -1);
            var width = ctx.CellUnitWidth * data.Length - 1;
            //if (degree == 180)
            //{
            //    offset = new Vector3(0, 0, width);
            //}
            //if (degree == 270)
            //{
            //    offset = new Vector3(width, 0,0);

            //}

            offset = new Vector3(0,0,ctx.CellUnitWidth * -.5f);
            if (degree == 180 || degree == 270)
            {

            }
                offset = quaternion * offset;

            //var offset = new Vector3(0, 0, width);
            //offset = quaternion * offset;
            //var position = World.Map.TransformCoordinateToWorld(coord);
            //position += offset * ctx.Get<float>(RuleConstants.CELL_UNIT_WIDTH);

            ////position.y = 1;


            output.Add(new CreateObjectAction(prefabName, position + offset, quaternion));


            return output;
        }

    }
}
