using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    bool soundOn = true;
    bool vibrationOn = true;
    AudioListener audioListenter;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        audioListenter = Camera.main.GetComponent<AudioListener>();
        gameManager = FindObjectOfType<GameManager>();
    }
    public void Close()
    {
        gameObject.SetActive(false);
        gameManager.SetPaused(false);
    }
    public void ToggleSound()
    {
        soundOn = !soundOn;
        if (soundOn)
        {
            audioListenter.enabled = true; 
        }
        else
        {
            audioListenter.enabled = false; 
        }
    }
    public void ToggleVibration()
    {

    }
}
