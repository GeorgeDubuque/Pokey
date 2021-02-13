using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poker : MonoBehaviour
{
    PlayerController player;
    DecalController decalController;
    public AudioSource stabSound;
    public AudioSource balloonPopSound;
    public AudioSource tingSound;
    public GameObject metalSparkParticle;
    public GameObject pokerParticle;
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
    //        balloonPopSound.Play();
    //        balloon.Pop();
    //    }
    //}
    private void Update()
    {
        Debug.DrawRay(player.transform.position, -player.transform.forward * RETICLE_DISTANCE_THRESH, Color.red);
    }
    private void PlayParticle(RaycastHit hit, GameObject particle)
    {
        GameObject pokerSpark = GameObject.Instantiate(particle, hit.transform);
        pokerSpark.transform.position = hit.point;
        pokerSpark.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        pokerSpark.GetComponent<ParticleSystem>().Play();
        Destroy(pokerSpark, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        RaycastHit hit;
        int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

        bool reticleHit = 
            Physics.Raycast(player.transform.position, -player.transform.forward, out hit, RETICLE_DISTANCE_THRESH, layerMask);

        if (other.CompareTag("Stickable"))
        {
            if (reticleHit)
            {
                decalController.SpawnDecal(hit);
            }
            PlayParticle(hit, pokerParticle);
            player.FreezePlayer(other.transform); 
            stabSound.Play();
        }else if (other.CompareTag("Balloon"))
        {
            Balloon balloon = other.GetComponentInParent<Balloon>();
            balloonPopSound.Play();
            balloon.Pop();
        }else if (other.CompareTag("Metal"))
        {
            tingSound.Play();
            PlayParticle(hit, metalSparkParticle);
            PlayParticle(hit, pokerParticle);
        }

    }
}
