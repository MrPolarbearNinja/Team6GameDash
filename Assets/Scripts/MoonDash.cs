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
    public GameObject arrowForward;
    public GameObject playerArrow;

    public void Start()
    {
        player = GameManager.instance.players[0];
        dashGem = transform.GetChild(0).gameObject.GetComponent<DashGem>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject && !isDashing && dashGem.isActive)
        {
            player.inOrbRange = true;
            player.dashDirection = (transform.position - player.transform.position).normalized;
            player.dashSpeed = dashSpeed / 10 * player.dashSpeedOriginal;

            arrowForward.SetActive(true);
            playerArrow.SetActive(true);
            float angle = Mathf.Atan2(player.transform.position.y - playerArrow.transform.position.y,
                                       player.transform.position.x - playerArrow.transform.position.x) * Mathf.Rad2Deg;
            playerArrow.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        }
        if (player.isDashing)
            isDashing = true;
        if (player.isGrounded)
            isDashing = false;

        if (isDashing)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= 0.4f)
            {
                player.transform.position = transform.position;
                player.velocity = Vector2.zero;
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
        arrowForward.SetActive(false);
        playerArrow.SetActive(false);
        isDashing = false;
        player.inOrbRange = false;
    }
}
