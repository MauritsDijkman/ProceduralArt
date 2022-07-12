using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabBuilding : MonoBehaviour
{
    [Header("MeshCreator")]
    [SerializeField] private GameObject meshCreator = null;

    [Header("AircoPlacer")]
    [SerializeField] private GameObject aircoPlacer = null;

    [Header("Size")]
    public int width = 0;
    public int depth = 0;

    [Header("Info")]
    [SerializeField] private BuildingInfoGab[] buildingInfo = null;
    private BuildingInfoGab chosenBuildingInfo = null;

    // Chosen parameters
    private int chosenHeight = 0;
    private int chosenRows = 0;

    // Shape parameters
    private Vector2 position = new Vector2();
    private float rowDepth = 0;
    private float depthLength = 0;

    private List<WallCoordinates> wallCoordinates = new List<WallCoordinates>();
    private List<WallCoordinates> roofCoordinates = new List<WallCoordinates>();

    private bool roofPlaced = false;

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

    public void RemoveExistingBuilding()
    {
        roofPlaced = false;

        int childcount = transform.childCount;

        for (int i = childcount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (Application.isPlaying)
                Destroy(child);
            else if (Application.isEditor)
                DestroyImmediate(child);
        }

        wallCoordinates.Clear();
        roofCoordinates.Clear();

        chosenBuildingInfo = null;

        rowDepth = 0;
        depthLength = 0;

        chosenHeight = 0;
        chosenRows = 0;

        foreach (Mesh_Creator pObject in transform.GetComponentsInChildren<Mesh_Creator>())
        {
            pObject.wallcoordinates.Clear();
            pObject.builder = null;
        }

        if (transform.localScale.x < 0)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        transform.position = new Vector3(position.x, 0, position.y);
    }

    private void Execute()
    {
        if (transform.localScale.x < 0)
            position = new Vector2(transform.position.x - width, transform.position.z);
        else
            position = new Vector2(transform.position.x, transform.position.z);


        RemoveExistingBuilding();

        System.Random random = FindObjectOfType<RandomGenerator>().GetRandom();
        int randomNumber = random.Next(0, buildingInfo.Length);
        chosenBuildingInfo = buildingInfo[randomNumber];


        // Height
        if (!chosenBuildingInfo.randomHeight)
            chosenHeight = chosenBuildingInfo.staticHeight;
        else
        {
            chosenHeight = Random.Range(chosenBuildingInfo.minHeight, chosenBuildingInfo.maxHeight + 1);
            chosenBuildingInfo.staticHeight = chosenHeight;
        }

        // Rows amount
        if (!chosenBuildingInfo.randomRows)
            chosenRows = chosenBuildingInfo.staticRows;
        else
        {
            chosenRows = Random.Range(chosenBuildingInfo.minRows, chosenBuildingInfo.maxRows + 1);
            chosenBuildingInfo.staticRows = chosenRows;
        }

        // Rown depth
        if (chosenBuildingInfo.randomDepth)
            chosenBuildingInfo.rowDepthPercentage = Random.Range(30, 70);

        // Direction
        if (chosenBuildingInfo.randomDirection)
        {
            int pRandomNumber = Random.Range(0, 2);

            if (pRandomNumber == 0)
            {
                chosenBuildingInfo.faceLeft = true;
                chosenBuildingInfo.faceRight = false;
            }
            else if (pRandomNumber == 1)
            {
                chosenBuildingInfo.faceLeft = false;
                chosenBuildingInfo.faceRight = true;
            }
        }

        rowDepth = (chosenBuildingInfo.rowDepthPercentage / 100f) * width;
        depthLength = (float)depth / (float)chosenRows;

        if (chosenBuildingInfo.rowDepthPercentage <= 0)
            depthLength = 1;

        Debug.Log($"Width: {width} || Depth: {depth} || RowDepth: {rowDepth} || DepthLength: {depthLength}|| RowDepthPercentage: {chosenBuildingInfo.rowDepthPercentage} || Facing left: {chosenBuildingInfo.faceLeft} || Facing right: {chosenBuildingInfo.faceRight}");

        if (chosenBuildingInfo.squaredFloorLevel < 0)
            chosenBuildingInfo.keepGroundFloorSquared = false;

        // Place walls
        for (int i = 0; i < chosenHeight; i++)
        {
            if (!chosenBuildingInfo.keepGroundFloorSquared)
                BuildWalls(i);
            else
            {
                if (i <= chosenBuildingInfo.squaredFloorLevel)
                    BuildSquareRow(i);
                else
                {
                    if (!roofPlaced)
                    {
                        PlaceSquaredRoofs();
                        roofPlaced = true;
                    }
                    BuildWalls(i);
                }
            }
        }

        // Place roofs
        BuildRoofs();

        if (chosenBuildingInfo.buildRoofWall)
            BuildRoofWall(chosenHeight);

        // Check wich side
        if (chosenBuildingInfo.faceLeft)
        {
            //transform.eulerAngles = new Vector3(0, 0, 0);
            //transform.position = new Vector3(position.x, 0, position.y);

            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            transform.position = new Vector3(position.x, 0, position.y);
        }
        else if (chosenBuildingInfo.faceRight)
        {
            //transform.eulerAngles = new Vector3(0, 180, 0);
            //transform.position = new Vector3(position.x + width, 0, position.y + depth);
            if (transform.localScale.x > 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            transform.position = new Vector3(position.x + width, 0, position.y);
        }
    }

    //-------------------------------------------------------// Roof Walls //-------------------------------------------------------//

    private void BuildRoofWall(int pHeight)
    {
        for (int i = 1; i <= chosenRows; i++)
        {
            PlaceLeftWallsVertical(i, chosenBuildingInfo.roofWallHeight, true);

            if (i != chosenRows)
                PLaceLeftWallsHorizontal(i, chosenBuildingInfo.roofWallHeight, true);
        }

        PlaceRightWall(chosenBuildingInfo.roofWallHeight, true);
        PlaceFrontWall(chosenBuildingInfo.roofWallHeight, true);
        PlaceBackWall(chosenBuildingInfo.roofWallHeight, true);

        // Instantiate
        GameObject pWallCreator = Instantiate(meshCreator, new Vector3(position.x, pHeight, position.y), Quaternion.identity);
        pWallCreator.transform.parent = transform;

        pWallCreator.GetComponent<Renderer>().material = chosenBuildingInfo.roofWallMaterial;
        pWallCreator.GetComponent<Mesh_Creator>().Generate(wallCoordinates);
        wallCoordinates.Clear();
    }


    //-------------------------------------------------------// SquaredLevel //-------------------------------------------------------//

    private void BuildSquareRow(int pHeight)
    {
        PlaceLeftWallsVertical(1, 1, false, true);
        PlaceRightWall();
        PlaceFrontWall(1, false, true);
        PlaceBackWall();

        // Instantiate
        GameObject pWallCreator = Instantiate(meshCreator, new Vector3(position.x, pHeight, position.y), Quaternion.identity);
        pWallCreator.transform.parent = transform;
        pWallCreator.GetComponent<Renderer>().material = chosenBuildingInfo.firstRowMaterial;
        pWallCreator.GetComponent<Mesh_Creator>().Generate(wallCoordinates, true);
        wallCoordinates.Clear();
    }

    private void PlaceSquaredRoofs()
    {
        WallCoordinates roof = new WallCoordinates();

        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        bottomLeft = new Vector3(0, 0, 0);
        topRight = new Vector3(width, depth, 0);

        roof.bottomLeft = bottomLeft;
        roof.topRight = topRight;

        roofCoordinates.Add(roof);

        // Instantiate
        GameObject pWallCreatorRoof = Instantiate(meshCreator, new Vector3(position.x, chosenBuildingInfo.squaredFloorLevel + 1, position.y), Quaternion.identity);
        pWallCreatorRoof.transform.parent = transform;
        pWallCreatorRoof.transform.eulerAngles = new Vector3(90, 0, 0);
        pWallCreatorRoof.GetComponent<Renderer>().material = chosenBuildingInfo.roofMaterial;
        pWallCreatorRoof.GetComponent<Mesh_Creator>().Generate(roofCoordinates);
        roofCoordinates.Clear();
    }


    //-------------------------------------------------------// Walls //-------------------------------------------------------//

    private void BuildWalls(int pHeight)
    {
        if (chosenBuildingInfo.rowDepthPercentage > 0)
        {
            for (int i = 1; i <= chosenRows; i++)
            {
                PlaceLeftWallsVertical(i);

                if (i != chosenRows)
                    PLaceLeftWallsHorizontal(i);
            }
        }
        else
            PlaceLeftWallsVertical(1);

        PlaceRightWall();
        PlaceFrontWall();
        PlaceBackWall();

        // Instantiate
        GameObject pWallCreator = Instantiate(meshCreator, new Vector3(position.x, pHeight, position.y), Quaternion.identity);
        pWallCreator.transform.parent = transform;

        pWallCreator.GetComponent<Renderer>().material = chosenBuildingInfo.brickMaterial;

        if (pHeight == 0)
            pWallCreator.GetComponent<Renderer>().material = chosenBuildingInfo.firstRowMaterial;
        else if (pHeight == chosenHeight - 1)
            pWallCreator.GetComponent<Renderer>().material = chosenBuildingInfo.lastRowMaterial; ;

        pWallCreator.GetComponent<Mesh_Creator>().Generate(wallCoordinates, true);
        wallCoordinates.Clear();
    }

    private void PlaceLeftWallsVertical(int pRowNumber, float pY_Value = 1, bool pDoubleSided = false, bool keepGroundFloor = false)
    {
        float yValue = pY_Value;

        bool isEven;
        if (pRowNumber % 2 == 0)
            isEven = true;
        else
            isEven = false;

        WallCoordinates wall = new WallCoordinates();
        WallCoordinates wallDouble = new WallCoordinates();

        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        Vector3 bottomLeftDouble = Vector3.zero;
        Vector3 topRightDouble = Vector3.zero;

        if (keepGroundFloor)
        {
            bottomLeft = new Vector3(position.x, 0, position.y + depth);
            topRight = new Vector3(position.x, yValue, position.y);
        }
        else
        {
            if (isEven)
            {
                bottomLeft = new Vector3(position.x + rowDepth, 0, position.y + depth - (depthLength * (pRowNumber - 1)));
                topRight = new Vector3(position.x + rowDepth, yValue, position.y + depth - (depthLength * pRowNumber));

                if (pDoubleSided)
                {
                    bottomLeftDouble = new Vector3(topRight.x, bottomLeft.y, topRight.z);
                    topRightDouble = new Vector3(bottomLeft.x, topRight.y, bottomLeft.z);
                }
            }
            else
            {
                bottomLeft = new Vector3(position.x, 0, position.y + depth - (depthLength * (pRowNumber - 1)));
                topRight = new Vector3(position.x, yValue, position.y + depth - (depthLength * pRowNumber));

                if (chosenBuildingInfo.rowDepthPercentage <= 0)
                {
                    bottomLeft = new Vector3(position.x, 0, position.y + depth);
                    topRight = new Vector3(position.x, yValue, position.y);
                }

                if (pDoubleSided)
                {
                    bottomLeftDouble = new Vector3(topRight.x, bottomLeft.y, topRight.z);
                    topRightDouble = new Vector3(bottomLeft.x, topRight.y, bottomLeft.z);
                }
            }
        }

        wall.bottomLeft = transform.InverseTransformPoint(bottomLeft);
        wall.topRight = transform.InverseTransformPoint(topRight);
        if (pDoubleSided)
        {
            wallDouble.bottomLeft = transform.InverseTransformPoint(bottomLeftDouble);
            wallDouble.topRight = transform.InverseTransformPoint(topRightDouble);
        }


        wallCoordinates.Add(wall);
        if (pDoubleSided)
            wallCoordinates.Add(wallDouble);
    }

    private void PLaceLeftWallsHorizontal(int pRowNumber, float pY_Value = 1, bool pDoubleSided = false)
    {
        if (chosenBuildingInfo.rowDepthPercentage <= 0)
            return;

        float yValue = pY_Value;

        bool isEven;
        if (pRowNumber % 2 == 0)
            isEven = true;
        else
            isEven = false;

        WallCoordinates wall = new WallCoordinates();
        WallCoordinates wallDouble = new WallCoordinates();

        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        Vector3 bottomLeftDouble = Vector3.zero;
        Vector3 topRightDouble = Vector3.zero;

        if (isEven)
        {
            bottomLeft = new Vector3(position.x + rowDepth, 0, position.y + depth - (depthLength * pRowNumber));
            topRight = new Vector3(position.x, yValue, position.y + depth - (depthLength * pRowNumber));

            if (pDoubleSided)
            {
                bottomLeftDouble = new Vector3(topRight.x, bottomLeft.y, topRight.z);
                topRightDouble = new Vector3(bottomLeft.x, topRight.y, bottomLeft.z);
            }
        }
        else
        {
            bottomLeft = new Vector3(position.x, 0, position.y + depth - (depthLength * pRowNumber));
            topRight = new Vector3(position.x + rowDepth, yValue, position.y + depth - (depthLength * pRowNumber));

            if (pDoubleSided)
            {
                bottomLeftDouble = new Vector3(topRight.x, bottomLeft.y, topRight.z);
                topRightDouble = new Vector3(bottomLeft.x, topRight.y, bottomLeft.z);
            }
        }

        wall.bottomLeft = transform.InverseTransformPoint(bottomLeft);
        wall.topRight = transform.InverseTransformPoint(topRight);
        if (pDoubleSided)
        {
            wallDouble.bottomLeft = transform.InverseTransformPoint(bottomLeftDouble);
            wallDouble.topRight = transform.InverseTransformPoint(topRightDouble);
        }

        wallCoordinates.Add(wall);
        if (pDoubleSided)
            wallCoordinates.Add(wallDouble);
    }


    private void PlaceRightWall(float pY_Value = 1, bool pDoubleSided = false)
    {
        float yValue = pY_Value;

        WallCoordinates wall = new WallCoordinates();
        WallCoordinates wallDouble = new WallCoordinates();

        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        Vector3 bottomLeftDouble = Vector3.zero;
        Vector3 topRightDouble = Vector3.zero;

        bottomLeft = new Vector3(position.x + width, 0, position.y);
        topRight = new Vector3(position.x + width, yValue, position.y + depth);

        if (pDoubleSided)
        {
            bottomLeftDouble = new Vector3(topRight.x, bottomLeft.y, topRight.z);
            topRightDouble = new Vector3(bottomLeft.x, topRight.y, bottomLeft.z);
        }

        wall.bottomLeft = transform.InverseTransformPoint(bottomLeft);
        wall.topRight = transform.InverseTransformPoint(topRight);
        if (pDoubleSided)
        {
            wallDouble.bottomLeft = transform.InverseTransformPoint(bottomLeftDouble);
            wallDouble.topRight = transform.InverseTransformPoint(topRightDouble);
        }

        wallCoordinates.Add(wall);
        if (pDoubleSided)
            wallCoordinates.Add(wallDouble);
    }

    private void PlaceFrontWall(float pY_Value = 1, bool pDoubleSided = false, bool firstRow = false)
    {
        float yValue = pY_Value;

        bool isEven;
        if (chosenRows % 2 == 0)
            isEven = true;
        else
            isEven = false;

        WallCoordinates wall = new WallCoordinates();
        WallCoordinates wallDouble = new WallCoordinates();

        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        Vector3 bottomLeftDouble = Vector3.zero;
        Vector3 topRightDouble = Vector3.zero;

        if (isEven)
        {
            if (firstRow)
                bottomLeft = new Vector3(position.x, 0, position.y);
            else
                bottomLeft = new Vector3(position.x + rowDepth, 0, position.y);

            topRight = new Vector3(position.x + width, yValue, position.y);

            if (pDoubleSided)
            {
                bottomLeftDouble = new Vector3(topRight.x, bottomLeft.y, topRight.z);
                topRightDouble = new Vector3(bottomLeft.x, topRight.y, bottomLeft.z);
            }
        }
        else
        {
            bottomLeft = new Vector3(position.x, 0, position.y);
            topRight = new Vector3(position.x + width, yValue, position.y);

            if (pDoubleSided)
            {
                bottomLeftDouble = new Vector3(topRight.x, bottomLeft.y, topRight.z);
                topRightDouble = new Vector3(bottomLeft.x, topRight.y, bottomLeft.z);
            }
        }

        wall.bottomLeft = transform.InverseTransformPoint(bottomLeft);
        wall.topRight = transform.InverseTransformPoint(topRight);
        if (pDoubleSided)
        {
            wallDouble.bottomLeft = transform.InverseTransformPoint(bottomLeftDouble);
            wallDouble.topRight = transform.InverseTransformPoint(topRightDouble);
        }

        wallCoordinates.Add(wall);
        if (pDoubleSided)
            wallCoordinates.Add(wallDouble);
    }

    private void PlaceBackWall(float pY_Value = 1, bool pDoubleSided = false)
    {
        float yValue = pY_Value;

        WallCoordinates wall = new WallCoordinates();
        WallCoordinates wallDouble = new WallCoordinates();

        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        bottomLeft = new Vector3(position.x + width, 0, position.y + depth);
        topRight = new Vector3(position.x, yValue, position.y + depth);

        Vector3 bottomLeftDouble = Vector3.zero;
        Vector3 topRightDouble = Vector3.zero;

        if (pDoubleSided)
        {
            bottomLeftDouble = new Vector3(topRight.x, bottomLeft.y, topRight.z);
            topRightDouble = new Vector3(bottomLeft.x, topRight.y, bottomLeft.z);
        }

        wall.bottomLeft = transform.InverseTransformPoint(bottomLeft);
        wall.topRight = transform.InverseTransformPoint(topRight);
        if (pDoubleSided)
        {
            wallDouble.bottomLeft = transform.InverseTransformPoint(bottomLeftDouble);
            wallDouble.topRight = transform.InverseTransformPoint(topRightDouble);
        }

        wallCoordinates.Add(wall);
        if (pDoubleSided)
            wallCoordinates.Add(wallDouble);
    }


    //-------------------------------------------------------// Roofs //-------------------------------------------------------//

    private void BuildRoofs()
    {
        PlaceBigRoof();

        for (int i = 1; i <= chosenRows; i++)
        {
            bool isEven;

            if (i % 2 == 0)
                isEven = true;
            else
                isEven = false;

            if (!isEven)
                PlaceSmallerRoof(i);
        }
    }

    private void PlaceBigRoof()
    {
        WallCoordinates roof = new WallCoordinates();

        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        bottomLeft = new Vector3(0, 0, 0);
        topRight = new Vector3(width - rowDepth, depth, 0);

        roof.bottomLeft = bottomLeft;
        roof.topRight = topRight;

        roofCoordinates.Add(roof);

        // Instantiate
        GameObject pWallCreatorRoof = Instantiate(meshCreator, new Vector3(position.x + rowDepth, chosenHeight, position.y), Quaternion.identity);
        pWallCreatorRoof.transform.parent = transform;
        pWallCreatorRoof.transform.eulerAngles = new Vector3(90, 0, 0);

        pWallCreatorRoof.GetComponent<Renderer>().material = chosenBuildingInfo.roofMaterial;

        pWallCreatorRoof.GetComponent<Mesh_Creator>().Generate(roofCoordinates);
        roofCoordinates.Clear();

        if (chosenBuildingInfo.buildAirco && width - rowDepth >= 2)
        {
            GameObject pAircoPlacer = Instantiate(aircoPlacer, new Vector3(position.x + rowDepth, chosenHeight, position.y), Quaternion.identity);
            pAircoPlacer.GetComponent<AircoPlacer>().Initialize(new Vector3(position.x + rowDepth, chosenHeight, position.y), (int)(topRight.x - bottomLeft.x), (int)(topRight.y - bottomLeft.y));
            pAircoPlacer.GetComponent<AircoPlacer>().Generate();
            pAircoPlacer.transform.parent = transform;
        }
    }

    private void PlaceSmallerRoof(int pRowNumber)
    {
        WallCoordinates roof = new WallCoordinates();

        Vector3 roofBottomLeft = Vector3.zero;
        Vector3 roofTopRight = Vector3.zero;

        roofBottomLeft = new Vector3(0, 0, 0);
        roofTopRight = new Vector3(rowDepth, depthLength, 0);

        roof.bottomLeft = roofBottomLeft;
        roof.topRight = roofTopRight;

        roofCoordinates.Add(roof);

        // Instantiate
        GameObject pWallCreatorRoof = Instantiate(meshCreator, new Vector3(position.x, chosenHeight, position.y + depth - (depthLength * (pRowNumber))), Quaternion.identity);
        pWallCreatorRoof.transform.parent = transform;
        pWallCreatorRoof.transform.eulerAngles = new Vector3(90, 0, 0);

        pWallCreatorRoof.GetComponent<Renderer>().material = chosenBuildingInfo.roofMaterial;

        pWallCreatorRoof.GetComponent<Mesh_Creator>().Generate(roofCoordinates);
        roofCoordinates.Clear();
    }
}
