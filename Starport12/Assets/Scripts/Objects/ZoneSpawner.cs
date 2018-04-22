using Smallgroup.Starport.Assets.Scripts;
using Smallgroup.Starport.Assets.Surface.Generation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneSpawner : MonoBehaviour {

    public MapZone Zone;
    public GameObject Prefab;

    private bool _spawned;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!_spawned){

            var world = GameObject.FindObjectOfType<WorldAnchor>();
            var possibleCells = world.Map.Coordinates.Select(xy => world.Map.GetCell(xy)).Where(c => {
                var proc = world.Map.Handlers.Zones.Process(c);
                return proc == Zone;
            }).ToList();

            float n = UnityEngine.Random.Range(0, possibleCells.Count);
            var startCell = possibleCells[(int)(Math.Round(n, 1))];
            var startCoord = world.Map.GetCoordinate(startCell);
            var startPos = world.Map.TransformCoordinateToWorld(startCoord);

            var obj = Instantiate(Prefab, world.transform);
            obj.transform.position = startPos;

            //world.Map.SetObjectPosition(startCoord, Actor);

            _spawned = true;
        }
	}
}
