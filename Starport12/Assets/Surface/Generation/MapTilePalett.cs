using Smallgroup.Starport.Assets.Surface.Generation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Tile Palett", menuName ="Map/Map Tile Palett")]
public class MapTilePalett : ScriptableObject {

    public List<MapTileSet> TileSets = new List<MapTileSet>();
}
