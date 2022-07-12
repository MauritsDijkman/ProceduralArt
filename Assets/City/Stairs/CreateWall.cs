using UnityEngine;

public class CreateWall : MonoBehaviour
{
    [Header("Sizes")]
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;

    // MeshBuilder
    private MeshBuilder builder;

    private void Start()
    {
        builder = new MeshBuilder();
        CreateShape();
        GetComponent<MeshFilter>().mesh = builder.CreateMesh(true);
    }

    private void CreateShape()
    {
        builder.Clear();

        float leftPos = (float)-width / 2;
        float rightPos = (float)width / 2;

        Vector2 uvBR = new Vector2(1f, 0f);
        Vector2 uvBL = new Vector2(0f, 0f);
        Vector2 uvTR = new Vector2(1f, 0.5f);
        Vector2 uvTL = new Vector2(0f, 0.5f);

        // Front
        int v1 = builder.AddVertex(new Vector3(rightPos, 0, 0), uvBR);            // Bottom right
        int v2 = builder.AddVertex(new Vector3(leftPos, 0, 0), uvBL);             // Bottom left
        int v3 = builder.AddVertex(new Vector3(rightPos, height, 0), uvTR);       // Top right

        int v4 = builder.AddVertex(new Vector3(leftPos, height, 0), uvTL);        // Top left
        int v5 = builder.AddVertex(new Vector3(rightPos, height, 0), uvTR);       // Top right
        int v6 = builder.AddVertex(new Vector3(leftPos, 0, 0), uvBL);             // Bottom left

        // Front
        builder.AddTriangle(v1, v2, v3);
        builder.AddTriangle(v4, v5, v6);
    }
}
