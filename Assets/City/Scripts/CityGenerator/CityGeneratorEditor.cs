using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(CityGenerator))]
public class CityGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CityGenerator targetScript = (CityGenerator)target;

        if (GUILayout.Button("(Re)Generate"))
            targetScript.Generate();

        if (GUILayout.Button("Reset"))
            targetScript.ResetLists();
    }
}
