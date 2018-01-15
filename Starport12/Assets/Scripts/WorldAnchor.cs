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

        public ActorAnchor[] Players;

   
        private void AttachCellAnchors()
        {
            for (var i = 0; i <  Map.Coordinates.Length; i++)
            {
                var coord = Map.Coordinates[i];
                var cell = Map.GetCell(coord);
                var cellObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var cellScript = cellObj.AddComponent<CellAnchor>();
                cellScript.Cell = cell;

                cellObj.transform.parent = transform;

                cellObj.transform.localPosition = World.Map.TransformCoordinateToWorld(coord);

                float scale = .9f;
                cellObj.transform.localScale = new Vector3(World.Map.CellWidth * scale, .1f, World.Map.CellWidth * scale);
            }
        }

        protected void Start()
        {

            AttachCellAnchors();
            Map = World.Map;
        }

        protected void Update()
        {

        }

       

    }
}
