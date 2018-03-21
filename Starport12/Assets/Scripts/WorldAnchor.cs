using Dialog.Engine;
using Smallgroup.Starport.Assets.Core;
using Smallgroup.Starport.Assets.Core.Generation;
using Smallgroup.Starport.Assets.Surface;
using Smallgroup.Starport.Assets.Surface.Generation;
using Smallgroup.Starport.Assets.Surface.Generation.Rules;
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

        public DialogAnchor DialogAnchor;
        public ActorAnchor[] Players;
        public MapLoader MapLoader;

        private void AttachCellAnchors()
        {
            for (var i = 0; i <  Map.Coordinates.Length; i++)
            {
                var coord = Map.Coordinates[i];
                var cell = Map.GetCell(coord);

                var cellObj = new GameObject();
                //var cellObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
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
            World.Map = MapLoader.LoadFromMFT();
            //World.Map = MapLoader.LoadFromFile();
            Map = World.Map;
            AttachCellAnchors();

            MapLoader.ApplyRules(Map, MapLoader.GenerationPatternSet);
            
        }

        protected void Update()
        {

        }

       

    }
}
