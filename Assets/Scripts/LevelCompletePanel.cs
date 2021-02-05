using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletePanel : MonoBehaviour
{
    public TMP_Text numCoins;
    public TMP_Text level;
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void SetEarnedCoins(int coins)
    {
        numCoins.text = coins.ToString();
    }

    public void SetLevel(int levelNum)
    {
        level.text = "Level " + levelNum.ToString();
    }

    public void OnClickResetButton()
    {
        gameManager.ReloadLevel();
    }
    public void OnClickNextButton()
    {
        gameManager.NextLevel();
    }

}
