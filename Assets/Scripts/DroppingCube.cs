using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingCube : MonoBehaviour
{
    GameManager gameManager;
    Animator animator;
    float startHoldTime;
    float startShakingTime;
    public float holdingTime = 2f;
    public float shakingTime = 2f;
    public float shakeAcceleration = 5f;
    public GameObject rootObject;
    bool shaking = false;
    bool holding = false;
    bool playerAttached = false;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponentInParent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (holding)
        {
            TimeHold();
        }else if (shaking)
        {
            TimeShake();
        }

        
    }

    void TimeHold()
    {
        if(Time.time - startHoldTime > holdingTime)
        {
            holding = false;
            shaking = true;
            startShakingTime = Time.time;
            animator.SetTrigger("Shake"); 
        }
    }
    void TimeShake()
    {
        animator.speed += shakeAcceleration * Time.deltaTime;
        if(Time.time - startShakingTime > shakingTime)
        {
            //TODO: shatter gameobject 
            if (IsPlayerAttached())
            {
                gameManager.DetachPlayer();
            }
            Destroy(rootObject);
        }
    }

    private bool IsPlayerAttached()
    {
        bool isPlayerAttached = false;
        PlayerController player = transform.parent.GetComponentInChildren<PlayerController>();
        if(player != null)
        {
            isPlayerAttached = true;
        }

        return isPlayerAttached;
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Poker"))
        {
            playerAttached = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Poker") && !shaking && !holding)
        {
            playerAttached = true;
            holding = true;
            startHoldTime = Time.time;
        }
    }
}
