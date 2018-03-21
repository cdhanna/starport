using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using Smallgroup.Starport.Assets.Surface.Generation;
using Smallgroup.Starport.Assets.Surface;

[CustomEditor(typeof(MapPattern))]
public class MapBitInspector : Editor {


    private List<bool> isLayerOpen = new List<bool>();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var mapBit = (MapPattern)target;
        if (mapBit.Layers == null)
        {
            mapBit.Layers = new List<MapPatternLayer>();
        }

        mapBit.Width = EditorGUILayout.IntField("Pattern Width", mapBit.Width);
        mapBit.Height = EditorGUILayout.IntField("Pattern Height", mapBit.Height);

        DoLayers(mapBit);


        //DoPattern(mapBit);

        if (GUILayout.Button("Center Children"))
        {
            mapBit.Center();
        }

        if (GUILayout.Button("Generate Floor"))
        {
            //mapBit.GenerateFloors();
            MapLoader.ApplyRules(new MapXY(), new PatternSet() );
        }

    }

    private void OnSceneGUI()
    {
        var mapBit = (MapPattern)target;
        if (mapBit.Layers == null)
        {
            mapBit.Layers = new List<MapPatternLayer>();
        }
        mapBit.pattern = new string[] { };


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


    private void DoLayers(MapPattern pattern)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Pattern Layers");

        if (GUILayout.Button("Add Layer"))
        {
            pattern.Layers.Add(new MapPatternLayer()
            {
                LayerName = "Unnamed Layer",
                Data = new List<Color>()
            });
            isLayerOpen = pattern.Layers.Select((layer, index) =>
           {
               if (isLayerOpen.Count > index)
               {
                   return isLayerOpen[index];
               }
               return false;
           }).ToList();
        }
        EditorGUILayout.EndHorizontal();


        var layersToDelete = new List<MapPatternLayer>();
        for (var i = 0; i < pattern.Layers.Count; i++)
        {
            var layer = pattern.Layers[i];
            var isFoldedOut = isLayerOpen[i];

            EditorGUILayout.BeginHorizontal();
            
            
            isLayerOpen[i] = EditorGUILayout.Foldout(isFoldedOut, "");
            EditorGUILayout.LabelField("Layer Name:", GUILayout.Width(100));
            layer.LayerName = EditorGUILayout.TextField(layer.LayerName, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Remove", GUILayout.MaxWidth(80)))
            {
                layersToDelete.Add(layer);
                continue;
            }
            EditorGUILayout.EndHorizontal();
            if (isFoldedOut)
            {
                EditorGUI.indentLevel += 1;
                //EditorGUILayout.LabelField("Howdy");
                DoColorPattern(pattern, layer);

                EditorGUI.indentLevel -= 1;
            }
        }
        layersToDelete.ForEach(l => pattern.Layers.Remove(l));


    }

    private void DoColorPattern(MapPattern pattern, MapPatternLayer layer)
    {
        var i = 0;
        for (var y = 0; y < pattern.Height; y++)
        {

            GUILayout.BeginHorizontal();
            for (var x = 0; x < pattern.Width; x++)
            {
                
                if (i >= layer.Data.Count)
                {
                    layer.Data.Add(new Color(1, 1, 1, 0));
                }
                layer.Data[i] = EditorGUILayout.ColorField(layer.Data[i]);
                i += 1;
                //EditorGUI.DrawRect(new Rect(y, x, 10, 10), Color.red);
            }
            GUILayout.EndHorizontal();
        }

    }

    private void DoPattern(MapPattern mapBit)
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
