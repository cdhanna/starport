using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class RuleLightMain : GenerationRule<Ctx>
    {

        public const string HAS_LIGHT = "light_has_main";

        public RuleLightMain()
        {
            Tag = RuleConstants.TAG_LIGHT;
        }

        private bool IsOpen(Ctx ctx)
        {
            if (ctx == null) return false;

            var left = ctx.GetNeighborCtx(-1, 0);
            var right = ctx.GetNeighborCtx(1, 0);
            var top = ctx.GetNeighborCtx(0, -1);
            var low = ctx.GetNeighborCtx(0, 1);
            return
                (left != null ? !left.WallTop && !left.WallLow : false)
                && (right != null ? !right.WallTop && !right.WallLow : false)
                && (top != null ? !top.WallLeft && !top.WallRight : false)
                && (low != null ? !low.WallLeft && !low.WallRight : false)
                && !ctx.WallAny;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {

            ctx.Ensure(HAS_LIGHT, false);

            var rightOpen = IsOpen(ctx.GetNeighborCtx(1, 0));
            var leftOpen = IsOpen(ctx.GetNeighborCtx(-1, 0));

            var topOpen = IsOpen(ctx.GetNeighborCtx(0, -1));
            var lowOpen = IsOpen(ctx.GetNeighborCtx(0, 1));

            return new bool[]
            {
                IsOpen(ctx)
                //(IsOpen(ctx) && !rightOpen && !leftOpen)
                //|| (IsOpen(ctx) && rightOpen && leftOpen)
                //|| (IsOpen(ctx) && topOpen && lowOpen)
                //IsOpen(ctx.GetNeighborCtx(1, 0))
            };

            //var left = ctx.GetNeighborCtx(-1, 0);
            //var right = ctx.GetNeighborCtx(1, 0);
            //var top = ctx.GetNeighborCtx(0, -1);
            //var low = ctx.GetNeighborCtx(0, 1);


            //return new bool[]
            //{
            //    //!ctx.WallLeft,
            //    //!ctx.WallRight,
            //    //!ctx.WallTop,
            //    //!ctx.WallLow,
            //    !ctx.WallAny,
            //    left != null ? !left.WallTop&&!left.WallLow : false,
            //    right != null ? !right.WallTop&&!right.WallLow : false,
            //    top != null ? !top.WallLeft&&!top.WallRight : false,
            //    low != null ? !low.WallLeft&&!low.WallRight : false,
            //};
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var position = ctx.WorldPos;

            //var left = 


            ctx.Set(HAS_LIGHT, true);

            output.Add(new CreateObjectAction("light_main1", position , Quaternion.identity));


            return output;
        }
    }
}
