using GXPEngine;
using System;
using System.Drawing;

class LowLevelDungeonNodeGraph : HighLevelDungeonNodeGraph
{
    private enum GraphConnectionType { SquareConnection, FullConnection };
    private GraphConnectionType _connectionType;
    public LowLevelDungeonNodeGraph(Dungeon pDungeon) : base(pDungeon)
    {
        _connectionType = GraphConnectionType.SquareConnection;
        Console.WriteLine($"\t!!!The graph connection type is {_connectionType}");
        Console.WriteLine("------ Press M to switch the connection type(Square/Full) ------");
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(Key.M))
        {
            _connectionType++;
            if ((int)_connectionType > 1)
                _connectionType = 0;
            Generate();
        }
    }

    protected override void generate()
    {
        roomsNodes();
        createDoorNodes();
        generateNodeConnections();
    }

    private void roomsNodes()
    {
        foreach (Room room in _dungeon.rooms)
        {
            if (room.doorsInRoom.Count > 0)
                CreateRoomNodes(room);
        }
    }

    private void CreateRoomNodes(Room room)
    {
        for (int i = room.area.Left + 1; i < room.area.Right - 1; ++i)
        {
            for (int j = room.area.Top + 1; j < room.area.Bottom - 1; ++j)
            {
                Node nodeToAdd = new Node(getPointCenter(new Point(i, j)));
                nodes.Add(nodeToAdd);
            }
        }
    }

    private void generateNodeConnections()
    {
        for (int i = 0; i < nodes.Count; ++i)
        {
            for (int j = 0; j < nodes.Count; ++j)
            {
                switch (_connectionType)
                {
                    case GraphConnectionType.SquareConnection:
                        if ((nodes[i].location.X == nodes[j].location.X - 1 * (int)_dungeon.scale && nodes[i].location.Y == nodes[j].location.Y) || (nodes[i].location.Y == nodes[j].location.Y - 1 * (int)_dungeon.scale && nodes[i].location.X == nodes[j].location.X))
                        {
                            AddConnection(nodes[i], nodes[j]);
                        }
                        break;
                    case GraphConnectionType.FullConnection:
                        if ((nodes[i].location.X == nodes[j].location.X - 1 * (int)_dungeon.scale && nodes[i].location.Y == nodes[j].location.Y) || (nodes[i].location.Y == nodes[j].location.Y - 1 * (int)_dungeon.scale && nodes[i].location.X == nodes[j].location.X) || (nodes[i].location.X == nodes[j].location.X - 1 * (int)_dungeon.scale && nodes[i].location.Y == nodes[j].location.Y - 1 * (int)_dungeon.scale) || ((nodes[i].location.X == nodes[j].location.X - 1 * (int)_dungeon.scale && nodes[i].location.Y == nodes[j].location.Y + 1 * (int)_dungeon.scale)))
                        {
                            AddConnection(nodes[i], nodes[j]);
                        }
                        break;
                }
            }
        }
    }
}
