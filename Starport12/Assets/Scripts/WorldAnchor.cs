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
using UnityEngine.AI;

namespace Smallgroup.Starport.Assets.Scripts
{
    public class WorldAnchor : MonoBehaviour
    {
        public MapXY Map { get { return Results.Output; } }

        public DialogAnchor DialogAnchor;
        public ActorAnchor[] Players;
        //public MapLoader MapLoader;
        public MapTilePalett TilePalett;
        public PatternSet PatternSet;
        public int MapWidth = 64;
        public int MapHeight = 64;

        public MapDataAnchor MapData;

        public List<SuperRule> AdditionalRules;

        [HideInInspector]
        public RuleAppliedResults Results { get; set; }
        //public WalkableHandler WalkableHandler = new WalkableHandler();
        //public RoomTypeNameHandler RoomTypeNameHandler = new RoomTypeNameHandler(new Dictionary<Color, string> {
        //    { ColorGen.FromRGB(128, 128, 128, 255), "stone"},
        //    { ColorGen.FromRGB(127, 51, 0, 255), "dirt" }
        //});

        public CellHandlers CellHandlers;
        public List<MapZone> Zones;

        private void AttachCellAnchors()
        {
            for (var i = 0; i < Map.Coordinates.Length; i++)
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
                cellObj.transform.localScale = new Vector3(Map.CellWidth * scale, .1f, Map.CellWidth * scale);
            }
        }

        public void GetSubset(int x, int y, int width, int height)
        {

        }

        protected void Awake()
        {

            CellHandlers = new CellHandlers(TilePalett);
            CellHandlers.Zones.Zones = Zones;

            var map = MapLoader.LoadFromMFT(CellHandlers, MapData.Raw);
            //World.Map = MapLoader.LoadFromFile();
            

            Results = MapLoader.ApplyRules(null, map, PatternSet, AdditionalRules, null);

            var insetResults = MapLoader.InsetMap(map, MapWidth, MapHeight);

            Results.Join(insetResults);

            World.Map = Map;
            AttachCellAnchors();

            RebuildNav();
        }

        public void RebuildNav()
        {
            GetComponent<NavMeshSurface>().BuildNavMesh();

        }

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                GetComponent<NavMeshSurface>().BuildNavMesh();
            }




        }

       

    }
}
