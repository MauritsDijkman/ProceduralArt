using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Header("Generate")]
    [SerializeField] private bool GenerateOnPlay = false;

    [Header("Size")]
    [SerializeField] private Vector2 startPosition = new Vector2(0, 0);
    [SerializeField] private Vector2 gridSize = new Vector2(100, 100);
    [SerializeField] private int minimumRoomWidth = 10;
    [SerializeField] private int minimumRoomHeight = 10;

    [Header("Road Horizontal")]
    [SerializeField] private int roadWidthHorizontal = 1;

    [Header("Road Vertical")]
    [SerializeField] private int roadWidthVertical = 1;
    [Tooltip("When random is turned off, 'Road Width Vertical' will be used for the width of all vertical roads")]
    [SerializeField] private bool randomWidth = false;
    [SerializeField] private int minRoadWidth = 2;
    [SerializeField] private int maxRoadWidth = 4;

    [Header("Generation")]
    [SerializeField] private bool placeRoads = true;
    [SerializeField] private bool placeFoundations = false;

    [Header("Objects")]
    [SerializeField] private GameObject road = null;
    [SerializeField] private GameObject foundation = null;

    [Header("Houses")]
    [SerializeField] private int buildingOffset = 3;

    [Header("Visual")]
    [SerializeField] private bool drawRooms = false;

    // Lists
    private List<Room> rooms = new List<Room>();
    private List<GameObject> roads = new List<GameObject>();
    private List<GameObject> foundations = new List<GameObject>();


    public void ResetLists()
    {
        FindObjectOfType<RandomBuildingGenerator>().SetValueSetState(false);

        rooms.Clear();

        if (roads.Count > 0)
        {
            foreach (GameObject road in roads)
            {
                if (Application.isPlaying)
                    GameObject.Destroy(road.gameObject);
                else if (Application.isEditor)
                    GameObject.DestroyImmediate(road.gameObject);
            }

            roads.Clear();
        }

        if (foundations.Count > 0)
        {
            foreach (GameObject foundation in foundations)
            {
                if (Application.isPlaying)
                    GameObject.Destroy(foundation.gameObject);
                else if (Application.isEditor)
                    GameObject.DestroyImmediate(foundation.gameObject);
            }

            foundations.Clear();
        }

        int childcountRoad = GameObject.Find("City/Roads").transform.childCount;

        for (int i = childcountRoad - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (Application.isPlaying)
                Destroy(child);
            else if (Application.isEditor)
                DestroyImmediate(child);
        }

        int childcountFoundation = GameObject.Find("City/BuildingFoundations").transform.childCount;

        for (int i = childcountFoundation - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (Application.isPlaying)
                Destroy(child);
            else if (Application.isEditor)
                DestroyImmediate(child);
        }
    }

    private void Start()
    {
        if (GenerateOnPlay)
            Generate();
    }

    public void Generate()
    {
        FindObjectOfType<RandomGenerator>().SetNewRandomSeed();

        ResetLists();

        int pRoadWidthVertical = roadWidthVertical;
        if (randomWidth)
            pRoadWidthVertical = Random.Range(minRoadWidth, maxRoadWidth + 1);

        //GridAlgorithm gridAlgoritm = FindObjectOfType<GridAlgorithm>();
        GridAlgorithm gridAlgorithm = new GridAlgorithm();

        gridAlgorithm.Initialize(startPosition, gridSize, minimumRoomWidth, minimumRoomHeight, roadWidthHorizontal, pRoadWidthVertical, true, true, drawRooms);
        rooms = gridAlgorithm.GenerateRooms();

        foreach (Room room in rooms)
        {
            if (placeFoundations)
                PlaceFoundation(room.area);

            if (placeRoads)
                SpawnRoads(room);
        }
    }

    private void PlaceFoundation(Rectangle rectangle)
    {
        //Debug.Log("Place foundation!");

        foundation.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        GameObject copyFoundation = foundation;
        copyFoundation.transform.localScale = new Vector3(rectangle.Width, 1.0f, rectangle.Height);

        Vector3 spawnPosition = new Vector3(rectangle.X, 0f, rectangle.Y);
        GameObject spawnedFoundation = (GameObject)Instantiate(copyFoundation, spawnPosition, Quaternion.identity);

        if (spawnedFoundation.GetComponent<SquareFoundation>() != null)
        {
            spawnedFoundation.GetComponent<SquareFoundation>().Initialize(rectangle.Width, rectangle.Height, new Vector2(rectangle.X, rectangle.Y));
            spawnedFoundation.GetComponent<SquareFoundation>().Generate();
        }
        if (spawnedFoundation.GetComponent<HouseBuilder>() != null)
        {
            spawnedFoundation.GetComponent<HouseBuilder>().Initialize(new Vector2(rectangle.X + buildingOffset, rectangle.Y + buildingOffset), new Vector2(rectangle.Width - buildingOffset * 2, rectangle.Height - buildingOffset * 2));
            spawnedFoundation.GetComponent<HouseBuilder>().Generate();
        }

        if (GameObject.Find("City/BuildingFoundations").transform != null)
            spawnedFoundation.transform.parent = GameObject.Find("City/BuildingFoundations").transform;

        foundations.Add(spawnedFoundation);

        foundation.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void SpawnRoads(Room pRoom)
    {
        //SpawnNewVerticalRoads(pRoom);

        SpawnRoadVertical(pRoom);
        SpawnRoadHorizontal(pRoom);
    }

    private void SpawnRoadVertical(Room pRoomA)
    {
        Point roadPos = new Point(0, 0);
        roadPos = new Point(pRoomA.area.X + pRoomA.area.Width, pRoomA.area.Y);

        if (roadPos.Y >= gridSize.y || roadPos.X >= gridSize.x)
            return;

        int pRoadWidth = roadWidthHorizontal;
        int roadHeight = pRoomA.area.Height + pRoomA.roadWidth;

        if (roadPos.Y + roadHeight > gridSize.y)
        {
            int different = (int)((roadPos.Y + roadHeight) - gridSize.y);
            roadHeight -= different;
        }

        // Reset the localScale
        road.transform.localScale = new Vector3(1f, 0.1f, 1f);

        GameObject customRoad = road;
        customRoad.transform.localScale = new Vector3(pRoadWidth, 0.1f, roadHeight);
        GameObject spawnedRoad = (GameObject)Instantiate(customRoad, new Vector3(roadPos.X, 0, roadPos.Y), Quaternion.identity);

        if (GameObject.Find("City/Roads").transform != null)
            spawnedRoad.transform.parent = GameObject.Find("City/Roads").transform;

        roads.Add(spawnedRoad);

        // Reset the localScale
        road.transform.localScale = new Vector3(1f, 0.1f, 1f);
    }

    private void SpawnRoadHorizontal(Room pRoomA)
    {
        Point roadPos = new Point(0, 0);
        roadPos = new Point(pRoomA.area.X, pRoomA.area.Y + pRoomA.area.Height);

        if (roadPos.Y >= gridSize.y)
            return;

        int pRoadWidth = pRoomA.area.Width;
        int roadHeight = pRoomA.roadWidth;

        // Reset localScale
        road.transform.localScale = new Vector3(1f, 0.1f, 1f);

        GameObject customRoad = road;
        customRoad.transform.localScale = new Vector3(roadHeight, 0.1f, pRoadWidth);
        GameObject spawnedRoad = (GameObject)Instantiate(customRoad, new Vector3(roadPos.X, 0, roadPos.Y + roadHeight), Quaternion.Euler(0, 90, 0));

        if (GameObject.Find("City/Roads").transform != null)
            spawnedRoad.transform.parent = GameObject.Find("City/Roads").transform;

        roads.Add(spawnedRoad);

        // Reset the localScale
        road.transform.localScale = new Vector3(1f, 0.1f, 1f);
    }

    //private void SpawnNewVerticalRoads(Room pRoomA)
    //{
    //    Point roadPos = new Point(0, 0);
    //    roadPos = new Point(pRoomA.area.X, pRoomA.area.Y - pRoomA.roadWidth);

    //    List<WallCoordinates> roadCoordinates = new List<WallCoordinates>();

    //    // Road
    //    WallCoordinates road = new WallCoordinates();
    //    //road.bottomLeft = new Vector3(pRoomA.area.X + pRoomA.area.Width, 0, pRoomA.area.Y);
    //    road.bottomLeft = new Vector3(0, 0, 0);
    //    //road.topRight = new Vector3(pRoomA.area.X + pRoomA.area.Width + pRoomA.roadWidth, pRoomA.area.Y);
    //    road.topRight = new Vector3(100, 100, 0);

    //    GameObject pWallCreator = Instantiate(roadCreator, new Vector3(roadPos.X, 0, roadPos.Y), Quaternion.identity);
    //    pWallCreator.transform.parent = transform;
    //    pWallCreator.transform.eulerAngles = new Vector3(90, 0, 0);

    //    pWallCreator.GetComponent<Renderer>().material = roadMaterial;

    //    pWallCreator.GetComponent<WallPiece>().Generate(roadCoordinates);
    //    roadCoordinates.Clear();
    //}
}
