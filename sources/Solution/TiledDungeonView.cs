class TiledDungeonView : TiledView
{
    private Dungeon _dungeon;
    public TiledDungeonView(Dungeon pDungeon, TileType pDefaultTileType) : base(pDungeon.size.Width, pDungeon.size.Height, (int)pDungeon.scale, pDefaultTileType)
    {
        _dungeon = pDungeon;
    }

    void CheckRoomBoundries()
    {
        foreach (Room room in _dungeon.rooms)
        {
            for (int i = room.area.Left; i < room.area.Right; ++i)       //GoThroughCollumns
            {
                SetTileType(i, room.area.Top, TileType.WALL);
                SetTileType(i, room.area.Bottom - 1, TileType.WALL);
            }
            for (int i = room.area.Top; i < room.area.Bottom; ++i)     //GoThroughRows
            {
                SetTileType(room.area.Left, i, TileType.WALL);
                SetTileType(room.area.Right - 1, i, TileType.WALL);
            }
            for (int i = room.area.Left+1; i < room.area.Right-1; ++i)
            {
                for (int j = room.area.Top+1; j < room.area.Bottom-1; ++j)
                {
                    SetTileType(i, j, TileType.GROUND);
                }
            }
        }
    }
    void CheckDoorPlacement()
    {
        foreach (Door door in _dungeon.doors)
        {
            SetTileType(door.location.X, door.location.Y, TileType.GROUND);
        }
    }

    protected override void generate()
    {
        CheckRoomBoundries();
        CheckDoorPlacement();
    }
}
