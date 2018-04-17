using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{

    public static class RuleLightMain_Ctx
    {
        public const string HAS_LIGHT = "light_has_main";
        public static bool HasLight(this Ctx ctx)
        {
            EnsureLight(ctx);
            return ctx.Get<bool>(HAS_LIGHT);
        }
        public static void SetLight(this Ctx ctx, bool hasLight)
        {
            ctx.Set(HAS_LIGHT, hasLight);
        }
        public static void EnsureLight(this Ctx ctx)
        {
            ctx.Ensure(HAS_LIGHT, false);
        }
    }

    public class RuleLightMainRemove : GenerationRule<Ctx>
    {
        public RuleLightMainRemove()
        {
            Tag = RuleConstants.TAG_LIGHT;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {

            var left = ctx.GetNeighborCtx(-1, 0);
            var right = ctx.GetNeighborCtx(1, 0);
            var top = ctx.GetNeighborCtx(0, -1);
            var low = ctx.GetNeighborCtx(0, 1);

            

            return new bool[]
            {
                ctx.HasLight(),
                left == null ? false : left.HasLight(),
                right == null ? false : right.HasLight(),
                //top == null ? false : top.HasLight(),
                //low == null ? false : low.HasLight(),
            };

        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            var output = new List<GenerationAction>();

            var rule2Actions = ctx.GetActions();

            rule2Actions.Values.ToList().ForEach(actions =>
            {
                actions.OfType<CreateObjectAction>()
                    .Where(a => a.PrefabPath.Equals("light_main1"))
                    .ToList()
                    .ForEach(a => actions.Remove(a));
            });

            return output;


        }
    }

    public class RuleLightMain : GenerationRule<Ctx>
    {


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


            var rightOpen = IsOpen(ctx.GetNeighborCtx(1, 0));
            var leftOpen = IsOpen(ctx.GetNeighborCtx(-1, 0));

            var topOpen = IsOpen(ctx.GetNeighborCtx(0, -1));
            var lowOpen = IsOpen(ctx.GetNeighborCtx(0, 1));

            return new bool[]
            {
                !ctx.HasLight(),
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


            ctx.SetLight(true);

            output.Add(new CreateObjectAction(ctx.Coord, "light_main1", position , Quaternion.identity));


            return output;
        }
    }
}
