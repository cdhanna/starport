using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class RuleSingleWall : GenerationRule<Ctx>
    {
        public RuleSingleWall()
        {
            Tag = RuleConstants.TAG_WALL;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {


            return new bool[]
            {
                ctx.Walkable
                 && (
                   (ctx.WallLeft && !ctx.WallLow && !ctx.WallRight && !ctx.WallTop)
                || (!ctx.WallLeft && ctx.WallLow && !ctx.WallRight && !ctx.WallTop)
                || (!ctx.WallLeft && !ctx.WallLow && ctx.WallRight && !ctx.WallTop)
                || (!ctx.WallLeft && !ctx.WallLow && !ctx.WallRight && ctx.WallTop)
                )
                
            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var position = ctx.Get<Vector3>(RuleConstants.CELL_WORLD_POS);

            var offset = new Vector3(0, 0, 0);
            var rotation = 0;

            if (ctx.Get<bool>(RuleConstants.CELL_WALL_TOP))
            {
                offset = new Vector3(0, 0, -ctx.WallOffset);
                rotation = 90;
            } else if (ctx.Get<bool>(RuleConstants.CELL_WALL_LEFT))
            {
                offset = new Vector3(-ctx.WallOffset, 0, 0);
            }
            else if (ctx.Get<bool>(RuleConstants.CELL_WALL_RIGHT))
            {
                offset = new Vector3(ctx.WallOffset, 0, 0);
                rotation = 180;

            }
            else if (ctx.Get<bool>(RuleConstants.CELL_WALL_LOW))
            {
                offset = new Vector3(0, 0, ctx.WallOffset);
                rotation = -90;

            }


            position += offset * ctx.Get<float>(RuleConstants.CELL_UNIT_WIDTH);

            output.Add(new CreateObjectAction(ctx.TileSet.WallPrefab, position, Quaternion.Euler(0,rotation, 0)));

            return output;
        }
    }
}
