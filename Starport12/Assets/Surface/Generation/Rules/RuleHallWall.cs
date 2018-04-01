using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class RuleHallWall : GenerationRule<Ctx>
    {
        public RuleHallWall()
        {
            Tag = RuleConstants.TAG_WALL;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            return new bool[]
            {
                   (ctx.Replaceable && ctx.Walkable && ctx.WallLeft && ctx.WallRight && !ctx.WallTop && !ctx.WallLow)
                || (ctx.Replaceable && ctx.Walkable && !ctx.WallLeft && !ctx.WallRight && ctx.WallTop && ctx.WallLow)


            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var offsetA = new Vector3(ctx.WallOffset, 0, 0);
            var offsetB = new Vector3(-ctx.WallOffset, 0, 0);

            var rotA = 180;
            var rotB = 0;

            if (ctx.WallTop)
            {
                rotA = 90;
                rotB = -90;
                offsetA = new Vector3(0, 0, -ctx.WallOffset);
                offsetB = new Vector3(0, 0, ctx.WallOffset);

            }


            output.Add(new CreateObjectAction(ctx.TileSet.WallPrefab,
                ctx.WorldPos + offsetB * ctx.Get<float>(RuleConstants.CELL_UNIT_WIDTH),
                Quaternion.Euler(0, rotB, 0)));



            output.Add(new CreateObjectAction(ctx.TileSet.WallPrefab,
                ctx.WorldPos + offsetA * ctx.Get<float>(RuleConstants.CELL_UNIT_WIDTH),
                Quaternion.Euler(0, rotA, 0)));


            return output;
        }
    }
}
