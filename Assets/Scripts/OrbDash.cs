using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGDDPlatformer;

public class OrbDash : MonoBehaviour
{

    PlayerController player;
    public float dashSpeed = 2;
    public float travelDist = 10;

    public void Start()
    {
        player = GameManager.instance.players[0];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject && player.canDash)
        {
            player.inOrbRange = true;
            player.dashDirection = (transform.position - player.transform.position).normalized * (travelDist/10);
            player.dashSpeed = dashSpeed * player.dashSpeedOriginal;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        player.inOrbRange = false;
    }

}
