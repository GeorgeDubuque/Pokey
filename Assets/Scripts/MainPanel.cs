using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform levelScrollViewContent;
    public GameObject settingsPanel;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        LoadLevels();
    }

    void LoadLevels()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        string currSceneName = "";

        for( int i = 0; i < sceneCount; i++ )
        {
            currSceneName = "Level " + (i + 1);
            GameObject currLevelButton = GameObject.Instantiate(levelButtonPrefab, levelScrollViewContent);
            var levelButton = currLevelButton.GetComponent<LevelButton>();

            if(i == 0 || GameState.levelsCompleted.Contains(currSceneName))
            {
                levelButton.isCompleted = true;
            }

            levelButton.levelName = currSceneName;
            levelButton.sceneName = currSceneName;
        }
    }

    public void Unpause()
    {
        gameObject.SetActive(false);
        gameManager.SetPaused(false);
    }

    public void LoadShopScene()
    {
        SceneManager.LoadScene("Shop");
    }

}
