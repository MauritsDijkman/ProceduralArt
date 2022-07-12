using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[CustomEditor(typeof(RandomBuildingGenerator))]
public class RandomBuildingGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RandomBuildingGenerator myScript = (RandomBuildingGenerator)target;

        if (GUILayout.Button("(Re)Calculate Weights"))
            myScript.CalculateWeights();
    }
}
