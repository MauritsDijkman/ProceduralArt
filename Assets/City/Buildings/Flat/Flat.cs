using System.Collections.Generic;
using UnityEngine;

public class Flat : MonoBehaviour
{
    [Header("MeshCreator")]
    [SerializeField] private GameObject meshCreator = null;

    [Header("AircoPlacer")]
    [SerializeField] private GameObject aircoPlacer = null;

    [Header("Size")]
    public int width = 0;
    public int depth = 0;

    [Header("Info")]
    [SerializeField] private BuildingInfoFlat[] buildingInfo = null;
    private BuildingInfoFlat chosenBuildingInfo = null;

    // Height
    private int chosenHeight = 0;

    // Position
    private Vector2 position = Vector2.zero;

    // Random texture number
    private int randomNumber;

    // Lists
    private List<WallCoordinates> wallCoordinates = new List<WallCoordinates>();
    private List<WallCoordinates> roofCoordinates = new List<WallCoordinates>();


    public void Initialize(Vector2 pPosition, int pWidth, int pDepth)
    {
        position = pPosition;
        width = pWidth;
        depth = pDepth;

    }

    public void Generate()
    {
        Execute();
    }

    private void Execute()
    {
        RemoveExistingBuilding();

        if (position == Vector2.zero)
            position = new Vector2(transform.position.x, transform.position.z);

        Debug.Log($"Flat position: {position}");

        System.Random random = FindObjectOfType<RandomGenerator>().GetRandom();

        int randomNumber = random.Next(0, buildingInfo.Length);
        chosenBuildingInfo = buildingInfo[randomNumber];


        if (!chosenBuildingInfo.randomHeight)
            chosenHeight = chosenBuildingInfo.staticHeight;
        else
        {
            chosenHeight = Random.Range(chosenBuildingInfo.minHeight, chosenBuildingInfo.maxHeight + 1);
            chosenBuildingInfo.staticHeight = chosenHeight;
        }

        // Debug.Log($"Position: {position}");

        randomNumber = Random.Range(0, 2);

        for (int i = 0; i < chosenHeight; i++)
        {
            GenerateWalls(1);
            PlaceNormalWalls(i);
        }

        GenerateRoof();

        if (chosenBuildingInfo.buildRoofWall)
            BuildRoofWall(chosenHeight);
    }

    private void BuildRoofWall(int pHeight)
    {
        GenerateWalls(chosenBuildingInfo.roofWallHeight, true);

        // Instantiate
        GameObject pWallCreator = Instantiate(meshCreator, new Vector3(position.x, pHeight, position.y), Quaternion.identity);
        pWallCreator.transform.parent = transform;

        pWallCreator.GetComponent<Renderer>().material = chosenBuildingInfo.roofWallMaterial;
        pWallCreator.GetComponent<Mesh_Creator>().Generate(wallCoordinates);
        wallCoordinates.Clear();
    }

    public void RemoveExistingBuilding()
    {
        int childcount = transform.childCount;

        for (int i = childcount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (Application.isPlaying)
                Destroy(child);
            else if (Application.isEditor)
                DestroyImmediate(child);
        }

        position = Vector2.zero;
    }

    private void GenerateWalls(float pY_Value = 1, bool pDoubleSided = false)
    {
        float yValue = pY_Value;

        // Front wall
        WallCoordinates frontWall = new WallCoordinates();
        Vector3 frontBottomLeft = new Vector3(position.x, 0, position.y);
        Vector3 frontTopRight = new Vector3(position.x + width, yValue, position.y);
        frontWall.bottomLeft = transform.InverseTransformPoint(frontBottomLeft);
        frontWall.topRight = transform.InverseTransformPoint(frontTopRight);
        wallCoordinates.Add(frontWall);

        // Back wall
        WallCoordinates backWall = new WallCoordinates();
        Vector3 backBottomLeft = new Vector3(position.x + width, 0, position.y + depth);
        Vector3 backTopRight = new Vector3(position.x, yValue, position.y + depth);
        backWall.bottomLeft = transform.InverseTransformPoint(backBottomLeft);
        backWall.topRight = transform.InverseTransformPoint(backTopRight);
        wallCoordinates.Add(backWall);

        // Right wall
        WallCoordinates rightWall = new WallCoordinates();
        Vector3 rightBottomLeft = new Vector3(position.x + width, 0, position.y);
        Vector3 rightTopRight = new Vector3(position.x + width, yValue, position.y + depth);
        rightWall.bottomLeft = transform.InverseTransformPoint(rightBottomLeft);
        rightWall.topRight = transform.InverseTransformPoint(rightTopRight);
        wallCoordinates.Add(rightWall);

        // Left wall
        WallCoordinates leftWall = new WallCoordinates();
        Vector3 leftBottomLeft = new Vector3(position.x, 0, position.y + depth);
        Vector3 leftTopRight = new Vector3(position.x, yValue, position.y);
        leftWall.bottomLeft = transform.InverseTransformPoint(leftBottomLeft);
        leftWall.topRight = transform.InverseTransformPoint(leftTopRight);
        wallCoordinates.Add(leftWall);

        if (pDoubleSided)
        {
            WallCoordinates frontWallDouble = new WallCoordinates();
            frontWallDouble.bottomLeft = transform.InverseTransformPoint(new Vector3(frontTopRight.x, frontBottomLeft.y, frontTopRight.z));
            frontWallDouble.topRight = transform.InverseTransformPoint(new Vector3(frontBottomLeft.x, frontTopRight.y, frontBottomLeft.z));
            wallCoordinates.Add(frontWallDouble);

            WallCoordinates backWallDouble = new WallCoordinates();
            backWallDouble.bottomLeft = transform.InverseTransformPoint(new Vector3(backTopRight.x, backBottomLeft.y, backTopRight.z));
            backWallDouble.topRight = transform.InverseTransformPoint(new Vector3(backBottomLeft.x, backTopRight.y, backBottomLeft.z));
            wallCoordinates.Add(backWallDouble);

            WallCoordinates rightWallDouble = new WallCoordinates();
            rightWallDouble.bottomLeft = transform.InverseTransformPoint(new Vector3(rightTopRight.x, rightBottomLeft.y, rightTopRight.z));
            rightWallDouble.topRight = transform.InverseTransformPoint(new Vector3(rightBottomLeft.x, rightTopRight.y, rightBottomLeft.z));
            wallCoordinates.Add(rightWallDouble);

            WallCoordinates leftWallDouble = new WallCoordinates();
            leftWallDouble.bottomLeft = transform.InverseTransformPoint(new Vector3(leftTopRight.x, leftBottomLeft.y, leftTopRight.z));
            leftWallDouble.topRight = transform.InverseTransformPoint(new Vector3(leftBottomLeft.x, leftTopRight.y, leftBottomLeft.z));
            wallCoordinates.Add(leftWallDouble);

            Debug.Log($"Double sided wall");
        }
    }

    private void PlaceNormalWalls(int pRowNumber)
    {
        // Instantiate
        GameObject pWallCreator = Instantiate(meshCreator, new Vector3(position.x, pRowNumber, position.y), Quaternion.identity);
        pWallCreator.transform.parent = transform;


        pWallCreator.GetComponent<Renderer>().material = chosenBuildingInfo.brickMaterial;

        if (pRowNumber == 0)
            pWallCreator.GetComponent<Renderer>().material = chosenBuildingInfo.firstRowMaterial;
        else if (pRowNumber == chosenHeight - 1 && chosenBuildingInfo.lastRowDifferent)
            pWallCreator.GetComponent<Renderer>().material = chosenBuildingInfo.lastRowMaterial;

        pWallCreator.GetComponent<Mesh_Creator>().Generate(wallCoordinates);
        wallCoordinates.Clear();
    }

    private void GenerateRoof()
    {
        // Roof
        WallCoordinates roof = new WallCoordinates();
        roof.bottomLeft = new Vector3(0, 0, 0);
        roof.topRight = new Vector3(width, depth, 0);
        roofCoordinates.Add(roof);

        // Instantiate
        GameObject pWallCreator = Instantiate(meshCreator, new Vector3(position.x, chosenHeight, position.y), Quaternion.identity);
        pWallCreator.transform.parent = transform;
        pWallCreator.transform.eulerAngles = new Vector3(90, 0, 0);

        pWallCreator.GetComponent<Renderer>().material = chosenBuildingInfo.roofMaterial;

        pWallCreator.GetComponent<Mesh_Creator>().Generate(roofCoordinates);
        roofCoordinates.Clear();

        if (chosenBuildingInfo.buildAirco)
        {
            Vector3 bottomLeft = new Vector3(0, 0, 0);
            Vector3 topRight = new Vector3(width, depth, 0);

            GameObject pAircoPlacer = Instantiate(aircoPlacer, new Vector3(position.x, chosenHeight, position.y), Quaternion.identity);
            pAircoPlacer.GetComponent<AircoPlacer>().Initialize(new Vector3(position.x, chosenHeight, position.y), (int)(topRight.x - bottomLeft.x), (int)(topRight.y - bottomLeft.y));
            pAircoPlacer.GetComponent<AircoPlacer>().Generate();
            pAircoPlacer.transform.parent = transform;
        }
    }
}
