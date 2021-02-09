using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poker : MonoBehaviour
{
    PlayerController player;
    DecalController decalController;
    public AudioSource stabSound;
    public AudioSource balloonPop;
    const float RETICLE_DISTANCE_THRESH = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        decalController = GetComponentInParent<DecalController>();
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
    private void Update()
    {
        Debug.DrawRay(player.transform.position, -player.transform.forward * RETICLE_DISTANCE_THRESH, Color.red);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stickable"))
        {
            RaycastHit hit;
            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

            bool reticleHit = 
                Physics.Raycast(player.transform.position, -player.transform.forward, out hit, RETICLE_DISTANCE_THRESH, layerMask);
            if (reticleHit)
            {
                decalController.SpawnDecal(hit);
            }
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
