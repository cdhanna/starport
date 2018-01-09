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
            SetSampleLevel();

            //Map.SetPosition()
        }


        private void SetSampleLevel()
        {

            for (var x = 0; x < 6; x++)
            {
                for (var y = 0; y < 12; y++)
                {
                    Map.Set(new GridXY(x, y), new Cell());
                }
            }
        }

        protected void Start()
        {

            //Map.SetPosition(Player.Actor, new GridXY(1, 3));
        }

        protected void Update()
        {
            DrawDebugLines();
        }

        private void DrawDebugLines()
        {
            if (!DebugVisuals) return;

            foreach(var coord in Map.Coordinates)
            {
                var cornerTopLeft = new Vector3(coord.X, 0, coord.Y) * Map.CellWidth;
                Debug.DrawLine(
                    cornerTopLeft + Vector3.right * Map.CellWidth * 0 + Vector3.forward * Map.CellWidth * 0,
                    cornerTopLeft + Vector3.right * Map.CellWidth * 1 + Vector3.forward * Map.CellWidth * 0,
                    DebugColor);

                Debug.DrawLine(
                    cornerTopLeft + Vector3.right * Map.CellWidth * 0 + Vector3.forward * Map.CellWidth * 0,
                    cornerTopLeft + Vector3.right * Map.CellWidth * 0 + Vector3.forward * Map.CellWidth * 1,
                    DebugColor);

                Debug.DrawLine(
                    cornerTopLeft + Vector3.right * Map.CellWidth * 1 + Vector3.forward * Map.CellWidth * 0,
                    cornerTopLeft + Vector3.right * Map.CellWidth * 1 + Vector3.forward * Map.CellWidth * 1,
                    DebugColor);

                Debug.DrawLine(
                    cornerTopLeft + Vector3.right * Map.CellWidth * 0 + Vector3.forward * Map.CellWidth * 1,
                    cornerTopLeft + Vector3.right * Map.CellWidth * 1 + Vector3.forward * Map.CellWidth * 1,
                    DebugColor);
            }
        }

    }
}
