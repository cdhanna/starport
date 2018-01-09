using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core
{
    interface ICell<TCellType>
        where TCellType : ICell<TCellType>,new()
    {
        //TCoordinateType Coord { get; }
        //ICell GetTraversable
    }
}
