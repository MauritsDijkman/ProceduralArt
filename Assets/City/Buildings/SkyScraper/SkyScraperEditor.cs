using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkyScraper))]
public class SkyScraperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SkyScraper targetScript = (SkyScraper)target;

        if (GUILayout.Button("Generate"))
            targetScript.Generate();

        if (GUILayout.Button("Reset"))
            targetScript.RemoveExistingBuilding();
    }
}
