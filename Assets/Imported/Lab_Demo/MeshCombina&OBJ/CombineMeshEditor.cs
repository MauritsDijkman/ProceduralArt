using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CombineMeshes))]
public class CombineMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Combine Mesh"))
        {
            CombineMeshes saver = (CombineMeshes)target;
            saver.CombineMesh();
        }

        DrawDefaultInspector();
    }
}
