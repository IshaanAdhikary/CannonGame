using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject background;
    public float cameraSnapSpeed;

    private PlayerMovement PlayerMovement;
    private Vector3 cameraOffset;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        PlayerMovement = player.GetComponent<PlayerMovement>();
        cameraOffset = new Vector3(0, 0, PlayerMovement.camOffset);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos = player.transform.position;

        if (PlayerMovement.isDead)
        {
            targetPos = transform.position - cameraOffset;
        }

        Vector3 newPos2D = Vector3.SmoothDamp(transform.position - cameraOffset, targetPos, ref velocity, cameraSnapSpeed);
        transform.position = newPos2D + cameraOffset;

        // Move Background with Camera
        background.transform.position = transform.position + new Vector3(0, 0, -PlayerMovement.camOffset);
    }
}
