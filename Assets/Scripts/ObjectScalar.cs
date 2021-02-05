using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScalar : MonoBehaviour
{
    void Start()
    {
        Vector3 parentScale = gameObject.transform.localScale;
        transform.localScale = Vector3.one;
        transform.GetChild(0).localScale = parentScale;
    }
}
