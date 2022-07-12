using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Mesh_Creator : MonoBehaviour
{
    public List<WallCoordinates> wallcoordinates;
    public MeshBuilder builder;
    private bool forceToInt = false;

    public void Generate(List<WallCoordinates> pWallcoordinates, bool pForceToInt = false)
    {
        wallcoordinates = pWallcoordinates;
        forceToInt = pForceToInt;
        CreateWalls(pWallcoordinates);
    }

    private void CreateWalls(List<WallCoordinates> pWallcoordinates)
    {
        builder = new MeshBuilder();

        foreach (WallCoordinates wall in pWallcoordinates)
            CreateWallPiece(wall.bottomLeft, wall.topRight);

        GetComponent<MeshFilter>().mesh = builder.CreateMesh(true);
        this.gameObject.isStatic = true;
    }

    private void CreateWallPiece(Vector3 BL, Vector3 TR)
    {
        float width = 0;
        float height = 0;

        if (forceToInt)
        {
            float widthF = TR.x - BL.x;
            float heighF = TR.y - BL.y;

            if (TR.x == BL.x)
                widthF = TR.z - BL.z;

            int widthInt = (int)widthF;
            int heightInt = (int)heighF;

            width = widthInt;
            height = heightInt;

            if (width == 0)
                width = 1;
        }
        else
        {
            float widthFloat = TR.x - BL.x;
            float heightFloat = TR.y - BL.y;

            if (TR.x == BL.x)
                widthFloat = TR.z - BL.z;

            width = widthFloat;
            height = heightFloat;
        }



        //Debug.Log($"UV INFO || Width: {width} || Height: {height}");

        Vector2 uvBR = new Vector2(width, 0f);      // Bottom right
        Vector2 uvBL = new Vector2(0f, 0f);         // Bottom left
        Vector2 uvTR = new Vector2(width, height);  // Top right
        Vector2 uvTL = new Vector2(0f, height);     // Top left

        int v1 = builder.AddVertex(new Vector3(TR.x, BL.y, TR.z), uvBR);   // Bottom right
        int v2 = builder.AddVertex(new Vector3(BL.x, BL.y, BL.z), uvBL);   // Bottom left
        int v3 = builder.AddVertex(new Vector3(TR.x, TR.y, TR.z), uvTR);   // Top right

        int v4 = builder.AddVertex(new Vector3(BL.x, TR.y, BL.z), uvTL);   // Top left
        int v5 = builder.AddVertex(new Vector3(TR.x, TR.y, TR.z), uvTR);   // Top right
        int v6 = builder.AddVertex(new Vector3(BL.x, BL.y, BL.z), uvBL);   // Bottom left

        builder.AddTriangle(v1, v2, v3);
        builder.AddTriangle(v4, v5, v6);
    }
}
