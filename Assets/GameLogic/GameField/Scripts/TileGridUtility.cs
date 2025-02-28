using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileGridUtility
{

    public static Serializable2DArray<TileSetting> CreateTileGridCopy(this Serializable2DArray<TileSetting> original)
    {
        Serializable2DArray<TileSetting> copy = new Serializable2DArray<TileSetting>(original.Columns, original.Rows);

        for (int row = 0; row < original.Rows; row++)
        {
            for (int col = 0; col < original.Columns; col++)
            {
                TileSetting originalTile = original.GetValue(col, row);
                TileSetting newTile = new TileSetting
                {
                    tile = originalTile.tile,
                    rotate = originalTile.rotate
                };
                copy.SetValue(col, row, newTile);
            }
        }

        return copy;
    }

    public static TileSetting FindTile(this Serializable2DArray<TileSetting> grid, TileType type)
    {
        for (int row = 0; row < grid.Rows; row++)
        {
            for (int col = 0; col < grid.Columns; col++)
            {
                TileSetting tile = grid.GetValue(col, row);
                if (tile.tile.tileType == type)
                    return tile;
            }
        }
        return null;
    }

    public static List<TileSetting> FindAllTiles(this Serializable2DArray<TileSetting> grid, TileType type)
    {
        List<TileSetting> tiles = new();
        for (int row = 0; row < grid.Rows; row++)
        {
            for (int col = 0; col < grid.Columns; col++)
            {
                TileSetting tile = grid.GetValue(col, row);
                if (tile.tile.tileType == type)
                    tiles.Add(tile);
            }
        }
        return tiles;
    }
}
