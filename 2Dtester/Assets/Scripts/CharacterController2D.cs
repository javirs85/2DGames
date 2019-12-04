using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_DashVeclocity = 10f;                       //Velocity during the dash
    [SerializeField] private float m_DashTime = 0.5f;                            //time during the dash
    public GameObject DashEffect;

    [SerializeField] private float m_KickBackForceWhenHit = 10f;                //Force of the kick back when WE land an attack
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private float m_JumpStopperForce = 40f;                    // Amount of force substracted from vertical force when release the jump button
    [SerializeField] private float m_QuickFall = 2f;                            // gravity multipliyer during fall to make asymetric jumping with quick fall
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
   


    public event EventHandler Landed;
    public Animator animator;
    public bool IsDashing = false;
    public ParticleSystem trail;
    public int MaxNumOfJumps = 2;

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool jumpStarted;
    private bool m_Grounded;            // Whether or not the player is grounded.
    private int CurrentNumOfJumps = 0;


    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    private AudioEffects AudioPlayer;
    
    

    public bool IsGrounded
    {
        get
        {
            return m_Grounded;
        }
    }

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;
    private bool isFreeFalling;

    private void Awake()
    {
        AudioPlayer = GetComponent<AudioEffects>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void Update()
    {
        if (m_Rigidbody2D.velocity.y < 0) // we are falling
        {
            m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (m_QuickFall - 1) * Time.deltaTime;
        }
        else if (m_Rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump") && !m_Grounded)
        {
            m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (m_JumpStopperForce - 1) * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    Land();
                }
            }
        }

        if (!m_Grounded && !jumpStarted && !isFreeFalling && m_Rigidbody2D.velocity.y < 0)
        {
            isFreeFalling = true;
            animator.SetTrigger("StartsFalling");
        }
    }



    public void Move(float move, bool crouch, bool jump)
    {
        if(IsDashing)
        {
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, new Vector2(m_DashVeclocity, 0), ref m_Velocity, m_MovementSmoothing);
            return;
        }
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
           
            // And then smoothing it out and applying it to the character
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
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            Jump();
        }
        else if(!m_Grounded && jump && CurrentNumOfJumps < MaxNumOfJumps)
        {
            Jump();
        }
    }

    private void Land()
    {
        CurrentNumOfJumps = 0;
        jumpStarted = false;
        isFreeFalling = false;
        animator.SetTrigger("Landed");
        Landed?.Invoke(this, EventArgs.Empty);
        OnLandEvent.Invoke();
        AudioPlayer.PlayEffect(AudioEffects.Sounds.land);
    }

    private void Jump()
    {
        CurrentNumOfJumps++;
        jumpStarted = true;
        // Add a vertical force to the player.
        m_Grounded = false;
        m_Rigidbody2D.velocity = new Vector2( m_Rigidbody2D.velocity.x , m_JumpForce / 100);
        animator.SetTrigger("StartJumping");
        AudioPlayer.PlayEffect(AudioEffects.Sounds.jump);
    }

    public void Dash()
    {
        //DashEffect.SetActive(true);
        trail.Play();
        IsDashing = true;
        m_Rigidbody2D.gravityScale = 0;
        m_Rigidbody2D.velocity = new Vector2(m_DashVeclocity, 0);
        animator.SetBool("IsDashing", true);
        Invoke("FinishDashing", m_DashTime);
    }

    void FinishDashing()
    {
        m_Rigidbody2D.gravityScale = 1;
        m_Rigidbody2D.velocity = Vector2.zero;
        //DashEffect.SetActive(false);
        animator.SetBool("IsDashing", false);
        trail.Stop();
        IsDashing = false;
    }

    internal void KickBack()
    {
        var kickVector = Vector2.left;
        if (!m_FacingRight)
            kickVector = Vector2.right;

        m_Rigidbody2D.AddForce(kickVector * m_KickBackForceWhenHit, ForceMode2D.Impulse);

    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        m_DashVeclocity = -m_DashVeclocity;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}