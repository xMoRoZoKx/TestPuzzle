using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameFieldView : MonoBehaviour
{
    [SerializeField] private TileView tileViewPrefab;
    [SerializeField] private float tileSize = 1f;
    private TileView[,] generatedTiles; 

    public void StartGame(PuzzleSolver solver, Action onGridChanged)
    {
        GenerateGrid(solver.Grid, tile =>
        {
            onGridChanged?.Invoke();
            HighlightAllConnectedTiles(solver);
        });
        HighlightAllConnectedTiles(solver);
    }

    public void HighlightAllConnectedTiles(PuzzleSolver solver)
    {
        TileSetting startTile = solver.Grid.FindTile(TileType.Start);

        foreach (var tile in generatedTiles)
        {
            tile.Highlight(false);
        }

        solver.GetAllConnected(startTile).ForEach(tileCoord =>
        {
            generatedTiles[tileCoord.x, tileCoord.y].Highlight(true);
        });
    }

    private void GenerateGrid(Serializable2DArray<TileSetting> grid, Action<TileView> onTileRotate)
    {
        int rows = grid.Rows;
        int cols = grid.Columns;

        generatedTiles = new TileView[cols, rows]; 

        Vector2 gridSize = new Vector2(cols * tileSize, -rows * tileSize);
        Vector2 gridCenterOffset = gridSize * 0.5f - new Vector2(tileSize, -tileSize) * 0.5f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                TileSetting setting = grid.GetValue(col, row);

                TileView tileInstance = Instantiate(tileViewPrefab, transform);
                generatedTiles[col, row] = tileInstance;

                Vector3 pos = new Vector3(col * tileSize, -row * tileSize, 0);
                pos -= (Vector3)gridCenterOffset;
                tileInstance.transform.localPosition = pos;

                tileInstance.SetTile(setting, () => onTileRotate?.Invoke(tileInstance));
            }
        }
    }
}
