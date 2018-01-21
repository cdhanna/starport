using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class RuleDeadEndWall : GenerationRule<Ctx>
    {
        public RuleDeadEndWall()
        {
            Tag = RuleConstants.TAG_WALL;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            return new bool[]
            {
                   (!ctx.WallLeft && ctx.WallLow && ctx.WallRight && ctx.WallTop)
                || (ctx.WallLeft && !ctx.WallLow && ctx.WallRight && ctx.WallTop)
                || (ctx.WallLeft && ctx.WallLow && !ctx.WallRight && ctx.WallTop)
                || (ctx.WallLeft && ctx.WallLow && ctx.WallRight && !ctx.WallTop)
            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();
            var position = ctx.WorldPos;
            if (ctx.WallLeft)
            {
                output.Add(new CreateObjectAction(ctx.WallPrefabName, position + new Vector3(-ctx.WallOffset, 0, 0) * ctx.CellUnitWidth, Quaternion.Euler(0, 0, 0)));
            }
            if (ctx.WallRight)
            {
                output.Add(new CreateObjectAction(ctx.WallPrefabName, position + new Vector3(ctx.WallOffset, 0, 0) * ctx.CellUnitWidth, Quaternion.Euler(0, 180, 0)));
            }
            if (ctx.WallTop)
            {
                output.Add(new CreateObjectAction(ctx.WallPrefabName, position + new Vector3(0, 0, -ctx.WallOffset) * ctx.CellUnitWidth, Quaternion.Euler(0, 90, 0)));
            }
            if (ctx.WallLow)
            {
                output.Add(new CreateObjectAction(ctx.WallPrefabName, position + new Vector3(0, 0, ctx.WallOffset) * ctx.CellUnitWidth, Quaternion.Euler(0, -90, 0)));
            }


            return output;
        }
    }
}
