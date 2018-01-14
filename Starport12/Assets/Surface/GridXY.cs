using Smallgroup.Starport.Assets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Surface
{
    [Serializable]
    public struct GridXY : ICoordinate<GridXY>
    {

        public int _x, _y;
        
        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }

        
        public GridXY(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public List<GridXY> GetNeighbors()
        {
            return new GridXY[] {
                new GridXY(_x + 1, _y + 0),
                new GridXY(_x + 0, _y + 1),
                new GridXY(_x - 1, _y + 0),
                new GridXY(_x + 0, _y - 1),
               
            }.ToList();
        }
        

        public override string ToString()
        {
            return "(xy " + _x + ", " + _y + ")";
        }

        public override bool Equals(object obj)
        {
            if (obj is GridXY)
            {
                var other = (GridXY)obj;
                return _x == other._x
                    && _y == other._y
                    ;
            } else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            const string delim = ",";
            return (
                _x
                + delim + _y
                ).GetHashCode();
        }
        
    }
}
