using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Flat))]
public class FlatEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Flat targetScript = (Flat)target;

        if (GUILayout.Button("Generate"))
            targetScript.Generate();

        if (GUILayout.Button("Reset"))
            targetScript.RemoveExistingBuilding();
    }
}
