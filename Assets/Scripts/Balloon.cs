using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public GameObject balloonPopParticle;
    private void PlayParticle(Vector3 point, GameObject particle)
    {
        GameObject pokerSpark = GameObject.Instantiate(particle, null);
        pokerSpark.transform.position = point;
        pokerSpark.GetComponent<ParticleSystem>().Play();
        Destroy(pokerSpark, 1f);
    }
    public void Pop()
    {
        PlayParticle(transform.position, balloonPopParticle);
        Destroy(gameObject);
    }
}
