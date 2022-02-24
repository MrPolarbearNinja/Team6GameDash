using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGDDPlatformer;

public class MoonDash : MonoBehaviour
{
    PlayerController player;
    public float dashSpeed = 2;
    bool isDashing = false;
    public bool isLeft;
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

        if (isDashing)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= 0.4)
            {
                Debug.Log("Yes");
                if (!isLeft)
                    player.dashDirection = new Vector2(1, 0);
                else
                    player.dashDirection = new Vector2(-1, 0);
                player.dash(player.dashDirection);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isDashing = false;
        player.inOrbRange = false;
    }
}
