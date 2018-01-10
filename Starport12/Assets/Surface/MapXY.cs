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
        public int CellWidth { get; set; }
        public Vector3 CellOffset { get { return new Vector3(CellWidth, 0, CellWidth)/2; } }
        public MapXY()
        {
            CellWidth = 1;
        }

        public override GridXY TransformWorldToCoordinate(Vector3 position)
        {
            var x = (int)Math.Floor(position.x / CellWidth);
            var y = (int)Math.Floor(position.z / CellWidth);

            var coord = new GridXY(x, y);
            return GetRightCoord(coord);
        }
    }
}
