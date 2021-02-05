using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    // Start is called before the first frame update
    public string levelName = "LevelName";
    public string sceneName = "Level 1";
    public bool isCompleted = false;
    TMP_Text levelButtonText;
    Button button;
    public Image lockedImage;

    private void Start()
    {
        levelButtonText = GetComponentInChildren<TMP_Text>();
        button = GetComponent<Button>();
        levelButtonText.text = levelName;
        if (isCompleted)
        {
            lockedImage.enabled = false;
            button.interactable = true;
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
