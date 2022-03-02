using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakablePlatformBehaviour : MonoBehaviour
{
    public TilemapCollider2D colliderObj;
    public TilemapRenderer rendererObj;
    public Tilemap tilemap;
    
    private bool _isTouchingPlatform = false;
    private float _timeToBreak = 1.5f; // how long until the platform breaks
    private float _timeToRespawn = 2f;
    private float _colorPercentage;

    private void Start()
    {
        _colorPercentage = _timeToBreak / 5;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _isTouchingPlatform = true;
        Invoke(nameof(StillOnPlatformCheck), _timeToBreak);
        gradualColorChange();
    }

    private void gradualColorChange()
    {
        if (_isTouchingPlatform && rendererObj.enabled)
        {
            tilemap.color = Color.Lerp(Color.white, Color.red, _colorPercentage);
            _colorPercentage += _timeToBreak / 5f;
            Invoke(nameof(gradualColorChange), _timeToBreak/5f);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _isTouchingPlatform = false;
        tilemap.color = Color.white;
        _colorPercentage = 0.25f;
        CancelInvoke();
    }

    private void StillOnPlatformCheck()
    {
        if (_isTouchingPlatform)
        {
            DeSpawnPlatform();
            Invoke(nameof(SpawnPlatform), _timeToRespawn);
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
