using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public GameObject darkenedScreen;
    public GameObject deadScreen;
    public GameObject pausePanel;
    public CharacterController2D controller;
    public PowerBar powerBar;
    public Animator animator;
    public Animator pauseAnimator;
    public GameObject arrowSprite;
    public GameObject powerBarObj;
    public bool hasLaunched = false;
    public bool isDead = false;
    public bool isCharging = false;
    public bool isCooldown = false;
    public bool isPaused = false;
    public float runSpeed;
    public float maxLaunch;
    public float camOffset;

    private Image darkImg;
    private float inputX = 0f;
    private float launchPower = 0f;
    private float cooldownTimer = 0f;
    private bool canMove = true;
    private bool simMove = true;
    private bool canLaunch = true;
    private bool doJump = false;
    private bool doLaunch = false;
    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        darkImg = darkenedScreen.GetComponent<Image>();
    }

    // Update is called once a frame
    void Update()
    {   
        Vector2 mouseFacePoint = mainCam.ScreenToWorldPoint(new Vector2(Mouse.current.position.x.ReadValue(), 0));

        // Tick Timer For Power If Charging
        if (isCharging)
        {
            powerBarObj.SetActive(true);
            controller.FaceMouse(mouseFacePoint.x);
            launchPower += 0.33f * Time.deltaTime;
            powerBar.SetPower(launchPower);
        }
        if (isCooldown)
        {
            canLaunch = false;
            cooldownTimer -= 0.5f * Time.deltaTime;
            powerBar.SetCooldown(cooldownTimer);
            if (cooldownTimer <= 0)
            {
                isCooldown = false;
                cooldownTimer = 0;
                canLaunch = true;
                powerBarObj.SetActive(false);
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(inputX));
        animator.SetFloat("VerticalSpeed", controller.GetVertSpeed());
        animator.SetBool("inAir", !controller.m_Grounded);
        animator.SetBool("isDead", isDead);
    }

    // FixedUpdate is called a set amount a second
    void FixedUpdate()
    {
        Vector3 launchToPoint;
        launchToPoint = mainCam.ScreenToWorldPoint(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        launchToPoint = launchToPoint + new Vector3(0, 0, -camOffset);

        // Use CharController Script to Move If Enabled
        if (canMove)
        {
            controller.Move(inputX * Time.fixedDeltaTime, false, doJump);
        }
        else if (simMove)
        {
            controller.Move(0, false, false);
        }

        // Launch If Called For
        if (doLaunch)
        {
            powerBar.SetPower(0);
            hasLaunched = true;
            controller.m_AirControl = false;
            controller.LaunchTowards(launchToPoint, launchPower * maxLaunch);
            cooldownTimer = launchPower;
            isCooldown = true;
            launchPower = 0f;
            canMove = true;
            StartCoroutine(CheckForLaunch());
        }


        // Move Arrow In Circle
        Vector3 mousePos = new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos) - new Vector3(0, 0, camOffset);
        mousePos = transform.InverseTransformPoint(mousePos);
        mousePos = mousePos.normalized;
        arrowSprite.transform.localPosition = mousePos;

        // Rotate Arrow To Mouse
        Vector3 mousePosRot = new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0);
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePosRot.x = mousePosRot.x - objectPos.x;
        mousePosRot.y = mousePosRot.y - objectPos.y;
        float angle = Mathf.Atan2(mousePosRot.y, mousePosRot.x) * Mathf.Rad2Deg;
        arrowSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (!controller.m_FacingRight)
        {
            arrowSprite.transform.localScale = new Vector3(-0.15f, 0.15f, 1);
        }
        else
        {
            arrowSprite.transform.localScale = new Vector3(0.15f, 0.15f, 1);
        }

        if (transform.position.y < -20 && !isDead)
        {
            KillPlayer();
        }

        // Reset Calls
        doJump = false;
        doLaunch = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Death")
        {
            KillPlayer();
        }
    }

    // Take Movement Input From New Input System
    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<float>() * runSpeed;
    }

    public void Launch(InputAction.CallbackContext context)
    {
        if (canLaunch)
        {
            if (context.started && !EventSystem.current.IsPointerOverGameObject())
            {
                arrowSprite.SetActive(true);
                isCharging = true;
                canMove = false;
            }
            else if (context.performed)
            {
                arrowSprite.SetActive(false);
                isCharging = false;
                doLaunch = true;
            }
            else if (context.canceled)
            {
                arrowSprite.SetActive(false);
                isCharging = false;
                doLaunch = true;
            }
        }
    }

    // Called when ground is hit, Event is based off of CharController Script
    public void onGrounded()
    {
        hasLaunched = false;
        controller.m_AirControl = true;
    }

    public void Jump()
    {
        doJump = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;

    }

    public void Pause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            darkenedScreen.SetActive(true);
            pauseAnimator.SetBool("isOpen", true);
        }
        else
        {
            pauseAnimator.SetBool("isOpen", false);
            StartCoroutine("Resume");
        }
    }

    IEnumerator Resume()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
        darkenedScreen.SetActive(false);
        yield return null;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("StartMenu");
        Time.timeScale = 1f;
    }

    public void KillPlayer()
    {
        // Set Variables
        isDead = true;
        canLaunch = false;
        canMove = false;
        simMove = false;

        // Animate Death
        controller.TiltChar();

        deadScreen.SetActive(true);
    }

    IEnumerator CheckForLaunch()
    {
        yield return new WaitForFixedUpdate();
        hasLaunched = controller.MovingAcceptable();
        controller.m_AirControl = !controller.MovingAcceptable();
    }
}
