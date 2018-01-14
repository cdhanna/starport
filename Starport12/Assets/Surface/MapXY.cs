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
        public Vector3 CellOffset { get { return new Vector3(CellWidth, 0, CellWidth) * -2.5f; } }
        public MapXY()
        {
            CellWidth = 1.7f;
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
