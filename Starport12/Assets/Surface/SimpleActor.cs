using Smallgroup.Starport.Assets.Core;
using Smallgroup.Starport.Assets.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface
{
    public class SimpleActor : Actor
    {
        private MapXY _map;
        private GridPather _pather;

        

        public SimpleActor(MapXY map)
        {
            _map = map;
            _pather = new GridPather();
        }

        public void MoveLeft()
        {
            var current = _map.GetObjectPosition(this);
            current.X -= 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetObjectPosition(current, this);
            }
        }

        public void MoveRight()
        {
            var current = _map.GetObjectPosition(this);
            current.X += 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetObjectPosition(current, this);
            }
        }

        public void MoveUp()
        {
            var current = _map.GetObjectPosition(this);
            current.Y += 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetObjectPosition(current, this);
            }
        }

        public void MoveDown()
        {
            var current = _map.GetObjectPosition(this);
            current.Y -= 1;
            if (_map.CoordinateExists(current))
            {
                _map.SetObjectPosition(current, this);
            }
        }


        public override IEnumerable<CommandResult> ProcessCommand_Generator(ICommand command)
        {
            if (command is GotoCommand)
            {
                return HandleGoto(command as GotoCommand);
            }

            return null;
            //yield return CommandResult.COMPLETE;
        }

        private IEnumerable<CommandResult> HandleGoto(GotoCommand command)
        {
            var path = _pather.FindPath(_map, _map.GetObjectPosition(this), command.Target);

            for (var i = 0; i < path.Count; i++)
            {
                var node = path[i];
                _map.SetObjectPosition(node, this);

                var placedAt = DateTime.Now;
                var procedeAt = placedAt.AddMilliseconds(250);
                while (procedeAt > DateTime.Now)
                {
                    yield return CommandResult.WORKING;
                }
            }


            Debug.Log("DONE");
            yield return CommandResult.COMPLETE;

        }

    }
}
