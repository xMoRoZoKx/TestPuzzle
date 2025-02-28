using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniTools;
using UniTools.Reactive;
using UnityEngine;

public class GameHUD : WindowBase
{
    [SerializeField] private TMP_Text timerText;
    public void Show(Reactive<float> timer)
    {
        connections += timerText.SetTextReactive(timer, value => value.ToString("0."));
    }
}
