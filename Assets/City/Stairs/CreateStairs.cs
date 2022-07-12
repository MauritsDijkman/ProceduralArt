using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CreateStairs : MonoBehaviour
{
    public int numberOfSteps = 10;

    // The dimensions of a single step of the staircase:
    public float width = 3;
    public float height = 1;
    public float depth = 1;

    MeshBuilder builder;

    void Start()
    {
        builder = new MeshBuilder();
        CreateShape();
        GetComponent<MeshFilter>().mesh = builder.CreateMesh(true);
    }

    /// <summary>
    /// Creates a stairway shape in [builder].
    /// </summary>
    void CreateShape()
    {
        builder.Clear();

        /**/
        // V3, with for loop, using parameters:
        for (int i = 0; i < numberOfSteps; i++)
        {
            Vector3 offset = new Vector3(0, height * i, depth * i);

            float leftPos = -width / 2;
            float rightPos = width / 2;

            Vector2 uvBR = new Vector2(1f, 0f);
            Vector2 uvBL = new Vector2(0f, 0f);
            Vector2 uvTR = new Vector2(1f, 0.5f);
            Vector2 uvTL = new Vector2(0f, 0.5f);

            // Front
            int v1 = builder.AddVertex(offset + new Vector3(rightPos, 0, 0), uvBR);            // Bottom right
            int v2 = builder.AddVertex(offset + new Vector3(leftPos, 0, 0), uvBL);             // Bottom left
            int v3 = builder.AddVertex(offset + new Vector3(rightPos, height, 0), uvTR);       // Top right

            int v4 = builder.AddVertex(offset + new Vector3(leftPos, height, 0), uvTL);        // Top left
            int v5 = builder.AddVertex(offset + new Vector3(rightPos, height, 0), uvTR);       // Top right
            int v6 = builder.AddVertex(offset + new Vector3(leftPos, 0, 0), uvBL);             // Bottom left

            // Top
            int v7 = builder.AddVertex(offset + new Vector3(rightPos, height, 0), uvBR);       // Bottom right
            int v8 = builder.AddVertex(offset + new Vector3(leftPos, height, 0), uvBL);        // Bottom left
            int v9 = builder.AddVertex(offset + new Vector3(rightPos, height, depth), uvTR);   // Top right

            int v10 = builder.AddVertex(offset + new Vector3(leftPos, height, depth), uvTL);   // Top left
            int v11 = builder.AddVertex(offset + new Vector3(rightPos, height, depth), uvTR);  // Top right
            int v12 = builder.AddVertex(offset + new Vector3(leftPos, height, 0), uvBL);       // Bottom left

            // Right side
            int v13 = builder.AddVertex(offset + new Vector3(rightPos, height, 0), uvTL);      // Top left
            int v14 = builder.AddVertex(offset + new Vector3(rightPos, height, depth), uvTR);  // Top right
            int v15 = builder.AddVertex(offset + new Vector3(rightPos, 0, 0), uvBL);           // Bottom left

            // Left side
            int v16 = builder.AddVertex(offset + new Vector3(leftPos, height, 0), uvTR);       // Top right
            int v17 = builder.AddVertex(offset + new Vector3(leftPos, 0, 0), uvBR);            // Bottom right
            int v18 = builder.AddVertex(offset + new Vector3(leftPos, height, depth), uvTL);   // Top left

            // Back
            int v19 = builder.AddVertex(offset + new Vector3(leftPos, 0, 0), uvBL);            // Bottom left
            int v20 = builder.AddVertex(offset + new Vector3(rightPos, 0, 0), uvBR);           // Bottom right
            int v21 = builder.AddVertex(offset + new Vector3(leftPos, height, depth), uvTL);   // Top left

            int v22 = builder.AddVertex(offset + new Vector3(rightPos, height, depth), uvTR);  // Top right
            int v23 = builder.AddVertex(offset + new Vector3(leftPos, height, depth), uvTL);   // Top left
            int v24 = builder.AddVertex(offset + new Vector3(rightPos, 0, 0), uvBR);           // Bottom right


            // Front
            builder.AddTriangle(v1, v2, v3);
            builder.AddTriangle(v4, v5, v6);

            // Top
            builder.AddTriangle(v7, v8, v9);
            builder.AddTriangle(v10, v11, v12);

            // Right side
            builder.AddTriangle(v13, v14, v15);

            // Left side
            builder.AddTriangle(v16, v17, v18);

            // Back
            builder.AddTriangle(v19, v20, v21);
            builder.AddTriangle(v22, v23, v24);
        }
        /**/
    }
}
