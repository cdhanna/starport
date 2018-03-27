using Smallgroup.Starport.Assets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface
{
    public class MapXY : Map<GridXY, Cell>
    {
        public float CellWidth { get; set; }
        public Vector3 CellOffset { get; set; }
        public CellHandlers Handlers { get; set; }
        public MapXY(CellHandlers handlers)
        {
            Handlers = handlers;
            CellWidth = 1.7f;
            CellOffset = Vector3.zero;
        }

        public int HighestX { get
            {
                var best = int.MinValue;
                Coordinates.ToList().ForEach(c => best = Math.Max(c.X, best));
                return best;
            }
        }
        public int HighestY
        {
            get
            {
                var best = int.MinValue;
                Coordinates.ToList().ForEach(c => best = Math.Max(c.Y, best));
                return best;
            }
        }
        public int LowestX
        {
            get
            {
                var best = int.MaxValue;
                Coordinates.ToList().ForEach(c => best = Math.Min(c.X, best));
                return best;
            }
        }

        public override GridXY TransformWorldToCoordinate(Vector3 position)
        {
            // TRANSLATE --> SCALE
            var x = (int)(Math.Round((position.x - CellOffset.x) / CellWidth));
            var y = (int)(Math.Round((position.z - CellOffset.z) / CellWidth));

            var coord = new GridXY(x, y);
            return coord;
        }

        public override Vector3 TransformCoordinateToWorld(GridXY coordinate)
        {
            // SCALE --> TRANSLATE

            return new Vector3(
                coordinate.X * CellWidth,
                0,
                coordinate.Y * CellWidth) + CellOffset;
        }
    }
}
