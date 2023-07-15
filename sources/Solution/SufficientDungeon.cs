using System.Collections.Generic;
using System.Drawing;

class SufficientDungeon : Dungeon
{
    protected Dictionary<Room, int> roomDoorCount;
    public SufficientDungeon(Size pSize) : base(pSize)
    {
        roomDoorCount = new Dictionary<Room, int>();
    }
    protected override void generate(int pMinimumRoomSize)
    {
        //pMinimumRoomSize *= (int)scale;
        Room roomA = new Room(new Rectangle(0, 0, size.Width, size.Height));
        rooms.Add(roomA); // This is the room that covers the whole screen initially

        CreateRooms(pMinimumRoomSize+2);
        CreateDoors();
        foreach (Room room in rooms)
        {
            DoorCountCheck(room);
        }
    }

    protected override void draw()
    {
        graphics.Clear(Color.Transparent);
        drawRooms(rooms, wallPen);
        drawDoors(doors, doorPen);
    }

    ///Random random = new Random(); //123155 - weird long rooms; 12315 - normal rooms; 1283518 - also kinda long

    void CreateRooms(int pMinimumRoomSize)
    {
        bool roomsModified;
        do //Checks for the possible rooms to be split among all the rooms
        {
            roomsModified = false;
            foreach (Room room in rooms.ToArray()) // Create a copy of the list to avoid modifying it while iterating
            {
                if (room.area.Width > pMinimumRoomSize * 2 || room.area.Height > pMinimumRoomSize * 2) ///Think of a way to make is so that there are no super long rooms.
                {
                    SplitRoom(room, room.area.Width > room.area.Height, pMinimumRoomSize);//Need to check for better split logic here
                    roomsModified = true; //If the room was split, the loop may continue
                }
            }
        } while (roomsModified); //This is not very efficient since there are two nested loops, so O(n^2), however, it makes it possible to work without recursion.
    }
    protected void DoorCountCheck(Room room)
    {
        roomDoorCount[room] = 0;
        foreach (Door door in doors)
        {
            if (door.location.X >= room.area.Left && door.location.X < room.area.Right &&
                door.location.Y >= room.area.Top && door.location.Y < room.area.Bottom)
            {
                room.doorsInRoom.Add(door);
                roomDoorCount[room]++;
            }
        }
    }
    void SplitRoom(Room room, bool verticalSplit, int minimumRoomSize)//WORKING
    {
        Room roomA = null;
        Room roomB = null;
        #region VerticalSplitLogic
        if (verticalSplit)
        {
            int roomAWidth = random.Next(minimumRoomSize, room.area.Width - minimumRoomSize * minimumRoomSize / room.area.Height);
            int roomBWidth = room.area.Width - roomAWidth;
            roomA = new Room(new Rectangle(room.area.Location.X + roomBWidth - 1, room.area.Location.Y, roomAWidth + 1, room.area.Height));
            roomB = new Room(new Rectangle(room.area.Location.X, room.area.Location.Y, roomBWidth, room.area.Height));
        }
        #endregion
        #region HorizontalSplitLogic
        else
        {
            int roomAHeight = random.Next(minimumRoomSize, room.area.Height - minimumRoomSize * minimumRoomSize / room.area.Width);
            int roomBHeight = room.area.Height - roomAHeight;
            roomA = new Room(new Rectangle(room.area.Location.X, room.area.Location.Y + roomBHeight - 1, room.area.Width, roomAHeight + 1));
            roomB = new Room(new Rectangle(room.area.Location.X, room.area.Location.Y, room.area.Width, roomBHeight));
        }
        #endregion


        #region RoomSizeRequirementCheck
        // Check if the new rooms meet the minimum size requirements
        if ((roomA.area.Width < minimumRoomSize || roomB.area.Width < minimumRoomSize || roomA.area.Height < minimumRoomSize || roomB.area.Height < minimumRoomSize)/** || roomA.area.Size.Width < roomA.area.Size.Height / 2.5f || roomA.area.Size.Height < roomA.area.Size.Width / 2.5f || roomB.area.Size.Width < roomB.area.Size.Height / 2.5f || roomB.area.Size.Height < roomB.area.Size.Width / 2.5f/**/)//The last part of this if is a hotfix for long rooms
        {
            return;
        }
        #endregion
        #region RoomArrayUpdate
        rooms.Add(roomA);
        rooms.Add(roomB);
        rooms.Remove(room);
        #endregion

        #region DebugInfo
        ///DEBUG INFO:
        /**
        if (verticalSplit)
            Console.WriteLine("Vertical\t");
        else
            Console.WriteLine("Horizontal\t");
        Console.WriteLine($"RoomA: {roomA}\t");
        Console.WriteLine($"RoomB: {roomB}\t");
        Console.WriteLine($"Door: {door}\n");
        /**/
        #endregion
    }

    void CreateDoors()
    {
        foreach (Room room in rooms)
        {
            int doorX;
            int doorY;
            int randomNum = random.Next(0, 2);
            #region randomDoorPlacement1
            if (randomNum == 0)
            {
                if (room.area.Left > 0)
                {
                    doorY = random.Next(room.area.Top + 1, room.area.Bottom - 1);
                    foreach (Room room1 in rooms)
                    {
                        while ((doorY == room1.area.Top || doorY == room1.area.Bottom - 1) && room.area.Left == room1.area.Right - 1)
                        {
                            doorY = random.Next(room.area.Top + 1, room.area.Bottom - 1);
                        }
                    }
                    Door doorToAdd = new Door(new Point(room.area.Left, doorY));
                    doors.Add(doorToAdd);
                }
                if (room.area.Top > 0)
                {
                    doorX = random.Next(room.area.Left + 1, room.area.Right - 1);
                    foreach (Room room1 in rooms)
                    {
                        while ((doorX == room1.area.Left || doorX == room1.area.Right - 1) && room.area.Top == room1.area.Bottom - 1)
                        {
                            doorX = random.Next(room.area.Left + 1, room.area.Right - 1);
                        }
                    }
                    Door doorToAdd = new Door(new Point(doorX, room.area.Top));
                    doorToAdd.horizontal = true;
                    doors.Add(doorToAdd);
                }
            }
            #endregion
            #region randomDoorPlacement2
            else
            {
                if (room.area.Top > 0)
                {
                    doorX = random.Next(room.area.Left + 1, room.area.Right - 1);
                    foreach (Room room1 in rooms)
                    {
                        while ((doorX == room1.area.Left || doorX == room1.area.Right - 1) && room.area.Top == room1.area.Bottom - 1)
                        {
                            doorX = random.Next(room.area.Left + 1, room.area.Right - 1);
                        }
                    }
                    Door doorToAdd = new Door(new Point(doorX, room.area.Top));
                    doorToAdd.horizontal = true;
                    doors.Add(doorToAdd);
                }
                if (room.area.Left > 0)
                {
                    doorY = random.Next(room.area.Top + 1, room.area.Bottom - 1);
                    foreach (Room room1 in rooms)
                    {
                        while ((doorY == room1.area.Top || doorY == room1.area.Bottom - 1) && room.area.Left == room1.area.Right - 1)
                        {
                            doorY = random.Next(room.area.Top + 1, room.area.Bottom - 1);
                        }
                    }
                    Door doorToAdd = new Door(new Point(room.area.Left, doorY));
                    doors.Add(doorToAdd);
                }
            }
            #endregion
        }
    }
    /**
    void CreateDoors()
    {
        for(int i = 0; i < rooms.Count-1; ++i)
        {
            for (int j = i; j < rooms.Count; ++j)
            {
                Rectangle intersection = Rectangle.Intersect(rooms[i].area, rooms[j].area);
                if (intersection != Rectangle.Empty)
                {
                    Door doorToAdd;
                    if (intersection.Width > intersection.Height)
                    {
                        doorToAdd = new Door(new Point(random.Next(intersection.Left + 1, intersection.Right), intersection.Y));
                        doorToAdd.horizontal = true;
                        doors.Add(doorToAdd);
                    }
                    else
                    {
                        doorToAdd = new Door(new Point(intersection.X, random.Next(intersection.Top + 1, intersection.Bottom)));
                        doors.Add(doorToAdd);
                    }
                    break;
                }
            }
        }
    }
    /**/
}

