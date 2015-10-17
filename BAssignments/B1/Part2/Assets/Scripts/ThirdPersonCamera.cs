using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    public float smooth = 3f;       // camera smoothing
    Transform cameraPos;            // camera position

    void Start()
    {
        cameraPos = GameObject.Find("CameraPos").transform;
    }

    void FixedUpdate()
    {
        // Linearly interpolate vectors
        transform.position = Vector3.Lerp(transform.position, cameraPos.position, Time.deltaTime * smooth);
        transform.forward = Vector3.Lerp(transform.forward, cameraPos.forward, Time.deltaTime * smooth);
    }
}