using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoTile : MonoBehaviour {


    private Vector2[] _baseUV;
    private Vector3[] _baseVerts;
	// Use this for initialization
	void Start () {
        _baseUV = GetComponent<MeshFilter>().mesh.uv;


        var x = _baseUV.ToList().Select(uv => uv * 3).ToArray();
        GetComponent<MeshFilter>().mesh.uv = x;

        _baseVerts = GetComponent<MeshFilter>().mesh.vertices;
    }

    // Update is called once per frame
    void Update () {


        var scale = transform.localScale;



	}
}
