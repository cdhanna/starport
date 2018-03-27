using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using Smallgroup.Starport.Assets.Surface.Generation;
using Smallgroup.Starport.Assets.Surface;
using System.IO;
using System.Security.AccessControl;
using System.Diagnostics;

[CustomEditor(typeof(MapPattern))]
public class MapBitInspector : Editor {
    

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var pattern = (MapPattern)target;

        
        if (GUILayout.Button("Open"))
        {
            
            if (pattern.PatternData == null)
            {
                if (pattern.MapDataPath != null)
                {
                    AssetDatabase.ImportAsset(pattern.MapDataPath);
                    var resultPre = AssetDatabase.LoadMainAssetAtPath(pattern.MapDataPath) as GameObject;
                    pattern.PatternData = resultPre.GetComponent<MapDataAnchor>();
                } else
                {
                    var path = Application.dataPath + $"/Resources/Patterns/{pattern.name}.mft";
                    File.WriteAllBytes(path, new byte[] { 0 }); // empty file

                    var resourcePath = $"Assets/Resources/Patterns/{pattern.name}.mft";
                    AssetDatabase.ImportAsset(resourcePath);
                    var result = AssetDatabase.LoadMainAssetAtPath(resourcePath) as GameObject;
                    pattern.PatternData = result.GetComponent<MapDataAnchor>();
                    pattern.MapDataPath = resourcePath;
                }
            }

            try
            {
                var prefabPath = AssetDatabase.GetAssetPath(pattern.PatternData);
                var fullPath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + prefabPath;

                Process myProcess = new Process();
                myProcess.StartInfo.FileName = fullPath; //not the full application path
                                                         //myProcess.StartInfo.Arguments = "/A \"page=2=OpenActions\" C:\\example.pdf";
                myProcess.Start();

            } catch (Exception ex)
            {
                throw new Exception("Dont worry about this", ex);
            }

        }
        HandleGenerationButton();


    }
    
    private void HandleGenerationButton()
    {
        var pattern = (MapPattern)target;

        if (GUILayout.Button("Generate Basic"))
        {
            if (pattern.Palett == null)
            {
                EditorUtility.DisplayDialog("Error", "You must assign a palett before you can generate a basic version of this pattern", "Succumb To Program's Demands...");
                return;
            }
            if (pattern.MapDataPath != null)
            {
                AssetDatabase.ImportAsset(pattern.MapDataPath);
                var resultPre = AssetDatabase.LoadMainAssetAtPath(pattern.MapDataPath) as GameObject;
                pattern.PatternData = resultPre.GetComponent<MapDataAnchor>();
                pattern.GenerateFloors();
            } else
            {
                EditorUtility.DisplayDialog("Error", "We couldnt find the .mft resource I guess. Sorry bro.", "Go home.");

            }

        }
    }
    //private List<bool> isLayerOpen = new List<bool>();

    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();

    //    var mapBit = (MapPattern)target;
    //    if (mapBit.Layers == null)
    //    {
    //        mapBit.Layers = new List<MapPatternLayer>();
    //    }

    //    mapBit.Width = EditorGUILayout.IntField("Pattern Width", mapBit.Width);
    //    mapBit.Height = EditorGUILayout.IntField("Pattern Height", mapBit.Height);

    //    DoLayers(mapBit);


    //    //DoPattern(mapBit);

    //    if (GUILayout.Button("Center Children"))
    //    {
    //        mapBit.Center();
    //    }

    //    if (GUILayout.Button("Generate Floor"))
    //    {
    //        //mapBit.GenerateFloors();
    //        var map = MapLoader.LoadFromPattern(mapBit.Palett, mapBit);
    //        MapLoader.ApplyRules(map, new PatternSet());
    //    }

    //}

    //private void OnSceneGUI()
    //{
    //    var mapBit = (MapPattern)target;
    //    if (mapBit.Layers == null)
    //    {
    //        mapBit.Layers = new List<MapPatternLayer>();
    //    }


    //    Handles.color = Color.white;
    //    GUIStyle style = new GUIStyle();
    //    style.normal.textColor = Color.green;
    //    style.fontSize = 18;
    //    //Handles.Label(mapBit.transform.position, "Hello World", style);

    //    //for (var i = 0; i < mapBit.pattern.Length; i++)
    //    //{
    //    //    for (var j = 0; j < mapBit.pattern[i].Length; j++)
    //    //    {
    //    //        Handles.Label(mapBit.GetPositionAtCell(i, j), $"{j},{i}: {mapBit.pattern[i][j]}", style);
    //    //    }
    //    //}

    //    //Handles.BeginGUI();
    //    //if (GUILayout.Button("Reset Area", GUILayout.Width(100)))
    //    //{
    //    //    //handleExample.shieldArea = 5;
    //    //}
    //    //Handles.EndGUI();


    //}


    //private void DoLayers(MapPattern pattern)
    //{
    //    EditorGUILayout.BeginHorizontal();
    //    GUILayout.Label("Pattern Layers");

    //    if (GUILayout.Button("Add Layer"))
    //    {
    //        pattern.Layers.Add(new MapPatternLayer()
    //        {
    //            LayerName = "Unnamed Layer",
    //            Data = new List<CellTemplate>()
    //        });
    //        isLayerOpen = pattern.Layers.Select((layer, index) =>
    //       {
    //           if (isLayerOpen.Count > index)
    //           {
    //               return isLayerOpen[index];
    //           }
    //           return false;
    //       }).ToList();
    //    }
    //    EditorGUILayout.EndHorizontal();


    //    var layersToDelete = new List<MapPatternLayer>();
    //    for (var i = 0; i < pattern.Layers.Count; i++)
    //    {
    //        var layer = pattern.Layers[i];

    //        if (isLayerOpen.Count >= i)
    //        {
    //            isLayerOpen.Add(false);
    //        }
    //        var isFoldedOut = isLayerOpen[i];

    //        EditorGUILayout.BeginHorizontal();


    //        isLayerOpen[i] = EditorGUILayout.Foldout(isFoldedOut, "");
    //        EditorGUILayout.LabelField("Layer Name:", GUILayout.Width(100));
    //        layer.LayerName = EditorGUILayout.TextField(layer.LayerName, GUILayout.ExpandWidth(true));
    //        if (GUILayout.Button("Remove", GUILayout.MaxWidth(80)))
    //        {
    //            layersToDelete.Add(layer);
    //            continue;
    //        }
    //        EditorGUILayout.EndHorizontal();
    //        if (isFoldedOut)
    //        {
    //            EditorGUI.indentLevel += 1;
    //            //EditorGUILayout.LabelField("Howdy");
    //            DoColorPattern(pattern, layer);

    //            EditorGUI.indentLevel -= 1;
    //        }
    //    }
    //    layersToDelete.ForEach(l => pattern.Layers.Remove(l));


    //}

    //private void DoColorPattern(MapPattern pattern, MapPatternLayer layer)
    //{
    //    var i = 0;
    //    for (var y = 0; y < pattern.Height; y++)
    //    {

    //        GUILayout.BeginHorizontal();
    //        for (var x = 0; x < pattern.Width; x++)
    //        {

    //            if (i >= layer.Data.Count)
    //            {
    //                layer.Data.Add(CellTemplates.Empty);
    //            }
    //            layer.Data[i] = (CellTemplate) EditorGUILayout.ObjectField(layer.Data[i], typeof(CellTemplate), true);
    //            //layer.Data[i] = EditorGUILayout.ColorField(layer.Data[i]);
    //            i += 1;
    //            //EditorGUI.DrawRect(new Rect(y, x, 10, 10), Color.red);
    //        }
    //        GUILayout.EndHorizontal();
    //    }

    //}

    //private void DoPattern(MapPattern mapBit)
    //{
    //    var combined = "";
    //    foreach (var line in mapBit.pattern)
    //    {
    //        combined += line + "\n";
    //    }
    //    GUILayout.Label("Pattern");
    //    var output = GUILayout.TextArea(combined);
    //    var parts = output.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
    //    mapBit.pattern = parts.ToArray();

    //}

}
