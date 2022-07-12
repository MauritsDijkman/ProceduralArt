using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[CustomEditor(typeof(SquareFoundation))]
public class SquareFoundationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SquareFoundation targetScript = (SquareFoundation)target;

        if (GUILayout.Button("(Re)Generate trees"))
            targetScript.Generate();
    }
}
