using Smallgroup.Starport.Assets.Surface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.MapSelect
{
    public class SelectionResult
    {

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<GameObject> OverlappingObjects { get; set; }
        public WorldAnchor World { get; set; }

        public List<GridXY> GetCoordinates(int padding = 0)
        {
            var output = new List<GridXY>();
            for (var i = X - padding; i <= X + Width + padding; i++)
            {
                for (var j = Y - padding; j <= Y + Height + padding; j++)
                {
                    output.Add(new GridXY(i, j));
                }
            }
            return output;
        }

        public List<GridXY> GetRelativeCoordinates()
        {
            var output = new List<GridXY>();
            for (var i = 0; i <= Width; i++)
            {
                for (var j = 0; j <= Height; j++)
                {
                    output.Add(new GridXY(i, j));
                }
            }
            return output;
        }

    }
}
