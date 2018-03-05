using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class PatternReplaceRule :  GenerationRule<Ctx>
    {

        private string[] data = new string[]
            {
                //".0.",
                //".0.",
                "..",
                "00",
                ".."

                //".."
                //".00."
                //"000",
                //"0.0",
                //"000"
            };

        public PatternReplaceRule()
        {
            Tag = "PATTERN";
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            
            var pattern = new List<bool>();
            for (var i = 0; i < data.Length; i++)
            {
                for (var j = 0; j < data[i].Length; j++)
                {
                        var cell = ctx.GetNeighborCtx((int)i, (int)j);
                    if (data[i][j] == '0')
                    {
                        //pattern.Add(new Vector2(i, j));
                        pattern.Add(cell != null && !cell.Patterned);
                    } else
                    {
                        Debug.Log("Adding must be null at " + i + "," + j + " for " + ctx.X + "," + ctx.Y + " and cell is " + (cell == null));
                        pattern.Add(cell == null);
                    }
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
            //var position = World.Map.TransformCoordinateToWorld(coord);
            position += offset * ctx.Get<float>(RuleConstants.CELL_UNIT_WIDTH);

            ////position.y = 1;
            

            output.Add(new CreateObjectAction("hall2", position, Quaternion.identity));


            return output;
        }

    }
}
