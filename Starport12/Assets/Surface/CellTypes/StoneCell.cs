using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Surface.CellTypes
{
    class StoneCell : Cell
    {
        public StoneCell(bool walkable)
        {
            Walkable = walkable;
            TypeName = "stone";
            FillPrefabName = "stone_fill";
        }
    }

    class PalaceCell : Cell
    {

    }

    class DirtCell : Cell
    {
        public DirtCell(bool walkable)
        {
            Walkable = walkable;
            TypeName = "dirt";
            FillPrefabName = "dirt_fill";
        }
    }
}
