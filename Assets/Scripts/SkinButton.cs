using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkinButton : MonoBehaviour
{
    // Start is called before the first frame update
    StorePanel store;
    public Skin skin;
    public GameObject selectedBorder;
    public GameObject currentBorder;
    public GameObject lockedImage;
    public GameObject checkmarkImage;
    public Image backgroundColorImg;
    public bool isUnlocked = false;
    Button button;
    private void Awake()
    {
        skin = GetComponent<Skin>();
    }
    private void Start()
    {
        button = GetComponent<Button>();
        store = GetComponentInParent<StorePanel>();
    }

    public void Unlock()
    {
        lockedImage.SetActive(false);
        isUnlocked = true;
    }

    public void Click()
    {
        store.SelectSkin();
    }

    public void SelectSkin()
    {
        selectedBorder.SetActive(false);
        currentBorder.SetActive(true);

        checkmarkImage.SetActive(true);
        lockedImage.SetActive(false);
        
    }
    public void Deselect()
    {
        selectedBorder.SetActive(false);
        currentBorder.SetActive(false);
        checkmarkImage.SetActive(false);
    }

}
