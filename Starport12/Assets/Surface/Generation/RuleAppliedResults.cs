using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    public class RuleAppliedResults
    {
        public List<GameObject> GeneratedObjects { get; set; }
        public Ctx Global { get; set; }
        public MapXY Output { get; set; }


        public void Join(RuleAppliedResults other)
        {
            // join the mapxy data.
            if (Output != other.Output)
            {

                var otherMap = other.Output;
                var otherCoords = otherMap.Coordinates;
                for (var c = 0; c < otherCoords.Length; c++)
                {
                    var coord = otherCoords[c];
                    var otherCell = otherMap.GetCell(coord);
                    var traverasble = otherMap.GetTraversable(coord);
                    Output.SetCell(coord, otherCell);
                    Output.SetTraversable(coord, traverasble);
                }
            }

            // combine object listing
            if (GeneratedObjects != other.GeneratedObjects)
            {
                GeneratedObjects.AddRange(other.GeneratedObjects);
            }

            // merge contexts
            if (Global != other.Global)
            {

                Global.Merge(other.Global);
            }

        }

    }
}
