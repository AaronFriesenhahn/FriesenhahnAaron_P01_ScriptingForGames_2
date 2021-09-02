using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //object camera will follow
    public GameObject playerToFollow;
    //offset to save, used for camera readjustment
    Vector3 cameraOffset;

    private void Start()
    {
        //calculate offset
        cameraOffset = transform.position - playerToFollow.transform.position;
    }

    private void LateUpdate()
    {
        //readjust camera position based off of player + offset position
        transform.position = playerToFollow.transform.position + cameraOffset;
    }
}
