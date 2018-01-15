using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Surface
{
    static class World
    {
        public static MapXY Map;

        static World()
        {
            var loader = new MapLoader();
            Map = loader.LoadFromFile("Assets\\Maps\\testmap.json");
        }
    }
}
