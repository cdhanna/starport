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
        private bool _top, _left, _right, _low;


        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }

        public bool Top { get { return _top; } set { _top = value; } }
        public bool Left { get { return _left; } set { _left = value; } }
        public bool Right { get { return _right; } set { _right = value; } }
        public bool Low { get { return _low; } set { _low = value; } }

        public GridXY(int x, int y)
        {
            _x = x;
            _y = y;
            _top = true;
            _left = true;
            _right = true;
            _low = true;
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

        public List<GridXY> GetTraversableNeighbors()
        {
            var list = new List<GridXY>();
            if (Top) list.Add(new GridXY(_x + 0, _y - 1));
            if (Left) list.Add(new GridXY(_x - 1, _y - 0));
            if (Right) list.Add(new GridXY(_x + 1, _y - 0));
            if (Low) list.Add(new GridXY(_x + 0, _y + 1));
            return list;
        }

        public override bool Equals(object obj)
        {
            if (obj is GridXY)
            {
                var other = (GridXY)obj;
                return _x == other._x
                    && _y == other._y
                    && _top == other._top
                    && _left == other._left 
                    && _right == other._right 
                    && _low == other._low
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
                + delim + _top
                + delim + _low
                + delim + _left
                + delim + _right
                ).GetHashCode();
        }

        public int GetSimilarHashCode()
        {
            const string delim = ",";
            return (
                _x + delim + _y
                )
                .GetHashCode();
        }
    }
}
