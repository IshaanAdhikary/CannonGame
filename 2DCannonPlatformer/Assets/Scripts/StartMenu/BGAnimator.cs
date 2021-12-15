using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAnimator : MonoBehaviour
{
    public Animator animator;
    public CharacterController2D controller;
    public float AnimDelay;

    private Rigidbody2D rb;
    private float actionRange = 4.5f;
    private float direction = 1;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Set Animation Parameters
    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("VerticalSpeed", controller.GetVertSpeed());
        animator.SetBool("inAir", !controller.m_Grounded);
        animator.SetBool("onSticky", true);
    }

    // Set a spark...
    void Start()
    {
        PlayAnim();
    }

    // ...turn into a flame.
    private void PlayAnim()
    {
        int whichCoroutine = Random.Range(1, 3);

        switch (whichCoroutine)
        {
            case 1:
                StartCoroutine("WalkAnim");
                break;
            case 2:
                StartCoroutine("LaunchAnim");
                break;
        }
    }

    IEnumerator WalkAnim()
    {
        // Set variables and random generation
        float delay = Random.Range(1, AnimDelay);
        float jumpAt = Random.Range(-actionRange, actionRange);
        bool shouldJump = Random.value > 0.3f;

        rb.velocity = Vector3.zero;
        transform.position = new Vector3(-7.5f * direction, -2.13f, 0);

        yield return new WaitForSeconds(delay);

        // Loop movement action based off random generation while on screen
        while (Mathf.Abs(rb.transform.position.x) < 9)
        {
            bool doJump = false;
            if (direction > 0 && rb.transform.position.x > jumpAt && shouldJump) { doJump = true; jumpAt = 1000; }
            else if (direction < 0 && rb.transform.position.x < jumpAt && shouldJump) { doJump = true; jumpAt = -1000; }

            controller.Move(60 * direction * Time.fixedDeltaTime, doJump, false);
            yield return new WaitForFixedUpdate();
        }
        direction = direction * -1;
        PlayAnim();
        yield return null;
    }

    IEnumerator LaunchAnim()
    {
        // Set variables and random generation
        float delay = Random.Range(1, AnimDelay);
        float launchAt = Random.Range(-actionRange, actionRange);
        float launchAfter = Random.Range(1, 3);
        float launchToY = Random.Range(7, 30);
        bool doMove = Random.value > 0.3f;

        rb.velocity = Vector3.zero;
        transform.position = new Vector3(-7.5f * direction, -2.13f, 0);

        yield return new WaitForSeconds(delay);

        while (Mathf.Abs(rb.transform.position.x) < 9)
        {
            bool doLaunch = false;

            if (doMove)
            {
                if (direction > 0 && rb.transform.position.x > launchAt) { doLaunch = true; launchAt = 1000; }
                else if (direction < 0 && rb.transform.position.x < launchAt) { doLaunch = true; launchAt = -1000; }
                controller.Move(60 * direction * Time.fixedDeltaTime, false, false);
            }
            else
            {
                yield return new WaitForSeconds(launchAfter);
                doLaunch = true;
            }

            if (doLaunch)
            {
                controller.FaceMouse(9 * direction);
                controller.LaunchTowards(new Vector3(9 * direction, launchToY, 0), 1000);
            }

            yield return new WaitForFixedUpdate();
        }
        direction = direction * -1;
        PlayAnim();
        yield return null;
    }
}
