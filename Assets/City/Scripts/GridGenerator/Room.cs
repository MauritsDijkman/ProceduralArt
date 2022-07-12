using System.Drawing;

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
public class Room
{
    public Rectangle area;
    public int roadWidth;

    //public Room(Rectangle pArea)
    public Room(Rectangle pArea)
    {
        area = pArea;
    }

    //TODO: Implement a toString method for debugging
    //Return information about the type of object and it's data
    //eg Room: (x, y, width, height)

    public override string ToString()
    {
        string information = $"Room: X: {area.X} Y: {area.Y} Width: {area.Width} Height: {area.Height}";
        return information;
    }
}
