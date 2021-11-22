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
    private Camera cam;
    private Vector3 cameraOffset;
    private Vector3 moveVelocity = Vector3.zero;
    private float scaleVelocity = 0;
    private float newScale2D;

    void Start()
    {
        cam = Camera.main;
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

        Vector3 newPos2D = Vector3.SmoothDamp(transform.position - cameraOffset, targetPos, ref moveVelocity, cameraSnapSpeed);
        transform.position = newPos2D + cameraOffset;

        if (PlayerMovement.isCharging)
        {
            newScale2D = Mathf.SmoothDamp(cam.orthographicSize, 5f, ref scaleVelocity, 1);
        }
        else
        {
            newScale2D = Mathf.SmoothDamp(cam.orthographicSize, 4f, ref scaleVelocity, 0.4f);
        }

        cam.orthographicSize = newScale2D;

        // Move Background with Camera
        background.transform.position = transform.position + new Vector3(0, 0, -PlayerMovement.camOffset);
    }
}
