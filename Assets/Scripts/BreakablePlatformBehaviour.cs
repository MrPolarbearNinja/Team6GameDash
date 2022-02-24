using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakablePlatformBehaviour : MonoBehaviour
{
    public TilemapCollider2D colliderObj;
    public TilemapRenderer rendererObj;
    private bool _isTouchingPlatform = false;
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("2denter");
        _isTouchingPlatform = true;
        Invoke(nameof(StillOnPlatformCheck), 1.5f);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _isTouchingPlatform = false;
    }

    private void StillOnPlatformCheck()
    {
        if (_isTouchingPlatform)
        {
            DeSpawnPlatform();
            Invoke(nameof(SpawnPlatform), 2f);
        }
    }


    private void DeSpawnPlatform()
    {
        colliderObj.enabled = false;
        rendererObj.enabled = false;
    }

    private void SpawnPlatform()
    {
        colliderObj.enabled = true;
        rendererObj.enabled = true;
    }
}
