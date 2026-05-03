using System;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public float cameraFollowSensitivity = 1000f;
    const float cameraFollowSpeed = 10f;
    
    public GameObject cameraShaker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 posXY = Vector2.Lerp(
            transform.position, 
            MouseHandler.Instance.mousePosFinal * (float)Math.Sqrt(MouseHandler.Instance.mousePosFinal.magnitude / cameraFollowSensitivity) + (Vector2) cameraShaker.transform.position,
            cameraFollowSpeed * Time.deltaTime
        );
        transform.position = new Vector3(posXY.x, posXY.y, transform.position.z);
    }
}
