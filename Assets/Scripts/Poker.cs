using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poker : MonoBehaviour
{
    PlayerController player;
    public AudioSource stabSound;
    public AudioSource balloonPop;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>(); 
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    GameObject other = collision.gameObject;
    //    if (other.CompareTag("Stickable"))
    //    {
    //        player.FreezePlayer(other.transform);
    //        stabSound.Play();
    //    }else if (other.CompareTag("Balloon"))
    //    {
    //        Balloon balloon = other.GetComponentInParent<Balloon>();
    //        balloonPop.Play();
    //        balloon.Pop();
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stickable"))
        {
            player.FreezePlayer(other.transform); 
            stabSound.Play();
        }else if (other.CompareTag("Balloon"))
        {
            Balloon balloon = other.GetComponentInParent<Balloon>();
            balloonPop.Play();
            balloon.Pop();
        }
    }
}
