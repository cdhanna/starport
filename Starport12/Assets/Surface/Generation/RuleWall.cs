using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    public class RuleRightWall : GenerationRule<Ctx>
    {
        public override bool[] EvaluateConditions(Ctx ctx)
        {
            return new bool[] {
                ctx.Get<bool>("cell_wallRight") || ctx.Get<bool>("cell_wallTop")
            };
        }

        //public override List<GenerationAction> Execute(GenerationContext ctx)
        //{
        //    // if the cell is a wall 


        //    return null;
        //}

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var actions = new List<GenerationAction>();

            var coord = new GridXY(ctx.CellX(), ctx.CellY());

            var right = ctx.Get<bool>("cell_wallRight");
            var top = ctx.Get<bool>("cell_wallTop");

            if (right && top)
            {
                var position = World.Map.TransformCoordinateToWorld(coord);
                //position.y += 1.7f;

                var mapScale = World.Map.CellWidth;
                //position.x += mapScale / 2f;

                actions.Add(new CreateObjectAction("wall_topRight", position, Quaternion.identity));
            } else if (ctx.Get<bool>("cell_wallRight"))
            {

                var position = World.Map.TransformCoordinateToWorld(coord);
                //position.y += 1;

                var mapScale = World.Map.CellWidth;
                position.x += mapScale / 2f;

                actions.Add(new CreateObjectAction("wall_standard", position, Quaternion.identity));
            } else if (ctx.Get<bool>("cell_wallTop"))
            {
                var position = World.Map.TransformCoordinateToWorld(coord);
                //position.y += 1;

                var mapScale = World.Map.CellWidth;
                position.z += mapScale / 2f;

                actions.Add(new CreateObjectAction("wall_standard", position, Quaternion.Euler(0, 90, 0)));
            }


            return actions;
        }
    }
}
