using Smallgroup.Starport.Assets.Core;
using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Surface
{
    public class SimpleActor : Actor
    {
        private MapXY _map;

        public SimpleActor(MapXY map)
        {
            _map = map;
        }

        public void MoveLeft()
        {
            var current = _map.GetPosition(this);
            current.X -= 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetPosition(this, current);
            }
        }

        public void MoveRight()
        {
            var current = _map.GetPosition(this);
            current.X += 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetPosition(this, current);
            }
        }

        public void MoveUp()
        {
            var current = _map.GetPosition(this);
            current.Y += 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetPosition(this, current);
            }
        }

        public void MoveDown()
        {
            var current = _map.GetPosition(this);
            current.Y -= 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetPosition(this, current);
            }
        }
    }
}
