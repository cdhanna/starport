using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UVDensityHelper : MonoBehaviour {

    public float X = 1, Y = 1;

    private Mesh _mesh;

    private Dictionary<int, int> _indexToGroup = new Dictionary<int, int>();
    private Dictionary<Vector3, List<int>> _normalToGroup = new Dictionary<Vector3, List<int>>();

    private Dictionary<int, Vector2> _indexToScale = new Dictionary<int, Vector2>();
    private Dictionary<int, Vector3> _indexToStretchX = new Dictionary<int, Vector3>();
    private Dictionary<int, Vector3> _indexToStretchY = new Dictionary<int, Vector3>();

    private Vector2[] _uvs;

	// Use this for initialization
	void Start () {
        //_indexToGroup = new Dictionary<int, int>();
        //_normalToGroup = new Dictionary<Vector3, List<int>>();

        _mesh = GetComponent<MeshFilter>().mesh;
        _uvs = _mesh.uv;
        var normals = _mesh.normals;
        var verts = _mesh.vertices;

        for (var i = 0; i < normals.Length; i++)
        {
            var existingGroup = new List<int>();
            if (_normalToGroup.TryGetValue(normals[i], out existingGroup))
            {
                existingGroup.Add(i);
            } else
            {
                _normalToGroup.Add(normals[i], new int[] { i }.ToList());
            }
        }

        var seenRights = new List<Vector3>();
        var seenDowns = new List<Vector3>();
        foreach (var kv in _normalToGroup)
        {
            var normal = kv.Key;
            var group = kv.Value;

            var right = verts[group[1]] - verts[group[0]];
            var down = verts[group[2]] - verts[group[0]];

            right = Vector3.Cross(normal, Vector3.up);
            if (right.magnitude < .01f)
            {
                right = Vector3.Cross(normal, Vector3.forward);
            }
            down = Vector3.Cross(normal, Vector3.right);
            if (down.magnitude < .01f)
            {
                down = Vector3.Cross(normal, Vector3.forward);
            }



            for (var i = 0; i < group.Count; i++)
            {
                var index = group[i];

                _indexToStretchX.Add(index, right);
                _indexToStretchY.Add(index, down);
                
            }


        }


    }
	
	// Update is called once per frame
	void Update () {

        var mapped = new Vector2[_uvs.Length];
        for (var i = 0; i < mapped.Length; i++)
        {
            var uv = _uvs[i];
            mapped[i] = uv;
            var right = _indexToStretchX[i];
            var down = _indexToStretchY[i];
            var compRight = Mathf.Abs(Vector3.Dot(transform.lossyScale, right));
            var compDown = Mathf.Abs(Vector3.Dot(transform.lossyScale, down));
            //var scale = _indexToScale[i];
            //var x = Vector3.Dot(_indexToStretchX[i], transform.lossyScale);
            mapped[i] = new Vector2(_uvs[i].x * compRight * X, _uvs[i].y * compDown * Y);
        }
        _mesh.uv = mapped;

	}
}
