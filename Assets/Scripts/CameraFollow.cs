using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGDDPlatformer;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (!target)
            target = GameManager.instance.players[0].transform;
    }

    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 3, -10));

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
