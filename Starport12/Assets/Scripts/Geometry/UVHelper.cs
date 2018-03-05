using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UVHelper : MonoBehaviour {

    [Serializable]
    public struct UVHelperComponent
    {
        public string name;

        public float X;
        public float Y;

        public int Length;
        public int Offset;

    }

    public List<UVHelperComponent> Components = new UVHelperComponent[] {
        new UVHelperComponent()
        {
            name = "sideA",
            X = 1,
            Y = 1,
            Length = 4,
            Offset = 0,
        }
    }.ToList();

    private Mesh _mesh;
    private Vector2[] _uvs;

	// Use this for initialization
	void Start () {
        _mesh = GetComponent<MeshFilter>().mesh;
        _uvs = _mesh.uv;
	}
	
	// Update is called once per frame
	void Update () {

        var mapped = new Vector2[_uvs.Length];
        var componentIndex = 0;
        for (var i = 0; i < _uvs.Length; i ++)
        {
            var uv = _uvs[i];
            var color = Color.red;
            var component = Components[componentIndex];
            if (i >= component.Offset && i < component.Length + component.Offset)
            {
                uv.x *= component.X;
                uv.y *= component.Y;
            } 
            if (i == component.Length + component.Offset - 1)
            {
                componentIndex = Math.Min(Components.Count -1, componentIndex + 1);
            }
            mapped[i] = uv;
        }
        //var mapped = _uvs.Select(uv =>
        //{
        //    return new Vector2(uv.x * X, uv.y * Y);
        //}).ToArray();

        
        _mesh.uv = mapped;


    }
}
