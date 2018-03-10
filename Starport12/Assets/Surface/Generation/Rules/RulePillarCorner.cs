using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class RulePillarCorner : GenerationRule<Ctx>
    {
        public RulePillarCorner()
        {
            Tag = RuleConstants.TAG_CORNER_JOINER;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            return new bool[]
            {
                    (!ctx.WallTop && !ctx.WallRight && ctx.GetNeighborCtx(0, -1).WallRight && ctx.GetNeighborCtx(1, 0).WallTop)
                ||  (!ctx.WallTop && !ctx.WallLeft && ctx.GetNeighborCtx(0, -1).WallLeft && ctx.GetNeighborCtx(-1, 0).WallTop)
                ||  (!ctx.WallLow && !ctx.WallLeft && ctx.GetNeighborCtx(0, 1).WallLeft && ctx.GetNeighborCtx(-1, 0).WallLow)
                ||  (!ctx.WallLow && !ctx.WallRight && ctx.GetNeighborCtx(0, 1).WallRight && ctx.GetNeighborCtx(1, 0).WallLow)
            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var position = ctx.WorldPos;

            if (!ctx.WallTop && !ctx.WallRight && ctx.GetNeighborCtx(0, -1).WallRight && ctx.GetNeighborCtx(1, 0).WallTop)
            {

                var offset = new Vector3(.45f, 0, -.45f);
                output.Add(new CreateObjectAction(ctx.Cell.DefaultCornerJoinAsset, position + offset * ctx.CellUnitWidth, Quaternion.identity));
            }

            if (!ctx.WallTop && !ctx.WallLeft && ctx.GetNeighborCtx(0, -1).WallLeft && ctx.GetNeighborCtx(-1, 0).WallTop)
            {
                var offset = new Vector3(-.45f, 0, -.45f);
                output.Add(new CreateObjectAction(ctx.Cell.DefaultCornerJoinAsset, position + offset * ctx.CellUnitWidth, Quaternion.identity));
            }

            if (!ctx.WallLow && !ctx.WallLeft && ctx.GetNeighborCtx(0, 1).WallLeft && ctx.GetNeighborCtx(-1, 0).WallLow)
            {
                var offset = new Vector3(-.45f, 0, .45f);
                output.Add(new CreateObjectAction(ctx.Cell.DefaultCornerJoinAsset, position + offset * ctx.CellUnitWidth, Quaternion.identity));
            }
            if (!ctx.WallLow && !ctx.WallRight && ctx.GetNeighborCtx(0, 1).WallRight && ctx.GetNeighborCtx(1, 0).WallLow)
            {
                var offset = new Vector3(.45f, 0, .45f);
                output.Add(new CreateObjectAction(ctx.Cell.DefaultCornerJoinAsset, position + offset * ctx.CellUnitWidth, Quaternion.identity));
            }



            return output;
        }
    }
}
