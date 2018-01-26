using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{
    public class RuleLightAtTJunctionTop : GenerationRule<Ctx>
    {
        public RuleLightAtTJunctionTop()
        {
            Tag = RuleConstants.TAG_LIGHT;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            var neighbor = ctx.GetNeighborCtx(0, 1);
            return new bool[]
            {
                ctx.WallTop,
                !ctx.WallRight,
                !ctx.WallLeft,
                !ctx.WallLow,
                neighbor != null && neighbor.WallRight,
                neighbor != null && neighbor.WallLeft,
            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            return new GenerationAction[]
            {
                new CreateObjectAction("light_wall2", ctx.WorldPos + new Vector3(0, 0, -.5f) * ctx.CellUnitWidth, Quaternion.Euler(0, -90, 0))
            }.ToList();
        }
    }

    public class RuleLightAtTJunctionLow : GenerationRule<Ctx>
    {
        public RuleLightAtTJunctionLow()
        {
            Tag = RuleConstants.TAG_LIGHT;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            var neighbor = ctx.GetNeighborCtx(0, -1);
            return new bool[]
            {
                ctx.WallLow,
                !ctx.WallTop,
                !ctx.WallRight,
                !ctx.WallLeft,
                neighbor != null && neighbor.WallRight,
                neighbor != null && neighbor.WallLeft,
            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            return new GenerationAction[]
            {
                new CreateObjectAction("light_wall2", ctx.WorldPos + new Vector3(0, 0, .5f) * ctx.CellUnitWidth, Quaternion.Euler(0, 90, 0))
            }.ToList();
        }
    }

    public class RuleLightAtTJunctionLeft : GenerationRule<Ctx>
    {
        public RuleLightAtTJunctionLeft()
        {
            Tag = RuleConstants.TAG_LIGHT;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            var neighbor = ctx.GetNeighborCtx(1, 0);
            return new bool[]
            {
                ctx.WallLeft,
                !ctx.WallLow,
                !ctx.WallTop,
                !ctx.WallRight,
                neighbor != null && neighbor.WallTop,
                neighbor != null && neighbor.WallLow,
            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            return new GenerationAction[]
            {
                new CreateObjectAction("light_wall2", ctx.WorldPos + new Vector3(-.5f, 0, 0) * ctx.CellUnitWidth, Quaternion.Euler(0, 0, 0))
            }.ToList();
        }
    }

    public class RuleLightAtTJunctionRight : GenerationRule<Ctx>
    {
        public RuleLightAtTJunctionRight()
        {
            Tag = RuleConstants.TAG_LIGHT;
        }

        public override bool[] EvaluateConditions(Ctx ctx)
        {
            var neighbor = ctx.GetNeighborCtx(-1, 0);
            return new bool[]
            {
                ctx.WallRight,
                !ctx.WallLeft,
                !ctx.WallLow,
                !ctx.WallTop,
                neighbor != null && neighbor.WallTop,
                neighbor != null && neighbor.WallLow,
            };
        }

        public override List<GenerationAction> Execute(Ctx ctx)
        {
            return new GenerationAction[]
            {
                new CreateObjectAction("light_wall2", ctx.WorldPos + new Vector3(.5f, 0, 0) * ctx.CellUnitWidth, Quaternion.Euler(0, 180, 0))
            }.ToList();
        }
    }
}
