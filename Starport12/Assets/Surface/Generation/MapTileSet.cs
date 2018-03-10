using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    [CreateAssetMenu(fileName = "MapTileSet", menuName = "Map/Map Tile Set")]
    public class MapTileSet : ScriptableObject
    {

        public char WalkableCode;
        public char FillCode;

        public GameObject FloorPrefab;
        public GameObject WallPrefab;
        public GameObject JoinPrefab;
        public GameObject CornerJoinPrefab;

    }
}
