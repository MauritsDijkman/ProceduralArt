using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Curve : MonoBehaviour
{
    [Header("Curve Points")]
    public List<Vector3> points;

    public void Apply()
    {
        WallMeshCreator creator = GetComponent<WallMeshCreator>();

        if (creator != null)
            creator.RecalculateMesh();
    }
}
