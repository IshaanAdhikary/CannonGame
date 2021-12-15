using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	public bool m_AirControl = true;											// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_PlayerPos;                             // A position marking where the player is

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.

	private PlayerMovement player;
	private Rigidbody2D m_Rigidbody2D;
	private Vector3 prevVelocity = Vector3.zero;
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		player = GetComponent<PlayerMovement>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		LayerMask WhatIsGroundNow = m_WhatIsGround;

		// If player has launched, bouncy no longer is ground.
		if (player)
        {
			if (player.hasLaunched) { WhatIsGroundNow = WhatIsGroundNow ^ (1 << LayerMask.NameToLayer("Bouncy")); }
		}

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, WhatIsGroundNow);

		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		if (player)
        {
			if (!player.hasLaunched) { m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y, -30, 30)); }
		}
	}


	public void Move(float move, bool jump, bool onIce)
	{
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			if (onIce && move == 0) { targetVelocity.x = prevVelocity.x * 0.94f; }
			else if (onIce) { targetVelocity.x = targetVelocity.x * 0.17f + prevVelocity.x * 0.83f; }

			// And then smoothing it out and applying it to the character}
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}

			prevVelocity = m_Rigidbody2D.velocity;
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	public void LaunchTowards(Vector3 launch, float magnitude)
    {
		m_Rigidbody2D.AddForce((launch - m_PlayerPos.position).normalized * magnitude);
	}

	public void StopJitter()
    {
		m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
    }

	public void FaceMouse(float mouseXPos)
    {
		if (mouseXPos < m_PlayerPos.position.x && m_FacingRight)
        {
			Flip();
        }
		else if (mouseXPos > m_PlayerPos.position.x && !m_FacingRight)
        {
			Flip();
        }
    }

	public void TiltChar()
    {
		foreach (Collider2D col in GetComponents<Collider2D>())
		{
			col.enabled = false;
		}

		m_Rigidbody2D.velocity = Vector2.zero;
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		m_Rigidbody2D.AddTorque(-20);
		m_Rigidbody2D.AddForce(new Vector2(125, 325));
	}

	public float GetVertSpeed()
    {
		return m_Rigidbody2D.velocity.y;
    }
	
	public bool MovingAcceptable()
    {
		float speed = m_Rigidbody2D.velocity.y;
		if (speed > 3.25f){ return true; }
        else { return false; }
    }

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
