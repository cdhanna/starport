using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class RuleFloor : DefaultCtxRule
    {
        public RuleFloor()
        {
            Tag = "FLOOR";
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            return new bool[] { ctx.Walkable };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var coord = new GridXY(ctx.X, ctx.Y);
            var position = ctx.Map.TransformCoordinateToWorld(coord);
            //position.y = 1;
            output.Add(new CreateObjectAction(ctx.TileSet.FloorPrefab, position, Quaternion.identity));

            return output;
        }
    }

    public class RuleFloorFull : GenerationRule<Ctx>
    {
        public RuleFloorFull()
        {
            Tag = "FLOOR";
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            return new bool[] { ctx.TileSet.FillPrefab != null && !ctx.Walkable };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var coord = new GridXY(ctx.X, ctx.Y);
            var position = ctx.Map.TransformCoordinateToWorld(coord);

            //position.y *= ctx.CellUnitWidth;
            output.Add(new CreateObjectAction(ctx.TileSet.FillPrefab, position, Quaternion.identity));

            return output;
        }
    }
}
