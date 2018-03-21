using Dialog.Engine;
using Smallgroup.Starport.Assets.Core;
using Smallgroup.Starport.Assets.Core.Generation;
using Smallgroup.Starport.Assets.Surface;
using Smallgroup.Starport.Assets.Surface.Generation;
using Smallgroup.Starport.Assets.Surface.Generation.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts
{
    class WorldAnchor : MonoBehaviour
    {
            
        public MapXY Map { get; set; }

        public DialogAnchor DialogAnchor;
        public ActorAnchor[] Players;
        public PatternSet GenerationPatternSet;
        public MapLoader MapLoader;

        private void AttachCellAnchors()
        {
            for (var i = 0; i <  Map.Coordinates.Length; i++)
            {
                var coord = Map.Coordinates[i];
                var cell = Map.GetCell(coord);

                var cellObj = new GameObject();
                //var cellObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var cellScript = cellObj.AddComponent<CellAnchor>();
                cellScript.Cell = cell;

                cellObj.transform.parent = transform;

                cellObj.transform.localPosition = World.Map.TransformCoordinateToWorld(coord);

                float scale = .9f;
                cellObj.transform.localScale = new Vector3(World.Map.CellWidth * scale, .1f, World.Map.CellWidth * scale);
            }
        }

        protected void Start()
        {
            World.Map = MapLoader.LoadFromMFT();
            //World.Map = MapLoader.LoadFromFile();
            Map = World.Map;
            AttachCellAnchors();

            var globalCtx = new Ctx(null);

            globalCtx.Set(RuleConstants.WALL_NAME, "stoneGen/wall_var1");
            globalCtx.Set(RuleConstants.FLOOR_NAME, "stoneGen/floor_stone");
            globalCtx.Set(RuleConstants.WALL_OFFSET, .5f);


            var runner = new GenerationRunner(new string[][]{

                new string[]{ RuleConstants.TAG_FLOOR,
                    RuleConstants.TAG_WALL,
                    RuleConstants.TAG_JOINER,
                    RuleConstants.TAG_CORNER_JOINER,
                    RuleConstants.TAG_LIGHT},
                new string[]{ RuleConstants.TAG_LIGHT, "PATTERN"}

            });

            var rules = new List<GenerationRule<Ctx>>();
            //rules.Add(new RuleFloor());

            GenerationPatternSet.Patterns = GenerationPatternSet.Patterns.Where(p => p != null).ToList();
            //GenerationPatternSet.Patterns.ToList().Sort(new Comparison<MapBit>((a, b) =>
            //{
            //    var aSize = a.pattern.Length * a.pattern[0].Length;
            //    var bSize = b.pattern.Length * b.pattern[0].Length;
            //    return aSize > bSize ? -1 : 1;
            //}));
            GenerationPatternSet.Patterns.ForEach(bit =>
            {
                 rules.AddRange(PatternRule.General(bit));
            });

            var rules2 = new GenerationRule<Ctx>[]{
                new RuleFloor(),
                new RuleFloorFull(),
                new RuleSingleWall(),
                new RuleCornerWall(),
                new RuleHallWall(),
                new RuleDeadEndWall(),

                new RulePillarsLeftAndRight(),
                new RulePillarsTopAndLow(),
                new RulePillarCorner(),

            //new RuleLightMain()

            //new RuleLightMain(),
            //new RuleLightMainRemove(),
            //new RuleLightAtTJunctionTop(),
            //new RuleLightAtTJunctionLow(),
            //new RuleLightAtTJunctionLeft(),
            //new RuleLightAtTJunctionRight()

            }.ToList();
            rules.AddRange(rules2);
            //rules.AddRange(PatternRule.General("side_good", new string[] {
            //    ".0"
            //}));
            //rules.AddRange(PatternRule.General("corner", new string[]
            //{
            //    "..",
            //    ".0"
            //}));


            var actions = runner.Run(globalCtx, Map, (ctx, coord) => ctx.SetFromGrid(Map, coord),
                    rules.ToArray());

           
            actions.ForEach(a => a.Invoke(globalCtx));
            //var globalCtx = new GenerationContext(null);
            ////globalCtx.Set("game_difficulty", "hard");
            //globalCtx.Set("game_playerCount", 1);
            ////globalCtx.Set("game_color", "brown");

            //var rules = new List<GenerationRule<Ctx>>();
            //rules.Add(new RuleFloor());
            //rules.Add(new RuleRightWall());
            //rules.Add(new RuleWallLighting());

            //var allActions = new List<GenerationAction>();
            //for (var x = 0; x < 3; x++)
            //{
            //    for (var y = 0; y < 3; y++)
            //    {
            //        var ctx = new Ctx(globalCtx);
            //        ctx.Set(RuleConstants.CELL_X, x);
            //        ctx.Set(RuleConstants.CELL_Y, y);

            //        ctx.Set("cell_wallRight", x == 1);
            //        ctx.Set("cell_wallTop", y == 0);
            //        ctx.Set("isWall", y == 0 || x == 1);
            //        if (x == 0)
            //        {
            //        }
            //        if (y == 2)
            //        {
            //        }
                    

            //        var actionLists = rules
            //            .Where(r => r.EvaluateConditions(ctx).All(b => b == true))
            //            .Select(r => r.Execute(ctx))
            //            .ToList();
            //        actionLists.ForEach(l => allActions.AddRange(l));
            //    }
            //}

            //allActions.ForEach(a => a.Invoke(globalCtx));
        }

        protected void Update()
        {

        }

       

    }
}
