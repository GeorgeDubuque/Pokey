using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public float launchPower = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * launchPower, Color.red); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.position = transform.position + transform.forward;
            Rigidbody collidedRb = collision.gameObject.GetComponent<Rigidbody>();
            collidedRb.velocity = Vector3.zero;
            collidedRb.AddForce(transform.forward * launchPower, ForceMode.VelocityChange);
        }
    }
}
