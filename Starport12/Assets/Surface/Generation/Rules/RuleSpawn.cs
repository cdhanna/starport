using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smallgroup.Starport.Assets.Core.Generation;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation.Rules
{


    [CreateAssetMenu(fileName = "Spawn", menuName = "Map/Rules/Spawner")]
    public class SuperSpawnRule : SuperRule
    {
        public GameObject SpawnObject { get; set; }
        public int x;

        public override DefaultCtxRule Rule
        {
            get
            {
                return new RuleSpawn()
                {
                    Template = SpawnObject
                };
            }
        }
        
    }

    public class RuleSpawn : DefaultCtxRule
    {

        public GameObject Template { get; set; }

        public RuleSpawn()
        {
            Tag = "SPAWN";
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
            }
            else if (ctx.Get<bool>(RuleConstants.CELL_WALL_LEFT))
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

            output.Add(new CreateObjectAction(Template, position, Quaternion.Euler(0, rotation, 0)));

            return output;
        }
    }
}
