using Smallgroup.Starport.Assets.Surface;
using Smallgroup.Starport.Assets.Surface.Generation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//[Serializable]
//public class MapPatternLayer
//{
//    public string LayerName;
//    public List<CellTemplate> Data;
//}

[Serializable]
public class MapPattern : MonoBehaviour {


    //public GameObject FloorPrefab;

    public MapTilePalett Palett;
    public MapDataAnchor PatternData;

    [HideInInspector]
    public string MapDataPath;

    //[HideInInspector]
    //public List<MapPatternLayer> Layers;
    [HideInInspector]
    public int Width;
    [HideInInspector]
    public int Height;

    private GameObject _basicGroup;
    //[HideInInspector]
    //public string[] pattern;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void GenerateFloors()
    {
        //while (transform.childCount > 0)
        //{
        //    DestroyImmediate(transform.GetChild(0).gameObject);
        //}

        if (_basicGroup != null)
        {
            DestroyImmediate(_basicGroup);
        }

        var group = GameObject.CreatePrimitive(PrimitiveType.Cube);
        DestroyImmediate(group.GetComponent<MeshFilter>());
        DestroyImmediate(group.GetComponent<MeshRenderer>());
        DestroyImmediate(group.GetComponent<BoxCollider>());
        group.name = "_basic";
        group.transform.SetParent(transform);
        _basicGroup = group;

        var mapXY = MapLoader.LoadFromPattern(new CellHandlers(Palett), this);
        mapXY.CellWidth = 1;
        mapXY.CellOffset = new Vector3(mapXY.HighestX, 0, mapXY.HighestY + 1) * -.5f;
        var all = MapLoader.ApplyRules(mapXY, new PatternSet());
        all.Where(obj => obj.transform.parent == null)
            .ToList()
            .ForEach(obj => obj.transform.SetParent(group.transform));
    }

    //public Vector3 GetPositionAtCell(int i, int j)
    //{
    //    var maxLength = Layers.Max(s => s.Data.Count);

    //    return new Vector3(j - ((maxLength-1) / 2f), 0, -i + ((Layers.Count-1) / 2f));
    //}

    public void Center()
    {
        var count = transform.childCount;
        for (var i = 0; i < count; i++)
        {
            var child = transform.GetChild(i);


        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        
        //Gizmos.DrawIcon(Vector3.zero, )
    }

}
