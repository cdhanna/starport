using Smallgroup.Starport.Assets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface
{
    class MapXY : Map<GridXY>
    {
        public int CellWidth { get; set; }
        public Vector3 CellOffset { get { return new Vector3(CellWidth, 0, CellWidth)/2; } }
        public MapXY()
        {
            CellWidth = 1;
        }
    }
}
