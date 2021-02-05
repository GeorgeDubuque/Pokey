using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform target;
    public float dstFromTarget = 2;
    public float verticalOffset = 2;
    public float positionSmoothTime = 1.2f;
    Vector3 positionSmoothVelocity;
    Vector3 targetPosition;

    void Start()
    {
    }

    void Update()
    {
        targetPosition = (target.position - transform.forward * dstFromTarget) + (transform.up * verticalOffset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref positionSmoothVelocity, positionSmoothTime);
    }
}
