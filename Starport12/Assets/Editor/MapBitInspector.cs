using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

[CustomEditor(typeof(MapBit))]
public class MapBitInspector : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var mapBit = (MapBit)target;

        DoPattern(mapBit);

        if (GUILayout.Button("Center Children"))
        {
            mapBit.Center();
        }

        if (GUILayout.Button("Generate Floor"))
        {
            mapBit.GenerateFloors();
        }

    }

    private void OnSceneGUI()
    {
        var mapBit = (MapBit)target;

        Handles.color = Color.white;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.green;
        style.fontSize = 18;
        //Handles.Label(mapBit.transform.position, "Hello World", style);
        
        for (var i = 0; i < mapBit.pattern.Length; i++)
        {
            for (var j = 0; j < mapBit.pattern[i].Length; j++)
            {
                Handles.Label(mapBit.GetPositionAtCell(i, j), $"{j},{i}: {mapBit.pattern[i][j]}", style);
            }
        }

        //Handles.BeginGUI();
        //if (GUILayout.Button("Reset Area", GUILayout.Width(100)))
        //{
        //    //handleExample.shieldArea = 5;
        //}
        //Handles.EndGUI();


    }



    private void DoPattern(MapBit mapBit)
    {
        var combined = "";
        foreach (var line in mapBit.pattern)
        {
            combined += line + "\n";
        }
        GUILayout.Label("Pattern");
        var output = GUILayout.TextArea(combined);
        var parts = output.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        mapBit.pattern = parts.ToArray();

    }

}
