using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin: MonoBehaviour
{
    public string skinName;
    public int cost = 50;
    public GameObject skinModel;

    private void Awake()
    {
        skinName = skinModel.name;
    }
}
