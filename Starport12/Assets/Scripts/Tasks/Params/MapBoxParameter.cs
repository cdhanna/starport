using Smallgroup.Starport.Assets.Scripts.Characters.Commands;
using Smallgroup.Starport.Assets.Scripts.MapSelect;
using Smallgroup.Starport.Assets.Surface;
using Smallgroup.Starport.Assets.Surface.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.Params
{
    [Serializable]
    public class MapBoxParameter : GameTaskParameter<SelectionResult>
    {
        public MapDataAnchor MatchPattern;
        public bool Loop;

        public bool UseMaxSize;
        public int MaxWidth;
        public int MaxHeight;

        public override SelectionResult GetDefault()
        {
            return new SelectionResult();
        }

        public BoxSelectionCommand GenerateCommand()
        {
            var command = new BoxSelectionCommand();
            command.Validator = (selection, x, y) =>
            {
                if (MaxWidth > 0 && selection.width >= MaxWidth)
                {
                    return false;
                }
                if (MaxHeight > 0 && selection.height >= MaxHeight)
                {
                    return false;
                }
                var coord = new GridXY(selection.x + x, selection.y + y);
                if (!selection.World.Map.CoordinateExists(coord))
                {
                    return false;
                }
                var cell = selection.World.Map.GetCell(coord);
                var walkable = selection.World.CellHandlers.Walkable.Process(cell);

                return walkable;
            };


            return command;
        }
    }

    [Serializable]
    public class RectContainer
    {
        public Rect Rect;
    }
}
