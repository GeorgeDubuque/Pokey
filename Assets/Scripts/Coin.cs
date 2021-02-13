using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioSource coinSound;
    public GameObject graphics;
    public ParticleSystem coinParticle;
    Collider coll;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
    }

    public void Collect()
    {
        coinSound.Play();
        graphics.SetActive(false);
        coll.enabled = false;
        coinParticle.Play();
        Destroy(gameObject, 1f);
    }
}
