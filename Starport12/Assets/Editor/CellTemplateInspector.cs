using Smallgroup.Starport.Assets.Surface.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CellTemplate))]
public class CellTemplateInspector : Editor
{
    public override void OnInspectorGUI()
    {
        var template = (CellTemplate)target;

        template.name = EditorGUILayout.TextField("Name", template.name);

        base.OnInspectorGUI();




        var inputColor = new Color(template.Red / 255f, template.Green / 255f, template.Blue / 255f, template.Alpha / 255f);
        var outputColor = EditorGUILayout.ColorField(inputColor);

        template.Red = (byte)Math.Floor(outputColor.r * 255);
        template.Green = (byte)Math.Floor(outputColor.g * 255);
        template.Blue = (byte)Math.Floor(outputColor.b * 255);
        template.Alpha = (byte)Math.Floor(outputColor.a * 255);


    }
}
