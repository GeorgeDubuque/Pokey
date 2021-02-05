using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private void Start()
    {
    }
    public void Pop()
    {
        Destroy(gameObject);
    }
}
