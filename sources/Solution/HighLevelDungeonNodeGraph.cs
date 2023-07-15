using System;
using System.Collections.Generic;
using System.Drawing;

class HighLevelDungeonNodeGraph : NodeGraph
{
    protected Dungeon _dungeon;
    private Dictionary<Room, Node> roomNode = new Dictionary<Room, Node>();
    protected Dictionary<Door, Node> doorNode = new Dictionary<Door, Node>();
    public HighLevelDungeonNodeGraph(Dungeon pDungeon) : base(pDungeon.width, pDungeon.height, (int)(pDungeon.scale/3))
    {
        _dungeon = pDungeon;
    }

    protected override void generate()
    {
        createNodes();
        connectNodes();
    }

    private bool CanAddRoomNode(Room room)
    {
        foreach(Door door in _dungeon.doors)
        {
            if (room.area.Left <= door.location.X && door.location.X < room.area.Right &&
               room.area.Top <= door.location.Y && door.location.Y < room.area.Bottom && room.doorsInRoom.Count > 0)
                return true;
        }
        return false;
    }

    private void createNodes()//Use dictionaries to keep track of the nodes inside each room and door
    {
        createRoomNodes();
        createDoorNodes();
        //connectNodes();
    }
    private void createRoomNodes()
    {
        foreach (Room room in _dungeon.rooms)
        {
            if (CanAddRoomNode(room))
            {
                roomNode[room] = null;
                Node nodeToAdd = new Node(getRoomCenter(room));
                nodes.Add(nodeToAdd);
                roomNode[room] = nodeToAdd;
            }
        }
    }
    protected void createDoorNodes()
    {
        foreach (Door door in _dungeon.doors)
        {
            Node nodeToAdd = new Node(getDoorCenter(door));
            nodes.Add(nodeToAdd);
            doorNode[door] = nodeToAdd;
        }
    }

    private void connectNodes()//Connect all the nodes(center of the room to surrounding the room)
    {
        foreach(Room room in _dungeon.rooms)
        {
            foreach (Door door in _dungeon.doors)
            {
                if (door.location.X >= room.area.Left && door.location.X < room.area.Right &&
                   door.location.Y >= room.area.Top && door.location.Y < room.area.Bottom)
                {
                    AddConnection(roomNode[room], doorNode[door]);
                }
            }
        }
    }

    /**
	 * A helper method for your convenience so you don't have to meddle with coordinate transformations.
	 * @return the location of the center of the given room you can use for your nodes in this class
	 */
    protected Point getRoomCenter(Room pRoom)
    {
        float centerX = ((pRoom.area.Left + pRoom.area.Right) / 2.0f) * _dungeon.scale;
        float centerY = ((pRoom.area.Top + pRoom.area.Bottom) / 2.0f) * _dungeon.scale;
        return new Point((int)centerX, (int)centerY);
    }

    /**
	 * A helper method for your convenience so you don't have to meddle with coordinate transformations.
	 * @return the location of the center of the given door you can use for your nodes in this class
	 */
    protected Point getDoorCenter(Door pDoor)
    {
        return getPointCenter(pDoor.location);
    }

    /**
	 * A helper method for your convenience so you don't have to meddle with coordinate transformations.
	 * @return the location of the center of the given point you can use for your nodes in this class
	 */
    protected Point getPointCenter(Point pLocation)
    {
        float centerX = (pLocation.X + 0.5f) * _dungeon.scale;
        float centerY = (pLocation.Y + 0.5f) * _dungeon.scale;
        return new Point((int)centerX, (int)centerY);
    }
}
