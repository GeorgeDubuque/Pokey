using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    PlayerController player;
    PlayerStats stats;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        stats = GetComponentInParent<PlayerStats>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        
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
        if (other.CompareTag("Finish") && !player.IsDetached() && !gameManager.paused)
        {
            //TODO: next level modal and show modal
            gameManager.SetPaused(true);
            gameManager.EndLevel(stats.numCoins);
        }
    }
}
