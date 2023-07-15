using System;
using System.Collections.Generic;
using System.Drawing;

class GoodDungeon : SufficientDungeon
{
    public GoodDungeon(Size psize) : base(psize)
    {
    }
    protected override void generate(int pMinimumRoomSize)
    {
        base.generate(pMinimumRoomSize);
        CheckRemoveRooms();
    }
    protected override void draw()
    {
        graphics.Clear(Color.Transparent);
        drawRooms();
        drawDoors(doors, doorPen);
    }

    void RemoveDoor(Room room)
    {
        foreach (Door door in doors.ToArray())
        {
            if (door.location.X >= room.area.Left && door.location.X < room.area.Right &&
                door.location.Y >= room.area.Top && door.location.Y < room.area.Bottom)
            {
                room.doorsInRoom.Remove(door);
                doors.Remove(door);
            }
        }
    }
    void drawRooms() //Check for the amount of doors in each room. Sets the color then draws the room.
    {
        foreach (Room room in rooms)
        {
            DoorCountCheck(room);
            if (roomDoorCount.ContainsKey(room))
            {
                int doorCount = roomDoorCount[room];

                if (doorCount == 0)
                    roomFill = new SolidBrush(Color.Red);
                else if (doorCount == 1)
                    roomFill = new SolidBrush(Color.Orange);
                else if (doorCount == 2)
                    roomFill = new SolidBrush(Color.Yellow);
                else
                    roomFill = new SolidBrush(Color.Green);
                drawRoom(room, wallPen, roomFill);
            }
        }
    }

    void CheckRemoveRooms()
    {
        int smallestRoomSize = game.width * game.height;
        int biggestRoomSize = 0;
        foreach (Room room in rooms)
        {
            if (room.areaSize < smallestRoomSize)
                smallestRoomSize = room.areaSize;
            if (room.areaSize > biggestRoomSize)
                biggestRoomSize = room.areaSize;
        }
        foreach (Room room in rooms.ToArray())
        {
            if (room.areaSize == smallestRoomSize || room.areaSize == biggestRoomSize)
            {
                Console.WriteLine("Removing room: {0}", room);
                rooms.Remove(room);
                RemoveDoor(room);
            }
        }
    }
}