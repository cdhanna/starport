using Smallgroup.Starport.Assets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface
{
    public class Cell : ICell<Cell>
    {
        
        public bool Walkable { get; set; }
        public char Code { get; set; }
        public string Type { get; set; }

        public GameObject DefaultFloorAsset { get; set; }
        public GameObject DefaultWallAsset { get; set; }
        public GameObject DefaultJoinAsset { get; set; }
        public GameObject DefaultCornerJoinAsset { get; set; }

    }
    
    public class StoneCell : Cell
    {
        public const string TYPE = "Stone";

        public StoneCell(bool walkable, char code)
        {
            Type = TYPE;
            Walkable = walkable;
            Code = code;
        }
    }

    public class DirtCell : Cell
    {
        public const string TYPE = "DIRT";

        public DirtCell(bool walkable, char code)
        {
            Type = TYPE;
            Walkable = walkable;
            Code = code;
        }
    }

}
