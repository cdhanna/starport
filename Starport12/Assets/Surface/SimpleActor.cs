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
            yield return CommandResult.WORKING;

            _map.SetObjectPosition(command.Target, this);

            yield return CommandResult.COMPLETE;

        }

    }
}
