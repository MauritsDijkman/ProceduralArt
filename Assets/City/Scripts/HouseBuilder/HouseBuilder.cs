using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class HouseBuilder : MonoBehaviour
{
    // Lists
    private List<Room> rooms = new List<Room>();
    private List<GameObject> houses = new List<GameObject>();

    // Position
    private Vector2 position = new Vector2(0, 0);

    // Grid
    private Vector2 gridSize = new Vector2(0, 0);

    [Header("Room sizes")]
    [SerializeField] private int minimumRoomWidth = 10;
    [SerializeField] private int minimumRoomHeight = 10;

    [Header("Generation")]
    [SerializeField] private bool generateVertical = false;
    [SerializeField] private bool generateHorizontal = true;

    [Header("Road Horizontal")]
    [SerializeField] private int roadWidthHorizontal = 0;

    [Header("Road Vertical")]
    [SerializeField] private int roadWidthVertical = 1;
    [Tooltip("When random is turned off, 'Road Width Vertical' will be used for the width of all vertical roads")]
    [SerializeField] private bool randomWidth = false;
    [SerializeField] private int minRoadWidth = 2;
    [SerializeField] private int maxRoadWidth = 4;

    [Header("House")]
    [SerializeField] private bool buildHouses = true;

    [Header("Visual")]
    [SerializeField] private bool drawRooms = false;

    public void Initialize(Vector2 pPosition, Vector2 pGridSize)
    {
        position = pPosition;
        gridSize = pGridSize;
    }

    public void ResetLists()
    {
        rooms.Clear();

        foreach (GameObject house in houses)
        {
            if (Application.isPlaying)
                GameObject.Destroy(house.gameObject);
            else if (Application.isEditor)
                GameObject.DestroyImmediate(house.gameObject);
        }

        houses.Clear();
    }

    public void Generate()
    {
        ResetLists();

        int pRoadWidthVertical = roadWidthVertical;
        if (randomWidth)
            pRoadWidthVertical = Random.Range(minRoadWidth, maxRoadWidth + 1);

        GridAlgorithm gridAlgorithm = new GridAlgorithm();
        gridAlgorithm.Initialize(position, gridSize, minimumRoomWidth, minimumRoomHeight, roadWidthHorizontal, pRoadWidthVertical, generateVertical, generateHorizontal, drawRooms);
        rooms = gridAlgorithm.GenerateRooms();

        foreach (Room room in rooms)
        {
            //Debug.Log($"Pos X: {room.area.X} | Pos Y: {room.area.Y}");

            if (buildHouses)
                BuildHouse(room.area);
        }
    }

    private void BuildHouse(Rectangle rectangle)
    {
        Vector3 spawnPosition = new Vector3(rectangle.X, 0, rectangle.Y);

        GameObject spawnedHouse = Instantiate(FindObjectOfType<RandomBuildingGenerator>().GetRandomHouse(), spawnPosition, Quaternion.identity);

        System.Random rand = new System.Random();

        if (spawnedHouse.GetComponent<SquareBuilding>() != null)
        {
            SquareBuilding script = spawnedHouse.GetComponent<SquareBuilding>();

            script.Initialize(new Vector2(rectangle.X, rectangle.Y), rectangle.Width, rectangle.Height, null, null);
            script.Generate();

        }
        else if (spawnedHouse.GetComponent<Flat>() != null)
        {
            Flat script = spawnedHouse.GetComponent<Flat>();

            script.Initialize(new Vector2(rectangle.X, rectangle.Y), rectangle.Width, rectangle.Height);
            script.Generate();
        }
        else if (spawnedHouse.GetComponent<SkyScraper>() != null)
        {
            SkyScraper script = spawnedHouse.GetComponent<SkyScraper>();

            script.Initialize(new Vector2(rectangle.X, rectangle.Y), rectangle.Width, rectangle.Height);
            script.Generate();
        }
        else if (spawnedHouse.GetComponent<GabBuilding>() != null)
        {
            GabBuilding script = spawnedHouse.GetComponent<GabBuilding>();

            script.Initialize(new Vector2(rectangle.X, rectangle.Y), rectangle.Width, rectangle.Height);
            script.Generate();
        }

        spawnedHouse.transform.parent = transform;
        houses.Add(spawnedHouse);
    }
}
