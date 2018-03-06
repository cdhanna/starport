using Smallgroup.Starport.Assets.Core;
using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    public static class RuleConstants
    {
        public const string TAG_WALL = "WALL";
        public const string TAG_FLOOR = "FLOOR";
        public const string TAG_JOINER = "JOINER";
        public const string TAG_CORNER_JOINER = "CORNERJOINER";
        public const string TAG_LIGHT = "LIGHT";


        public const string CELL_X = "cell_x";
        public const string CELL_Y = "cell_y";
        public const string CELL_WORLD_POS = "cell_world_pos";
        public const string CELL_WALL_HAS_ANY = "cell_hasAnyWall";
        public const string CELL_WALL_TOP = "cell_wallTop";
        public const string CELL_WALL_LOW = "cell_wallLow";
        public const string CELL_WALL_LEFT = "cell_wallLeft";
        public const string CELL_WALL_RIGHT = "cell_wallRight";
        public const string CELL_UNIT_WIDTH = "cell_unitWidth";
        public const string CELL_PATTERN_MATCHED = "cell_patterned";
        //public const string CELL_QUAD_NEIGHBOR_COUNT = "cell_quadNeighborCount";

        public const string WALL_NAME = "wall_prefabName";
        public const string FLOOR_NAME = "floor_prefabName";
        public const string WALL_OFFSET = "wall_offset";

        public static Func<object, string> COORD_CTX = (coord) => "ctx_"+coord.GetHashCode();
    }

    public class Ctx : GenerationContext
    {

        public Ctx(GenerationContext parent) : base(parent) { }

        public Ctx() : base(null) { }


        public bool WallTop { get { return Get<bool>(RuleConstants.CELL_WALL_TOP); } }
        public bool WallLow { get { return Get<bool>(RuleConstants.CELL_WALL_LOW); } }
        public bool WallRight { get { return Get<bool>(RuleConstants.CELL_WALL_RIGHT); } }
        public bool WallLeft { get { return Get<bool>(RuleConstants.CELL_WALL_LEFT); } }

        public bool WallAny { get { return Get<bool>(RuleConstants.CELL_WALL_HAS_ANY); } }
        public bool Patterned { get { return Get<bool>(RuleConstants.CELL_PATTERN_MATCHED); } set { Set<bool>(RuleConstants.CELL_PATTERN_MATCHED, value); } }

        public Vector3 WorldPos { get { return Get<Vector3>(RuleConstants.CELL_WORLD_POS); } }
        public float CellUnitWidth { get { return Get<float>(RuleConstants.CELL_UNIT_WIDTH); } }

        public int X { get { return Get<int>(RuleConstants.CELL_X); } }
        public int Y { get { return Get<int>(RuleConstants.CELL_Y); } }

        public string WallPrefabName { get { return Get(RuleConstants.WALL_NAME); } }
        public string FloorPrefabName { get { return Get(RuleConstants.FLOOR_NAME); } }
        public float WallOffset { get { return Get<float>(RuleConstants.WALL_OFFSET); } }

        public Cell Cell { get { return World.Map.GetCell(new GridXY(X, Y)); } }

        public Dictionary<GenerationRule<Ctx>, List<GenerationAction>> GetActions()
        {
            return Ensure<Dictionary<GenerationRule<Ctx>, List<GenerationAction>>>("actions", new Dictionary<GenerationRule<Ctx>, List<GenerationAction>>());
            //return Get<Dictionary<GenerationRule<Ctx>, List<GenerationAction>>>("actions");
        }


        public Ctx GetNeighborCtx(int xDiff, int yDiff)
        {
            var otherCoord = new GridXY(X + xDiff, Y + yDiff);

            if (World.Map.CoordinateExists(otherCoord))
            {
                return GetSubContext<GridXY, Ctx>(otherCoord);

            } else
            {
                return null;
            }

        }
       

        public void SetFromGrid(MapXY map, GridXY coord)
        {
            Set(RuleConstants.CELL_X, coord.X);
            Set(RuleConstants.CELL_Y, coord.Y);
            Set(RuleConstants.CELL_WORLD_POS, map.TransformCoordinateToWorld(coord));
            Set(RuleConstants.CELL_UNIT_WIDTH, map.CellWidth);

            // check if the cell is a single wall. 
            var neighbors = map.GetTraversable(coord);
            Set(RuleConstants.CELL_WALL_HAS_ANY, neighbors.Count != 4);
            Set(RuleConstants.CELL_PATTERN_MATCHED, false);
            //Set(RuleConstants.CELL_QUAD_NEIGHBOR_COUNT, neighbors.Count);


            Set(RuleConstants.CELL_WALL_TOP, !neighbors.Contains(new GridXY(coord.X, coord.Y - 1)));
            Set(RuleConstants.CELL_WALL_LOW, !neighbors.Contains(new GridXY(coord.X, coord.Y + 1)));
            Set(RuleConstants.CELL_WALL_RIGHT, !neighbors.Contains(new GridXY(coord.X + 1, coord.Y)));
            Set(RuleConstants.CELL_WALL_LEFT, !neighbors.Contains(new GridXY(coord.X - 1, coord.Y)));

        }

    }
}
