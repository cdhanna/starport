using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core
{
    interface ICoordinate<TOutCoordinate>
        where TOutCoordinate : ICoordinate<TOutCoordinate>, new()
    {

        IEnumerable<TOutCoordinate> GetNeighbors();

    }


}
