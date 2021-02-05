using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainHud : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager gameManager;
    public TMP_Text numCoinsText;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 
    }

    // Update is called once per frame
    public void OpenSettings()
    {
        gameManager.OpenSettings();
        gameManager.SetPaused(true);
    }

    public void SetCoins(int numCoins)
    {
        numCoinsText.text = numCoins.ToString();
    }

    public void CloseStore()
    {
        gameManager.LoadCurrentLevel();
    }
}
