using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniTools;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : AnimatedWindowBase
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private TMP_Text resultText;
    public void Show(bool isWin)
    {
        resultText.text = isWin ? "WON!!!" : "DEFEATE";
        restartBtn.SetActive(!isWin);

        connections += restartBtn.Subscribe(() => GameSession.ReloadGame());
    }
}
