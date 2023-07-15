using System.Drawing;
class ExcellentDungeon : SufficientDungeon
{
    public ExcellentDungeon(Size pSize) : base(pSize) { }

    protected override void draw()
    {
        graphics.Clear(Color.Black);
        ReduceRoomSize();
        //SetDoors();
        drawRooms(rooms, wallPen, new SolidBrush(Color.White));
        drawDoors(doors, Pens.White);
    }

    void ReduceRoomSize()
    {
        /**/
        foreach (Room room in rooms)
        {
            room.area.Width -= 2;
            room.area.Height -= 2;
        }
        /**/
        //foreach(Door door in doors)
        //{
        //    door.location.X -= 3;
        //    door.location.Y -= 4;
        //}
        //RepositionDoors();
    }

    void RepositionDoors()
    {
        foreach (Door door in doors)
        {
            if (door.horizontal)
            {
                if (door.location.X > door.roomB.area.Left + 1 || door.location.X > door.roomA.area.Left +1)
                    door.location.X -= 2;
                else
                {
                    door.location.X = (door.roomB.area.Left > door.roomA.area.Left) ? door.roomB.area.Left : door.roomA.area.Left;
                }
            }
            else
            {
                if (door.location.Y > door.roomB.area.Top + 1 || door.location.Y > door.roomA.area.Top + 1)
                    door.location.Y -= 2;
                else
                {
                    door.location.Y = (door.roomB.area.Top > door.roomA.area.Top) ? door.roomB.area.Top : door.roomA.area.Top;
                }
            }
        }
    }

    void SetDoors()
    {
        foreach (Door door in doors.ToArray())
        {
            /**
            if(rooms.Contains(door.roomA))
                Console.WriteLine($"Rooms contains: {door.roomA}; RoomA");
            else
                Console.WriteLine($"Rooms doesn't contain: {door.roomA}; RoomA");
            Console.WriteLine(door.roomA.area.Left);
            if(rooms.Contains(door.roomB))
                Console.WriteLine($"Rooms contains: {door.roomB}; RoomB");
            else
                Console.WriteLine($"Rooms doesn't contain: {door.roomB}; RoomB");
            Console.WriteLine(door.roomB.area.Right);
            /**/
            if (door.location.X > door.roomB.area.Right - 0.5f && !door.horizontal)
            {
                for (int i = 0; i < (door.location.X - door.roomB.area.Right + 0.5f); ++i)
                {
                    Door newDoor = new Door(new Point(door.location.X - i - 1, door.location.Y));
                    newDoor.roomA = door.roomA;
                    newDoor.roomB = door.roomB;
                    newDoor.horizontal = door.horizontal;
                    doors.Add(newDoor);
                }
            }
            if (door.location.Y > door.roomB.area.Bottom - 0.5f && door.horizontal)
            {
                for (int i = 0; i < (door.location.Y - door.roomB.area.Bottom + 0.5f); ++i)
                {
                    Door newDoor = new Door(new Point(door.location.X, door.location.Y - i - 1));
                    newDoor.roomA = door.roomA;
                    newDoor.roomB = door.roomB;
                    newDoor.horizontal = door.horizontal;
                    doors.Add(newDoor);
                }
            }
            /**
            if (door.location.Y < door.roomA.area.Top && door.horizontal)
            {
                Console.WriteLine("HappensY\t");
                for (int i = 0; i < (door.roomA.area.Top - door.location.X); ++i)
                {
                    doors.Add(new Door(new Point(door.location.X, door.location.Y+i+1)));
                }
            }
            if (door.location.X < door.roomA.area.Left && !door.horizontal)
            {
                Console.WriteLine("HappensX\t");
                for (int i = 0; i < (door.roomA.area.Left - door.location.X); ++i)
                {
                    doors.Add(new Door(new Point(door.location.X + i + 1, door.location.Y)));
                }
            }
            /**/
        }
    }

}
