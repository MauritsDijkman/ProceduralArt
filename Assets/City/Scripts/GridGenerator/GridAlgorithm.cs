using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GridAlgorithm : MonoBehaviour
{
    // If you put an number between the brackets, you get the same random generation.
    private static System.Random random = new System.Random();

    // Original value
    private Vector2 position = new Vector2(0, 0);
    private Vector2 gridSize = new Vector2(0, 0);
    private int minRoomWidth = 0;
    private int minRoomHeight = 0;
    private int sizeBetweenRoomsHorizontal = 0;
    private int sizeBetweenRoomsVertical = 0;

    // Lists
    public List<Room> finalRooms = new List<Room>();
    private List<Room> splittableRoomsVertical = new List<Room>();
    private List<Room> splittableRoomsHorizontal = new List<Room>();

    // Booleans
    private bool generateVertical = true;
    private bool generateHorizontal = true;
    private bool drawRooms = true;


    public void Initialize(Vector2 pPosition, Vector2 pGridSize, int pMinRoomWidth, int pMinRoomHeight, int pSizeBetweenRoomsHorizontal, int pSizeBetweenRoomsVertical, bool pGenerateVertical = true, bool pGenerateHorizontal = true, bool pDrawRooms = true)
    {
        position = pPosition;
        gridSize = pGridSize;
        minRoomWidth = pMinRoomWidth;
        minRoomHeight = pMinRoomHeight;
        sizeBetweenRoomsHorizontal = pSizeBetweenRoomsHorizontal;
        sizeBetweenRoomsVertical = pSizeBetweenRoomsVertical;
        generateVertical = pGenerateVertical;
        generateHorizontal = pGenerateHorizontal;
        drawRooms = pDrawRooms;
    }

    private void ResetValues()
    {
        finalRooms.Clear();
        random = FindObjectOfType<RandomGenerator>().GetRandom();
    }

    public List<Room> GenerateRooms()
    {
        ResetValues();

        Room newRoom = new Room(new Rectangle((int)position.x, (int)position.y, (int)gridSize.x, (int)gridSize.y));
        finalRooms.Add(newRoom);

        Room chosenRoom = newRoom;

        if (generateHorizontal)
        {
            while (chosenRoom != null)
            {
                chosenRoom = RoomDetectVertical();

                if (chosenRoom != null)
                    SplitVertical(chosenRoom);
            }
        }

        if (generateVertical)
        {
            chosenRoom = finalRooms[0];
            while (chosenRoom != null)
            {
                chosenRoom = RoomDetectHorizontal();

                if (chosenRoom != null)
                    SplitHorizontal(chosenRoom);
            }
        }

        if (drawRooms)
        {
            foreach (Room room in finalRooms)
            {
                Debug.Log($"Position: {new Vector2(room.area.X, room.area.Y)} || Width: {room.area.Width} || Height: {room.area.Height}");
                DrawRoom(room.area, UnityEngine.Color.red);
            }
        }

        // Debug line
        // Debug.Log($"Done!");

        // Return the list with rooms
        return finalRooms;
    }

    private void DrawRoom(Rectangle rectangle, UnityEngine.Color drawColor)
    {
        float yPos = 1f;            // Y place position
        float durationTime = 30f;   // How long the lines are visible

        Vector3 posBL = new Vector3(rectangle.X, yPos, rectangle.Y);                                        // Bottom Left
        Vector3 posTL = new Vector3(rectangle.X, yPos, rectangle.Y + rectangle.Height);                     // Top Left
        Vector3 posBR = new Vector3(rectangle.X + rectangle.Width, yPos, rectangle.Y);                      // Bottom Right
        Vector3 posTR = new Vector3(rectangle.X + rectangle.Width, yPos, rectangle.Y + rectangle.Height);   // Top Right

        Debug.DrawLine(posTL, posTR, drawColor, durationTime);  // Top left to top right
        Debug.DrawLine(posBL, posBR, drawColor, durationTime);  // Bottom left to bottom right
        Debug.DrawLine(posTL, posBL, drawColor, durationTime);  // Top left to bottom left
        Debug.DrawLine(posTR, posBR, drawColor, durationTime);  // Top right to bottom right
    }

    //Check for splittable rooms and return one. If it can't be done, return null.
    private Room RoomDetectVertical()
    {
        // Make sure to start clean.
        splittableRoomsVertical.Clear();

        //Check for splittable rooms vertically and adds it to the list.
        for (int i = 0; i < finalRooms.Count; i++)
        {
            Room currentRoom = finalRooms[i];
            if (currentRoom.area.Width > minRoomWidth * 2)
                splittableRoomsVertical.Add(currentRoom);
        }

        // Takes random room in the list and returns it.
        if (splittableRoomsVertical.Count > 0)
        {
            int index = random.Next(0, splittableRoomsVertical.Count);
            Room newRandomRoom = splittableRoomsVertical[index];
            return newRandomRoom;
        }
        else
            return null;
    }

    //Checks for splittable rooms and returns one. If it can't return one, it returns null.
    private Room RoomDetectHorizontal()
    {
        // Make sure to start clean.
        splittableRoomsHorizontal.Clear();

        //Check for splittable rooms horizontally.
        for (int i = 0; i < finalRooms.Count; i++)
        {
            Room currentRoom = finalRooms[i];
            if (currentRoom.area.Height > minRoomHeight * 2)
                splittableRoomsHorizontal.Add(currentRoom);
        }

        // Takes random room in the list and returns it.
        if (splittableRoomsHorizontal.Count > 0)
        {
            int index = random.Next(0, splittableRoomsHorizontal.Count);
            Room newRandomRoom = splittableRoomsHorizontal[index];
            return newRandomRoom;
        }
        else
            return null;
    }

    // Splits the rooms vertical.
    private void SplitVertical(Room room)
    {
        // Make sure to start clean.
        finalRooms.Remove(room);
        splittableRoomsVertical.Remove(room);

        if (room.area.Width > minRoomWidth * 2)
        {
            // Gets the new size by getting the random new width from the previous room size.
            int maxRoomSize = room.area.Width - minRoomWidth;
            int newWidth = random.Next(minRoomWidth, maxRoomSize);

            int pSizeBetweenRooms = sizeBetweenRoomsHorizontal;

            // Gets the left and right side of the room and adds these.
            Room roomLeft = new Room(new Rectangle(room.area.X, room.area.Y, newWidth - pSizeBetweenRooms, room.area.Height));
            roomLeft.roadWidth = pSizeBetweenRooms;
            Room roomRight = new Room(new Rectangle(room.area.X + newWidth, room.area.Y, room.area.Width - newWidth, room.area.Height));
            roomRight.roadWidth = pSizeBetweenRooms;

            // Adds both the left and right toom and draws them.
            finalRooms.Add(roomLeft);
            finalRooms.Add(roomRight);

            // Debug to see information about all the vertical rooms in the maze.
            /**
            System.Console.WriteLine(roomLeft.ToString());
            System.Console.WriteLine(roomRight.ToString());
            /**/
        }
    }

    // Splits the rooms horizontal.
    private void SplitHorizontal(Room room)
    {
        // Make sure to start clean.
        finalRooms.Remove(room);
        splittableRoomsHorizontal.Remove(room);

        //Make sure the room can be split between 2 minimum-sized rooms.
        if (room.area.Height > minRoomHeight * 2)
        {
            // Gets the new size by getting the random new width from the previous room size.
            int maxRoomSize = room.area.Height - minRoomHeight;
            int newHeight = random.Next(minRoomHeight, maxRoomSize);

            int pSizeBetweenRooms = sizeBetweenRoomsVertical;

            // Gets the left and right side of the room and adds these.
            Room roomTop = new Room(new Rectangle(room.area.X, room.area.Y, room.area.Width, newHeight - pSizeBetweenRooms));
            roomTop.roadWidth = pSizeBetweenRooms;
            Room roomBottom = new Room(new Rectangle(room.area.X, room.area.Y + newHeight, room.area.Width, room.area.Height - newHeight));
            roomBottom.roadWidth = pSizeBetweenRooms;

            // Adds both the left and right toom and draws them.
            finalRooms.Add(roomTop);
            finalRooms.Add(roomBottom);

            // Debug to see information about all the horizontal rooms in the maze.
            /**
            System.Console.WriteLine(roomTop.ToString());
            System.Console.WriteLine(roomBottom.ToString());
            /**/
        }
    }
}
