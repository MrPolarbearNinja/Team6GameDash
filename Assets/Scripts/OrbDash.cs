using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGDDPlatformer;

public class OrbDash : MonoBehaviour
{

    PlayerController player;
    public float dashSpeed = 2;
    bool isDashing = false;
    DashGem dashGem;

    public void Start()
    {
        player = GameManager.instance.players[0];
        dashGem = transform.GetChild(0).gameObject.GetComponent<DashGem>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject && player.canDash && !isDashing && dashGem.isActive)
        {
            player.inOrbRange = true;
            player.dashDirection = (transform.position - player.transform.position).normalized;
            player.dashSpeed = dashSpeed * player.dashSpeedOriginal;
        }
        if (player.isDashing)
            isDashing = true;
        if (player.isGrounded)
            isDashing = false;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        isDashing = false;
        player.inOrbRange = false;
    }

}
