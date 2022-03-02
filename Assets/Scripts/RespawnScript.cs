using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGDDPlatformer;

public class RespawnScript : MonoBehaviour
{
    
    private GameObject player;
    public GameObject playerSprite;
    public PlayerController playerController;
    private Vector3 playerSpawnPoint;
    void Start()
    {
        player = GameManager.instance.players[0].gameObject;
        playerSpawnPoint = player.transform.position;
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (this.tag == "Respawn" && other.gameObject.layer == 8)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        // Disable player movement and sprite on death
        //playerController.enabled = false;
        Rigidbody2D rigi = playerController.gameObject.GetComponent<Rigidbody2D>();
        rigi.simulated = false;
        playerSprite.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        playerController.isDead = true;

        yield return new WaitForSeconds(0.5f);

        // Reset the position of the player to the original position at the start of the level
        playerController.ResetPlayer();
        rigi.simulated = true;
        playerController.isDead = false;
        // Enable player movement and sprite on death
        //playerController.enabled = true;
        playerSprite.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
