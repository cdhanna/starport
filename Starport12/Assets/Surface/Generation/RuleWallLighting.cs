using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    public class RuleWallLighting : GenerationRule<Ctx>
    {
        public override bool[] EvaluateConditions(Ctx ctx)
        {
            return new bool[]
            {
                ctx.Get<bool>("isWall")
            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var actions = new List<GenerationAction>();

            // get random wall side
            var right = ctx.Get<bool>("cell_wallRight");
            var top = ctx.Get<bool>("cell_wallTop");
            var coord = new GridXY(ctx.CellX(), ctx.CellY());

            if (right)
            {
                var position = World.Map.TransformCoordinateToWorld(coord);
                //position.y += 1;

                var mapScale = World.Map.CellWidth;
                position.x += mapScale * .4f;

                actions.Add(new CreateObjectAction("light_simple", position, Quaternion.identity));
            }

            return actions;
        }
    }
}
