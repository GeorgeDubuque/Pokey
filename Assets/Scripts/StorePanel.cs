using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorePanel : MonoBehaviour
{
    SkinButton[] skinButtons;
    SkinButton currButton;
    public GameObject selectButton;
    public GameObject unlockButton;
    public GameObject skinAnchor;
    public Skin currShownSkin;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        skinButtons = GetComponentsInChildren<SkinButton>();
        int i = 0;
        foreach(SkinButton currButt in skinButtons)
        {
            if(i == 0 || GameState.skinsUnlocked.Contains(currButt.skin.skinName))
            {
                currButt.Unlock();
            }
            
            if(currButt.skin.skinName == GameState.skinName)
            {
                SetSelectedSkinButton(currButt);
                OnClickSelectButton();
            }

            i++;
        }
    }

    public void SelectSkin()
    {
        GameObject selectedButtonObj = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        SkinButton selectedButton = selectedButtonObj.GetComponent<SkinButton>();
        gameManager.SetPlayerSkin(selectedButton.skin.skinName);
        //ShowPlayerSkin(selectedButton.skin.skinName);
        SetSelectedSkinButton(selectedButton);
    }

    public void SetSelectedSkinButton(SkinButton selectedButton)
    {
        foreach(SkinButton currButton in skinButtons)
        {
            currButton.selectedBorder.SetActive(false);
        }
        selectedButton.selectedBorder.SetActive(true);
        currButton = selectedButton;
        Debug.Log(selectedButton.skin.skinName);
        Debug.Log("IsUnlocked: " + currButton.isUnlocked);
        if (currButton.isUnlocked)
        {
            ToggleUnlockButton(false, selectedButton);
            selectButton.SetActive(true);
        }
        else
        {
            ToggleUnlockButton(true, selectedButton);
            selectButton.SetActive(false);
        }
    }

    void ToggleUnlockButton(bool activate, SkinButton selectedButton)
    {
        TMP_Text text = unlockButton.GetComponentInChildren<TMP_Text>();
        text.text = selectedButton.skin.cost.ToString();
        unlockButton.GetComponent<Button>().interactable = GameState.numCoins >= selectedButton.skin.cost;
        unlockButton.SetActive(activate);
    }

    public void OnClickSelectButton()
    {
        foreach(SkinButton currButt in skinButtons)
        {
            currButt.Deselect();
        }
        GameState.skinName = currButton.skin.skinName;
        currButton.SelectSkin();
    }

    public void ShowPlayerSkin(string skinName)
    {
        GameObject body = gameManager.skinDict[skinName];
        // set current selected model to body and reset postion
        if(body != null)
        {
            // destroy any other body in case they were already instantiated
            Transform[] currBodies = skinAnchor.transform.GetComponentsInChildren<Transform>();
            foreach(var go in currBodies)
            {
                if(go.gameObject != skinAnchor.gameObject)
                {
                    Destroy(go.gameObject);
                }
            }

            // append selected body to body anchor position
            GameObject bodyClone = Instantiate(body);
            bodyClone.transform.parent = skinAnchor.transform;
            bodyClone.transform.localPosition = Vector3.zero;
        }
    }


    public void TryUnlockSkin()
    {
        GameObject unlockButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Skin currSkin = currButton.skin;
        bool hasEnoughCoins = GameState.numCoins >= currSkin.cost;
        if (hasEnoughCoins)
        {
            gameManager.UnlockSkin(currSkin);
            currButton.Unlock();
            unlockButton.SetActive(false);
            selectButton.SetActive(true);
        }
    }
}

