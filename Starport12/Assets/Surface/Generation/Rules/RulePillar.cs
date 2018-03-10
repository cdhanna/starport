using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class RulePillarsLeftAndRight : GenerationRule<Ctx>
    {
        public RulePillarsLeftAndRight()
        {
            Tag = RuleConstants.TAG_JOINER;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            return new bool[] {
                  (ctx.WallRight  && !ctx.WallTop && ctx.GetNeighborCtx(0, -1).WallRight)
                ||(ctx.WallLeft  && !ctx.WallTop && ctx.GetNeighborCtx(0, -1).WallLeft)
            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var coord = new GridXY(ctx.X, ctx.Y);
            var position = World.Map.TransformCoordinateToWorld(coord);

            if (ctx.WallRight && ctx.GetNeighborCtx(0, -1).WallRight)
            {
                var offset = new Vector3(.55f, 0, -.5f);
                output.Add(new CreateObjectAction(ctx.Cell.DefaultJoinAsset, position + offset * ctx.CellUnitWidth, Quaternion.identity));
            }

            if (ctx.WallLeft && ctx.GetNeighborCtx(0, -1).WallLeft)
            {
                var offset = new Vector3(-.55f, 0, -.5f);
                output.Add(new CreateObjectAction(ctx.Cell.DefaultJoinAsset, position + offset * ctx.CellUnitWidth, Quaternion.identity));
            }



            return output;
        }
    }

    public class RulePillarsTopAndLow : GenerationRule<Ctx>
    {
        public RulePillarsTopAndLow()
        {
            Tag = RuleConstants.TAG_JOINER;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            return new bool[] {
                  (ctx.WallTop  && !ctx.WallRight && ctx.GetNeighborCtx(1, 0).WallTop)
                ||(ctx.WallLow  && !ctx.WallRight && ctx.GetNeighborCtx(1, 0).WallLow)
            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var coord = new GridXY(ctx.X, ctx.Y);
            var position = World.Map.TransformCoordinateToWorld(coord);

            if (ctx.WallTop && ctx.GetNeighborCtx(1, 0).WallTop)
            {
                var offset = new Vector3(.5f, 0, -.55f);
                output.Add(new CreateObjectAction(ctx.Cell.DefaultJoinAsset, position + offset * ctx.CellUnitWidth, Quaternion.Euler(0, 90, 0)));
            }

            if (ctx.WallLow && ctx.GetNeighborCtx(1, 0).WallLow)
            {
                var offset = new Vector3(.5f, 0,.55f);
                output.Add(new CreateObjectAction(ctx.Cell.DefaultJoinAsset, position + offset * ctx.CellUnitWidth, Quaternion.Euler(0, 90, 0)));
            }



            return output;
        }
    }
}
