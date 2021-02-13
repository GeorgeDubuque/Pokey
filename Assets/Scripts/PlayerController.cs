using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float RETICLE_DISTANCE_THRESH = 10f;
    const float POKER_DISTANCE_THRESH = 10f;
    const int POKE_FRAME_COUNT = 3;

    Rigidbody rb;
    Camera cam;
    Animator anim;
    public Animator ballAnim;
    PlayerStats stats;
    PlayerSounds sounds;
    DecalController decalController;
    public GameObject puncturedObject;
    public GameManager gameManager;
    public GameObject reticle;
    public Vector3 puncturePos = Vector3.zero;
    public Transform pokerTip;
    public GameObject testPanel;
    public Transform bodyAnchor;
    public GameObject body;
    public float maxSpeed = 20f;

    public Material bodyMat;

    Vector3 startPos;
    Vector3 endPos;
    public Transform spawn;

    bool startedLaunch = false;
    bool detached = true;
    bool startHold = false;
    bool endHold = false;


    public float shakeThreshold = 0.95f;
    public float bendRate = 3f;
    public float shakeAmount = .1f;
    float bendPercent = 0f;
    float targetBendPercent = 0f;
    bool bending = false;

    void Start()
    {
        // getters for global objects
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
        sounds = GetComponent<PlayerSounds>();
        decalController = GetComponent<DecalController>();
        gameManager = FindObjectOfType<GameManager>();

        transform.position = spawn.position;
        stats.numCoins = 0;
    }


    void Update()
    {
        //Debug.DrawRay(transform.position, -Vector3.forward * 10, Color.green);
        //Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
        //Debug.Log("transform.right: " + transform.right);
        if (!gameManager.paused)
        {
            CastReticle();
            CheckDeath();
            PlayerInput();

            if (detached)
            {
                if (startHold)
                {
                    Poke();
                }
            }
            else
            {
                //Debug.Log("1: " + transform.rotation.eulerAngles.y);
                //transform.up = Vector3.down;
                //Debug.Log("2: " + transform.rotation.eulerAngles.y);
                if (startHold)
                {
                    startPos = Input.mousePosition;
                    startedLaunch = true;
                }

                if (endHold && startedLaunch)
                {
                    endPos = Input.mousePosition;
                    Vector3 dir = endPos - startPos;
                    startedLaunch = false;

                    Launch(dir); 
                }
            }
            if (startedLaunch)
            {
                Bend();
            }
            else
            {
                bending = false;
                bendPercent = 0f;
                anim.SetFloat("BendVertical", 0f);
                ballAnim.SetFloat("BendPercentage", 0f);
                anim.SetBool("Bending", false);
            }
            anim.SetBool("IsDetached", detached);
        }
    }
    public void Bend()
    {
        // getting drag direction and magnitude
        Vector3 dir = Input.mousePosition - startPos;
        dir = 
            new Vector3(dir.x/(Screen.width/stats.dragFactor), dir.y/(Screen.height/stats.dragFactor), 0);
        if(dir.magnitude > 1)
        {
            dir = dir.normalized;
        }
        dir = dir * stats.maxLaunchPower;

        float rotation = Vector3.SignedAngle(Vector3.down, dir, Vector3.forward);
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, -rotation));

        float bendAmount = (dir.magnitude / stats.maxLaunchPower);

        bendPercent = dir.magnitude / stats.maxLaunchPower;
        Debug.DrawRay(transform.position, dir * 5 * bendPercent, Color.green);
        #region shake using bend anim as opposed to anim
        /* Uncomment this section to shake using bend percentage instead of animation */
        //if (!bending)
        //{
        //    targetBendPercent = bendAmount - shakeAmount;
        //}

        //if(bendAmount > shakeThreshold)
        //{
        //    if(bendPercent > targetBendPercent)
        //    {
        //        bendPercent -= bendRate * Time.deltaTime;
        //        targetBendPercent = bendAmount - shakeAmount;
        //    }
        //    else
        //    {
        //        bendPercent += bendRate * Time.deltaTime;
        //        targetBendPercent = bendAmount;
        //    }
        //}
        //else
        //{
        //    bendPercent = bendAmount;
        //}
        #endregion

        bending = true;
        anim.SetBool("Bending", bending);
        anim.SetFloat("BendVertical", bendPercent);
        ballAnim.SetFloat("BendPercentage", bendPercent);
        gameManager.Debug_DisplayLaunchPower(bendPercent);
    }
    public void Launch(Vector3 dir)
    {
        Detach();
        anim.SetTrigger("Launch");
        dir = new Vector3(
            dir.x/(Screen.width/stats.dragFactor), 
            dir.y/(Screen.height/stats.dragFactor), 0);
        if(dir.magnitude > 1)
        {
            dir = dir.normalized;
        }

        gameManager.Debug_DisplayLaunchPower(dir.magnitude);

        rb.AddForce(-dir * stats.maxLaunchPower, ForceMode.VelocityChange);
    }
    public void Detach()
    {
        transform.parent = null;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        detached = true;
    }
    private void FixedUpdate()
    {
        if(rb.velocity.magnitude > stats.maxLaunchPower)
        {
            rb.velocity = rb.velocity.normalized * stats.maxLaunchPower;
        }
        //if(rb.velocity.y < 0)
        //{
        //    rb.velocity += Physics.gravity * stats.fallMultiplier * Time.fixedDeltaTime;
        //}
    }
    void PlayerInput()
    {
        startHold = false;
        endHold = false;
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount >= 1)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    startHold = true;
                }

                if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    endHold = true;
                }
            }
        }
        else
        {
            startHold = Input.GetButtonDown("Mouse1");
            endHold = Input.GetButtonUp("Mouse1");
        }
    }

    void CastPoker()
    {
        RaycastHit hit;
        int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

        bool pokerHit = 
            Physics.Raycast(transform.position, -transform.forward, out hit, POKER_DISTANCE_THRESH, layerMask);

        if (pokerHit && detached){
            if (hit.transform.CompareTag("Stickable"))
            {
                FreezePlayer(hit.transform);
                sounds.stabSound.Play();
            }else if (hit.transform.CompareTag("Balloon"))
            {
                Balloon balloon = hit.transform.GetComponentInParent<Balloon>();
                sounds.balloonPop.Play();
                balloon.Pop();
            }
        }
    }

    void CastReticle()
    {
        RaycastHit hit;
        int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

        bool reticleHit = 
            Physics.Raycast(transform.position, -transform.forward, out hit, RETICLE_DISTANCE_THRESH, layerMask);

        if (detached && reticleHit){
            reticle.transform.position = hit.point;
            reticle.SetActive(true);
        }
        else
        {
            reticle.SetActive(false);
        }
    }

    private void CheckDeath()
    {
        if(transform.position.y < 0)
        {
            gameManager.ReloadLevel();
        }
    }

    void Poke()
    {
        anim.SetTrigger("Poke");
    }

    public void FreezePlayer(Transform trans)
    {
        transform.parent = trans.parent;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.velocity = Vector3.zero;
        detached = false;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            other.GetComponent<Coin>().Collect();
            stats.numCoins += 1;
            gameManager.SetHudCoins(GameState.numCoins + stats.numCoins);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Finish") && !detached && !gameManager.paused)
        {
            //TODO: next level modal and show modal
            gameManager.SetPaused(true);
            gameManager.EndLevel(stats.numCoins);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Detach();
    }
}
