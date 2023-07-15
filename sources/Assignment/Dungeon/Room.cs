using System.Drawing;
using System;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
class Room
{
	public Rectangle area;

    public List<Door> doorsInRoom;
    //public readonly int areaSize;
    //public int doorCount;

	public Room (Rectangle pArea)
	{
		area = pArea;
        doorsInRoom = new List<Door>();
	}

    public int areaSize
    {
        get
        {
            return area.Width * area.Height;
        }
    }

    public override string ToString()
    {
        return $"X:{area.Location.X}, Y:{area.Location.Y}, Width:{area.Width}, Height:{area.Height}";
    }
    //TODO: Implement a toString method for debugging?
    //Return information about the type of object and it's data
    //eg Room: (x, y, width, height)
}
