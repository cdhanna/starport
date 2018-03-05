using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class RuleFilled : GenerationRule<Ctx>
    {
        public RuleFilled()
        {
            Tag = "FLOOR";
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {

            return new bool[] { !ctx.Cell.Walkable };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var coord = new GridXY(ctx.X, ctx.Y);
            var position = World.Map.TransformCoordinateToWorld(coord);
            //position.y = 1;
            output.Add(new CreateObjectAction(ctx.Cell.FillPrefabName, position, Quaternion.identity));

            return output;
        }
    }
}
