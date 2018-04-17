using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core
{
    public interface ICoordinate<TOutCoordinate>
        where TOutCoordinate : ICoordinate<TOutCoordinate>, new()
    {
        string Key { get; }
        List<TOutCoordinate> GetNeighbors();
        //List<TOutCoordinate> GetTraversableNeighbors();

        //int GetSimilarHashCode();
    }


}
