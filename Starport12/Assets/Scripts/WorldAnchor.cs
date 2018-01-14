using Smallgroup.Starport.Assets.Core;
using Smallgroup.Starport.Assets.Surface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts
{
    class WorldAnchor : MonoBehaviour
    {
            
        public MapXY Map { get; set; }
        public bool DebugVisuals = true;
        public Color DebugColor = Color.blue;

        public ActorAnchor Player;

        public WorldAnchor()
        {
            Map = World.Map;

            //Map.SetPosition()
        }


        private void SetSampleLevel()
        {

            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 12; y++)
                {

                    if (  (x == 4 && y == 4)
                        || (x == 5 && y == 4)
                        || (x == 4 && y == 5)
                        || (x == 7 && y == 4)
                        || (x == 4 && y == 6))
                    {
                        continue;
                    }

                    var coord = new GridXY(x, y);

                    var cell = new Cell();
                    var cellObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    var cellScript = cellObj.AddComponent<CellAnchor>();
                    cellScript.Cell = cell;

                    cellObj.transform.parent = transform;
            
                    cellObj.transform.localPosition = World.Map.TransformCoordinateToWorld(coord);

                    float scale = .9f;
                    cellObj.transform.localScale = new Vector3(World.Map.CellWidth * scale, .1f, World.Map.CellWidth * scale);
                    Map.SetCell(coord, cell);
                }
            }
            Map.AutoMap();
            /*
             * 
             * map.GetTraversable(coord) -> coord[]
             * map.SetTraversable(coord, coord[])
             * 
             */ 
        }

        protected void Start()
        {
            SetSampleLevel();

            //Map.SetPosition(Player.Actor, new GridXY(1, 3));
        }

        protected void Update()
        {
        }

       

    }
}
