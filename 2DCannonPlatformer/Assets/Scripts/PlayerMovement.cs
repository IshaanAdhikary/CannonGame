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
    public GameObject arrowSprite;
    public GameObject powerBarObj;
    public Animator animator;
    public Animator pauseAnimator;
    public bool hasLaunched = false;
    public bool isDead = false;
    public bool isCharging = false;
    public bool isCooldown = false;
    public bool isPaused = false;
    public float runSpeed;
    public float maxLaunch;
    public float camOffset;

    private CharacterController2D controller;
    private PowerBar powerBar;
    private float inputX = 0f;
    private float launchPower = 0f;
    private float cooldownTimer = 0f;
    private bool canLaunch = true;
    private bool doJump = false;
    private bool onIce = false;
    private Vector3 launchToPoint;
    private Camera mainCam;

    // Start is called before the first frame update
    void Awake()
    {
        // Get References
        controller = GetComponent<CharacterController2D>();
        powerBar = powerBarObj.GetComponent<PowerBar>();
        mainCam = Camera.main;
        powerBar.childrenImg = powerBarObj.GetComponentsInChildren<Image>();
    }

    // Update is called once a frame
    void Update()
    {
        Vector2 mouseFacePoint = mainCam.ScreenToWorldPoint(new Vector2(Mouse.current.position.x.ReadValue(), 0));

        // Tick Timer For Power If Charging
        if (isCharging)
        {
            arrowSprite.SetActive(true);
            powerBarObj.SetActive(true);
            controller.FaceMouse(mouseFacePoint.x);
            launchPower += 0.33f * Time.deltaTime;
            powerBar.SetPower(launchPower);
        }
        else if (isCooldown)
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

        // Set Animation Parameters
        animator.SetFloat("Speed", Mathf.Abs(inputX));
        animator.SetFloat("VerticalSpeed", controller.GetVertSpeed());
        animator.SetBool("inAir", !controller.m_Grounded);
        animator.SetBool("isDead", isDead);
        animator.SetBool("isCharging", isCharging);
    }

    // FixedUpdate is called a set amount a second
    void FixedUpdate()
    {
        // Get Mouse Position
        launchToPoint = mainCam.ScreenToWorldPoint(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        launchToPoint = launchToPoint + new Vector3(0, 0, -camOffset);

        // Check if the player should control the character
        bool canMove = !isDead && !isCharging;
        bool simMove = !isDead;

        // Use CharController Script to Move If Enabled
        if (canMove) { controller.Move(inputX * Time.fixedDeltaTime, doJump, onIce); }
        else if (simMove) { controller.Move(0, false, onIce); }

        // Move Arrow, then flip if character is facing other way
        MoveArrow();
        if (!controller.m_FacingRight) { arrowSprite.transform.localScale = new Vector3(-0.15f, 0.15f, 1); }
        else { arrowSprite.transform.localScale = new Vector3(0.15f, 0.15f, 1); }

        // Kill if under y = -20
        if (transform.position.y < -20 && !isDead) { KillPlayer(); }

        // Reset Variables
        doJump = false;
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        // Kill player if they touch Death
        if (col.gameObject.CompareTag("Death")) { KillPlayer(); }

        // Icy movement enabled if they touch Icy
        if (col.gameObject.CompareTag("Icy")) { onIce = true; }
        else { onIce = false; }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // If the player is barely bouncing, stop bouncing.
        if (col.gameObject.CompareTag("Bouncy") && controller.GetVertSpeed() < 4.2f)
        {
            controller.StopJitter();
            hasLaunched = false;
        }
    }

    // Called when ground is hit, Event is based off of CharController Script
    public void onGrounded()
    {
        hasLaunched = false;
        controller.m_AirControl = true;
    }

    // Take Movement Input From New Input System
    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<float>() * runSpeed;
    }

    // Take Launch Input from New Input System
    public void Launch(InputAction.CallbackContext context)
    {
        if (canLaunch && controller.m_Grounded)
        {
            if (context.started && !EventSystem.current.IsPointerOverGameObject())
            {
                isCharging = true;
            }
            else if (context.performed)
            {
                LaunchPlayer(launchToPoint);
            }
            else if (context.canceled)
            {
                LaunchPlayer(launchToPoint);
            }
        }
    }

    // Get Jump Input from New Input System
    public void Jump()
    {
        doJump = true;
    }

    // Get Restart Input from New Input System
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    // Gets Pause Input from New Input System
    public void Pause()
    {
        isPaused = !isPaused;
        if (isPaused) { DoPause(); }
        else { StartCoroutine(Resume()); }
    }

    // Pause Game
    private void DoPause()
    {
        Time.timeScale = 0f;
        darkenedScreen.SetActive(true);
        pauseAnimator.SetBool("isOpen", true);
    }

    // Resume Game
    IEnumerator Resume()
    {
        pauseAnimator.SetBool("isOpen", false);
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
        darkenedScreen.SetActive(false);
        yield return null;
    }

    // Go to Menu
    public void GoToMenu()
    {
        SceneManager.LoadScene("StartMenu");
        Time.timeScale = 1f;
    }

    // Kill the Player
    public void KillPlayer()
    {
        // Set Variables
        isDead = true;
        canLaunch = false;

        // Animate Death
        controller.TiltChar();
        deadScreen.SetActive(true);
    }

    // Launch the Player
    private void LaunchPlayer(Vector3 launchToPoint)
    {
        arrowSprite.SetActive(false);
        powerBar.SetPower(0f);
        isCharging = false;
        hasLaunched = true;
        controller.m_AirControl = false;
        cooldownTimer = launchPower;
        isCooldown = true;
        controller.LaunchTowards(launchToPoint, launchPower * maxLaunch);
        StartCoroutine(CheckForLaunch());
        launchPower = 0f;
    }

    // Move the Arrow
    private void MoveArrow()
    {
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
    }

    // Reset HasLaunched if character moves less than a threshold
    IEnumerator CheckForLaunch()
    {
        yield return new WaitForFixedUpdate();
        hasLaunched = controller.MovingAcceptable();
        controller.m_AirControl = !hasLaunched;
    }
}
