using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class MapPatternLayer
{
    public string LayerName { get; set; }
    public List<Color> Data { get; set; }
}

[Serializable]
public class MapPattern : MonoBehaviour {


    public GameObject FloorPrefab;

    public MapTilePalett Palett;

    public List<MapPatternLayer> Layers { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    [HideInInspector]
    public string[] pattern;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void GenerateFloors()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }


        for (var i = 0; i < pattern.Length; i++)
        {
            for (var j = 0; j < pattern[i].Length; j++)
            {
                var tile = Instantiate(FloorPrefab, transform);
                tile.transform.localPosition = GetPositionAtCell(i, j);
            }
        }
    }

    public Vector3 GetPositionAtCell(int i, int j)
    {
        var maxLength = pattern.Max(s => s.Length);

        return new Vector3(j - ((maxLength-1) / 2f), 0, -i + ((pattern.Length-1) / 2f));
    }

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
