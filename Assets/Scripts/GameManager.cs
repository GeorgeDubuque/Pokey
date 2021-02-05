using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const string LEVEL = "Level "; 
    bool levelComplete = false;
    public bool paused = false;
    public GameObject[] skins;
    public Dictionary<string, GameObject> skinDict;
    public LevelCompletePanel levelStatusPanel;
    public MainPanel mainPanel;
    public SettingsPanel settingsPanel;
    public DebugPanel debugPanel;
    public MainHud hud;
    public StorePanel storePanel;
    PlayerController playerController;

    private void Awake()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;

        skinDict = new Dictionary<string, GameObject>();
        foreach(var skin in skins)
        {
            skinDict.Add(skin.name, skin);
        }

        SetCoins(GameState.numCoins);
        playerController = FindObjectOfType<PlayerController>();

        if(activeSceneName != "Shop")
        {
            int activeSceneNum = Int32.Parse(activeSceneName.Split()[1]);
            GameState.currLevelNum = activeSceneNum;
            GameState.levelsCompleted.Add(activeSceneName);
            paused = true;
            mainPanel.gameObject.SetActive(true);
        }
        else
        {
            storePanel.gameObject.SetActive(true);
        }

        SetPlayerSkin(GameState.skinName);

    }

    private void Start()
    {
    }

    public void SetPlayerSkin(string skinName)
    {
        if(!String.IsNullOrEmpty(skinName))
        {
            // destroy any other body in case they were already instantiated
            Transform[] currBodies = playerController.bodyAnchor.transform.GetComponentsInChildren<Transform>();
            foreach(var currBody in currBodies)
            {
                if(currBody.gameObject != playerController.bodyAnchor.gameObject)
                {
                    Destroy(currBody.gameObject);
                }
            }

            GameObject skinObj = skinDict[skinName];

            // append selected body to body anchor position
            GameObject bodyClone = Instantiate(skinObj);
            bodyClone.transform.parent = playerController.bodyAnchor;
            bodyClone.transform.localPosition = Vector3.zero;
        }
    }


    public void EndLevel(int coinsEarned)
    {
        if (levelComplete == false) {
            SetCoins(GameState.numCoins + coinsEarned);
            levelStatusPanel.gameObject.SetActive(true);
            levelStatusPanel.SetEarnedCoins(coinsEarned);
            levelStatusPanel.SetLevel(GameState.currLevelNum);
            levelComplete = true;
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadShopLevel()
    {
        SceneManager.LoadScene("Shop");
    }
    public void NextLevel()
    {
        if(GameState.currLevelNum < SceneManager.sceneCountInBuildSettings)
        {
            GameState.currLevelNum += 1;
            levelStatusPanel.gameObject.SetActive(false);
            SceneManager.LoadScene(LEVEL + GameState.currLevelNum);
        }
    }

    public void LoadCurrentLevel()
    {
        levelStatusPanel.gameObject.SetActive(false);
        SceneManager.LoadScene(LEVEL + GameState.currLevelNum);
    }

    public void SetPaused(bool pause)
    {
        paused = pause;
    }

    public void OpenSettings()
    {
        settingsPanel.gameObject.SetActive(true);
    }

    public void SetCoins(int newCoins)
    {
        GameState.numCoins = newCoins;
        SetHudCoins(GameState.numCoins);
    }

    public void UnlockSkin(Skin currSkin)
    {
        GameState.skinsUnlocked.Add(currSkin.skinName);
        int newCoins = GameState.numCoins - currSkin.cost;
        SetCoins(newCoins);

    }
    public void SetHudCoins(int newCoins) 
    {
        hud.SetCoins(newCoins);
    }

    public void DetachPlayer()
    {
        playerController.Detach();
    }

    #region Debug
    public void Debug_DisplayLaunchPower(float launchValue)
    {
        debugPanel.DisplayLaunchPercentage(launchValue);
    }

    #endregion
}
