using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSmoothing;

    private Transform mainCam;
    private Transform player;

    private Vector3 curentVelocity;

    void Start()
    {
        mainCam = Camera.main.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        curentVelocity = Vector3.zero;

        mainCam.position = new Vector3(player.position.x, player.position.y, mainCam.position.z);
    }
    
    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(player.position.x, player.position.y, mainCam.position.z);

        mainCam.position = Vector3.SmoothDamp(mainCam.position, newPos, ref curentVelocity, cameraSmoothing);
    }
}
