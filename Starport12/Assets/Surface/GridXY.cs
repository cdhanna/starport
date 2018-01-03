using Smallgroup.Starport.Assets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Surface
{
    struct GridXY : ICoordinate<GridXY>
    {

        private int _x, _y;

        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }

        public GridXY(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public IEnumerable<GridXY> GetNeighbors()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (obj is GridXY)
            {
                var other = (GridXY)obj;
                return _x == other._x
                    && _y == other._y;
            } else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (_x + "," + _y).GetHashCode();
        }
    }
}
