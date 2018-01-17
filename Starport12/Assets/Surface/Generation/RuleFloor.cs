using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    public class RuleFloor : GenerationRule<Ctx>
    {
        public RuleFloor()
        {
            Tags = new string[] { "general", "floor" };
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            return new bool[] { };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var coord = new GridXY(ctx.CellX(), ctx.CellY());
            var position = World.Map.TransformCoordinateToWorld(coord);
            //position.y = 1;
            output.Add(new CreateObjectAction("floor_standard", position, Quaternion.identity));

            return output;
        }
    }
}
