using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleSolver
{
    public Serializable2DArray<TileSetting> Grid;
    private int width, height;
    private Dictionary<Vector2Int, Vector2Int> cameFrom;

    public PuzzleSolver(Serializable2DArray<TileSetting> tilesData)
    {
        Grid = tilesData;
        width = tilesData.Columns;
        height = tilesData.Rows ;
    }

    public bool FindPatch(TileSetting start, TileSetting finish)
    {
        Vector2Int? startPoint = FindTilePosition(start);
        Vector2Int? finishPoint = FindTilePosition(finish);

        if (!startPoint.HasValue || !finishPoint.HasValue) return false;

        return GetAllConnected(start).Any(point => point == finishPoint);
    }
    public List<Vector2Int> GetAllConnected(TileSetting start)
    {
        Vector2Int? startPoint = FindTilePosition(start);
        if (!startPoint.HasValue) return new List<Vector2Int>();

        Dictionary<Vector2Int, HashSet<SideType>> visited = new();
        ExploreConnectedTiles(startPoint.Value, visited, null);

        return visited.Keys.ToList(); 
    }

    private void ExploreConnectedTiles(Vector2Int current, Dictionary<Vector2Int, HashSet<SideType>> visited, SideType? cameFromSide)
    {
        if (!visited.ContainsKey(current))
            visited[current] = new HashSet<SideType>();

        if (cameFromSide.HasValue && visited[current].Contains(cameFromSide.Value))
            return;

        if (cameFromSide.HasValue)
            visited[current].Add(cameFromSide.Value);

        TileSetting currentTile = Grid.GetValue(current.x, current.y);

        foreach (SideType side in GetValidDirections(currentTile, cameFromSide))
        {
            SideType rotatedSide = GetRotatedSide(side, currentTile.rotate);
            Vector2Int neighbor = current + GetOffset(rotatedSide);

            if (!IsInBounds(neighbor)) continue;

            TileSetting neighborTile = Grid.GetValue(neighbor.x, neighbor.y);
            SideType oppositeSide = GetOppositeSide(rotatedSide);

            if (!CanConnect(neighborTile, neighbor, oppositeSide)) continue;

            ExploreConnectedTiles(neighbor, visited, oppositeSide);
        }
    }
    private Vector2Int? FindTilePosition(TileSetting tile)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (Grid.GetValue(x, y) == tile)
                    return new Vector2Int(x, y);
        return null;
    }


    private List<SideType> GetValidDirections(TileSetting tile, SideType? cameFromSide)
    {
        List<SideType> validDirections = new();
        foreach (Patch patch in tile.tile.patches)
        {
            if (cameFromSide.HasValue && !patch.patch.Any(p => GetRotatedSide(p, tile.rotate) == cameFromSide.Value)) continue;

            foreach (SideType side in patch.patch)
                validDirections.Add(side);
        }
        return validDirections;
    }

    private bool CanConnect(TileSetting tile, Vector2Int pos, SideType requiredSide)
    {
        foreach (Patch patch in tile.tile.patches)
        {
            foreach (SideType side in patch.patch)
                if (GetRotatedSide(side, tile.rotate) == requiredSide)
                    return true;
        }
        return false;
    }

    private SideType GetRotatedSide(SideType side, RotateType rotation)
    {
        return rotation switch
        {
            RotateType.R90 => side switch
            {
                SideType.Left => SideType.Up,
                SideType.Up => SideType.Right,
                SideType.Right => SideType.Bottom,
                SideType.Bottom => SideType.Left,
                _ => side
            },
            RotateType.R180 => side switch
            {
                SideType.Left => SideType.Right,
                SideType.Right => SideType.Left,
                SideType.Up => SideType.Bottom,
                SideType.Bottom => SideType.Up,
                _ => side
            },
            RotateType.R270 => side switch
            {
                SideType.Left => SideType.Bottom,
                SideType.Bottom => SideType.Right,
                SideType.Right => SideType.Up,
                SideType.Up => SideType.Left,
                _ => side
            },
            _ => side
        };
    }

    private SideType GetOppositeSide(SideType side)
    {
        return side switch
        {
            SideType.Left => SideType.Right,
            SideType.Right => SideType.Left,
            SideType.Up => SideType.Bottom,
            SideType.Bottom => SideType.Up,
            _ => side
        };
    }

    private Vector2Int GetOffset(SideType side)
    {
        return side switch
        {
            SideType.Left => new Vector2Int(-1, 0),
            SideType.Right => new Vector2Int(1, 0),
            SideType.Up => new Vector2Int(0, -1),
            SideType.Bottom => new Vector2Int(0, 1),
            _ => Vector2Int.zero
        };
    }

    private bool IsInBounds(Vector2Int pos) => pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;
}
