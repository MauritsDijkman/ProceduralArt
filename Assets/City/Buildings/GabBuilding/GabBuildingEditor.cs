using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GabBuilding))]
public class GabBuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GabBuilding targetScript = (GabBuilding)target;

        //GUIContent arrayLabel = new GUIContent("Rows Facing Direction");
        //targetScript.facingSideNumber = EditorGUILayout.Popup(arrayLabel, targetScript.facingSideNumber, targetScript.facingSide);

        if (GUILayout.Button("Generate"))
            targetScript.Generate();

        if (GUILayout.Button("Reset"))
            targetScript.RemoveExistingBuilding();
    }
}
