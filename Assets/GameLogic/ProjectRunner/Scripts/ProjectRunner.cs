using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniTools;
using UniTools.Reactive;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D.IK;

public class ProjectRunner : MonoBehaviour
{
    [SerializeField] private GameFieldView gameFieldView;
    [SerializeField] private TileGridSettings tileGridSettings;
    private void Awake()
    {
        var timerReactive = SetupTimer(tileGridSettings.timeForSolve, out Tweener timerTweener);

        WindowManager.Instance.Show<GameHUD>(window => window.Show(timerReactive));

        PuzzleSolver solver = new(tileGridSettings.tilesData.CreateTileGridCopy());

        gameFieldView.StartGame(solver, () =>
        {
            if (CheckWinCondition(solver))
            {
                WindowManager.Instance.Show<ResultScreen>(window => window.Show(true));
                timerTweener.Kill();
            }
        });
    }
    public Reactive<float> SetupTimer(float playTime, out Tweener timerTweener)
    {
        Reactive<float> timer = new(playTime);

        timerTweener = DOVirtual.Float(playTime, 0, playTime, currentTime =>
        {
            timer.value = currentTime;
        })
        .OnComplete(() =>
        {
            WindowManager.Instance.Show<ResultScreen>(window => window.Show(false));
        }).SetEase(Ease.Linear);

        return timer;
    }
    public bool CheckWinCondition(PuzzleSolver solver)
    {
        TileSetting startTile = solver.Grid.FindTile(TileType.Start);
        List<TileSetting> finishTiles = solver.Grid.FindAllTiles(TileType.Finish);

        if (startTile == null || finishTiles.Count == 0)
        {
            Debug.LogError("Start or Finish tile not found!");
            return false;
        }
        //finishTiles.ForEach(finishTile => Debug.LogError(solver.FindPatch(startTile, finishTile).Count()));
        if (finishTiles.All(finishTile => solver.FindPatch(startTile, finishTile))) return true;

        return false;
    }
}
