using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceScript : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Simulated"))
        {
            if (col.otherCollider)
            {
                col.otherCollider.sharedMaterial.friction = 0.0025f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Simulated"))
        {
            if (col.otherCollider)
            {
                col.otherCollider.sharedMaterial.friction = 0.4f;
            }
        }
    }
}
