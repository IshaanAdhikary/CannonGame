using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyScript : MonoBehaviour
{
    private Joint2D fixedJoint;
    private GameObject player;
    private PlayerMovement playerScript;

    // Awake is called before the first frame
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        fixedJoint = GetComponent<FixedJoint2D>();
    }

    private void FixedUpdate()
    {
        if (playerScript.hasLaunched)
        {
            fixedJoint.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            fixedJoint.enabled = true;
        }
    }
}
