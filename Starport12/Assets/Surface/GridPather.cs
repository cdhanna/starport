using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Surface
{
    public class GridPather
    {

        public List<GridXY> FindPath(MapXY map, GridXY start, GridXY end)
        {

            // simple search
            var toExplore = new Queue<GridXY>();
            var explored = new List<GridXY>();
            var trace = new Dictionary<GridXY, GridXY>();
            toExplore.Enqueue(start);

            while (toExplore.Count > 0)
            {
                var current = toExplore.Dequeue();
                if (explored.Contains(current))
                {
                    continue; // jump to next cycle.
                }
                if (current.Equals(end))
                {
                    //trace.Add(end, current);

                    // find full path.
                    var path = new List<GridXY>();
                    var node = end;
                    var hasTrace = true;
                    while (hasTrace)
                    {
                        var back = default(GridXY);
                        if (trace.TryGetValue(node, out back))
                        {
                            path.Add(back);
                            node = back;
                        } else
                        {
                            hasTrace = false;
                        }
                    }
                    path.Reverse();
                    path.Add(end);
                    return path;

                    break; // exit loop
                }
                explored.Add(current);

                var neighbors = map.GetTraversable(current);
                foreach (var neighbor in neighbors)
                {
                    if (!explored.Contains(neighbor))
                    {
                        if (!trace.ContainsKey(neighbor))
                        {
                            trace.Add(neighbor, current);

                        }
                        toExplore.Enqueue(neighbor);
                    }
                }
            }

            throw new InvalidOperationException("No path exists.");
            return new List<GridXY>();
        }

    }
}
