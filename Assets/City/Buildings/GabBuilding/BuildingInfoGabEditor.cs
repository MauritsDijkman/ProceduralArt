using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingInfoGab))]
public class BuildingInfoGabEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //BuildingInfoGab targetScript = (BuildingInfoGab)target;

        //GUIContent arrayLabel = new GUIContent("Rows Facing Direction");
        //targetScript.facingSideNumber = EditorGUILayout.Popup(arrayLabel, targetScript.facingSideNumber, targetScript.facingSide);
    }
}
