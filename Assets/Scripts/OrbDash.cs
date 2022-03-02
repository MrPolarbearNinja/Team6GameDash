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
    public GameObject arrows;

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
            player.dashDirection.Normalize();
            player.dashSpeed = dashSpeed/10 * player.dashSpeedOriginal;

            arrows.SetActive(true);
            float angle = Mathf.Atan2(player.transform.position.y - arrows.transform.position.y,
                                       player.transform.position.x - arrows.transform.position.x) * Mathf.Rad2Deg;
            arrows.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

            

        }
        if (player.isDashing)
            isDashing = true;
        if (player.isGrounded)
            isDashing = false;

        dashGem.canPickUp = isDashing;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        arrows.SetActive(false);
        isDashing = false;
        player.inOrbRange = false;
    }

}
