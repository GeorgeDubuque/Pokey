using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioSource coinSound;
    public MeshRenderer meshRender;
    public Collider coll;
    // Start is called before the first frame update
    void Start()
    {
        meshRender = GetComponent<MeshRenderer>();
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collect()
    {
        coinSound.Play();
        meshRender.enabled = false;
        coll.enabled = false;
        Destroy(gameObject, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
