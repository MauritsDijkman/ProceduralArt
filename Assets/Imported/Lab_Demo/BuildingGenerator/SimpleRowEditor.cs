using Demo;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleRow))]
public class SimpleRowEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SimpleRow targetScript = (SimpleRow)target;

        if (GUILayout.Button("Generate"))
            targetScript.Generate();

        if (GUILayout.Button("Reset"))
            targetScript.RemoveExistingBuilding();
    }
}
