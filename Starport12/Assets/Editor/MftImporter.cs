using Smallgroup.Starport.Assets.Surface.Generation;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, "mft")]
public class MftImporter : ScriptedImporter {


    [SerializeField]
    public string IconPath = "icons/jafar-gear";

    public override void OnImportAsset(AssetImportContext ctx)
    {
        var bytes = File.ReadAllBytes(ctx.assetPath);

        var tex = Resources.Load(IconPath) as Texture2D;

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        DestroyImmediate(cube.GetComponent<MeshRenderer>());
        DestroyImmediate(cube.GetComponent<BoxCollider>());
        DestroyImmediate(cube.GetComponent<MeshFilter>());

        var data = cube.AddComponent<MapDataAnchor>();
        //data.MapFile = MapFileCodec.Converter.FromBytes(bytes);
        data.Raw = bytes;
        data.ByteSize = bytes.Length;
        ctx.AddObjectToAsset("MainAsset", cube, tex);
        //ctx.AddObjectToAsset("Data", data);
        ctx.SetMainObject(cube);
        
        //throw new System.NotImplementedException();
    }
    
}
