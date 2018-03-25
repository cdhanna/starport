using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class RuleCornerWall : GenerationRule<Ctx>
    {
        public RuleCornerWall()
        {
            Tag = RuleConstants.TAG_WALL;
        }
        

        public override bool[] EvaluateConditions(Ctx ctx)
        {

            return new bool[]
            {
                ctx.Walkable && (ctx.WallLeft ^ ctx.WallRight),
                ctx.Walkable && (ctx.WallTop ^ ctx.WallLow)
            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var vertOffset = new Vector3(0, 0, -ctx.WallOffset);
            var vertRotation = 90;
            var hortOffset = new Vector3(-ctx.WallOffset, 0, 0);
            var hortRotation = 0;
            var position = ctx.Get<Vector3>(RuleConstants.CELL_WORLD_POS);

            if (ctx.WallLow)
            {
                vertOffset.z *= -1;
                vertRotation = -90;
            }

            if (ctx.WallRight)
            {
                hortOffset.x *= -1;
                hortRotation = 180;
            }


            output.Add(new CreateObjectAction(ctx.TileSet.WallPrefab,
                position + vertOffset * ctx.Get<float>(RuleConstants.CELL_UNIT_WIDTH),
                Quaternion.Euler(0, vertRotation, 0)));



            output.Add(new CreateObjectAction(ctx.TileSet.WallPrefab,
                position + hortOffset * ctx.Get<float>(RuleConstants.CELL_UNIT_WIDTH),
                Quaternion.Euler(0, hortRotation, 0)));


            return output;
        }
    }
}
