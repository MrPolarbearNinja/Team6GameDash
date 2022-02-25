using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGDDPlatformer;

public class DiamondDash : MonoBehaviour
{
    PlayerController player;
    public float dashSpeed = 2;
    bool isDashing = false;
    Vector2 dirNum;
    DashGem dashGem;

    public GameObject arrowRight;
    public GameObject arrowLeft;
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
            player.dashDirection = ((transform.position - player.transform.position)*1000).normalized;
            dirNum = AngleDir(transform.forward, player.dashDirection, transform.up);
            player.dashSpeed = dashSpeed / 10 * player.dashSpeedOriginal;

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
                player.dashDirection = (dirNum.normalized);
                player.dash(player.dashDirection);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isDashing = false;
        player.inOrbRange = false;
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);
        playerArrow.SetActive(false);
    }

    Vector2 AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            arrowLeft.SetActive(false);
            arrowRight.SetActive(true);
            return new Vector2(1, 1.5f);
        }
        else if (dir < 0f)
        {
            arrowLeft.SetActive(true);
            arrowRight.SetActive(false);
            return new Vector2(-1, 1.5f);
        }
        else
        {
            return new Vector2(0, 0);
        }
    }
}
