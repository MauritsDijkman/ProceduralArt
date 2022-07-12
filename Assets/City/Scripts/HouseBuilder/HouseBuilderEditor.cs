using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[CustomEditor(typeof(HouseBuilder))]
public class HouseBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HouseBuilder targetScript = (HouseBuilder)target;

        if (GUILayout.Button("Generate"))
            targetScript.Generate();

        if (GUILayout.Button("Reset"))
            targetScript.ResetLists();
    }
}
