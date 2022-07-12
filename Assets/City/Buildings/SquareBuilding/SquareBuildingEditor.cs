using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SquareBuilding))]
public class SquareBuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SquareBuilding targetScript = (SquareBuilding)target;

        if (GUILayout.Button("Generate"))
            targetScript.Generate();

        if (GUILayout.Button("Reset"))
            targetScript.RemoveExistingBuilding();
    }
}
