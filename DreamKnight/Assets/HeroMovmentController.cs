using Assets.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovmentController : MonoBehaviour
{

    public Animator HeroAnimator;
    public Rigidbody2D rigidBody;
    public MovementFSM StateMachine;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private LayerMask m_WhatIsGround;

    public float JumpStoper = 10;
    public float QuickFall = 5;


    public float runSpeed;
    public float jumpForce;

    private bool isGrounded;
    private bool LookingRight = true;
    private float horizontalMove;
    private Vector3 actualVelocity = Vector3.zero;
    private bool JumpRequested = false;


    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }

    public event EventHandler Landed;

    // Start is called before the first frame update
    void Start()
    {
        StateMachine = new MovementFSM(HeroAnimator);
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down,0.5f, m_WhatIsGround);
        if (hit.collider != null)
            IsGrounded = true;
        else
            IsGrounded = false;


        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        JumpRequested = Input.GetButtonDown("Jump");

        if(StateMachine.CurrentState == MovementStates.idle)
        {
            if (horizontalMove != 0)
            {
                if (StateMachine.CurrentState == MovementStates.idle)
                    StateMachine.ProcessCommand(MovementCommands.StartsRunning);
            }
           

        }

        else if(StateMachine.CurrentState == MovementStates.running)
        {
            //if we stoped moving, we stop
            if(horizontalMove == 0)
            {
                if (StateMachine.CurrentState == MovementStates.running)
                    StateMachine.ProcessCommand(MovementCommands.StopMoving);
            }

            if(!IsGrounded) //we where running and we failed
            {
                StateMachine.ProcessCommand(MovementCommands.Fall);
            }
        }
        
        if(JumpRequested)
        {
            StateMachine.ProcessCommand(MovementCommands.Jump);
        }

        if(StateMachine.CurrentState == MovementStates.jumping)
        {
            if(Input.GetButton("Jump"))
                rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (JumpStoper) * Time.deltaTime;
            else
                rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (QuickFall) * Time.deltaTime;

            if (rigidBody.velocity.y < 0)
                StateMachine.ProcessCommand(MovementCommands.Fall);
        }

        if(StateMachine.CurrentState == MovementStates.falling)
        {
            rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (QuickFall) * Time.deltaTime;

            if (isGrounded)
                StateMachine.ProcessCommand(MovementCommands.LandedAfterAir);
        }

        /*
        if (!isGrounded)
        {
            if (StateMachine.CurrentState == MovementStates.running) // we can be unvoluntarelly falling
            {
                StateMachine.ProcessCommand(MovementCommands.Fall);
            }


            
            si estamos saltando:
                si apretamos el boton poca resistencia
                no no apretamos el boton mucha resisencia
                si la V pasa a ser negativa estamos  cayendo
             

            si estamos cayendo:
               mucha resistencia
            
                

            else if (StateMachine.CurrentState == MovementStates.jumping)
            {
                if (rigidBody.velocity.y > 0 && !Input.GetButton("Jump") && !isGrounded)
                {
                    rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (5 - 1) * Time.deltaTime;
                }
                if(rigidBody.velocity.y > 0)
                    StateMachine.ProcessCommand(MovementCommands.Fall);
            }

            else if (StateMachine.CurrentState == MovementStates.falling)
            {
                rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (10 - 1) * Time.deltaTime;
            }
        }

        if(StateMachine.CurrentState == MovementStates.falling)
        {
            if(IsGrounded) //we landed
            {
                StateMachine.ProcessCommand(MovementCommands.LandedAfterAir);
                IsGrounded = true;
            }
        }
        */
    }

   


    private void FixedUpdate()
    {
        if (JumpRequested)
        {
            IsGrounded = false;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            StateMachine.ProcessCommand(MovementCommands.Jump);
        }

        Vector3 targetVelocity = new Vector2(horizontalMove * 10f, rigidBody.velocity.y);
        rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref actualVelocity, m_MovementSmoothing);
        if (LookingRight && horizontalMove < 0)
            Flip();
        else if (!LookingRight && horizontalMove > 0)
            Flip();

       
    }

    private void Flip()
    {
        LookingRight = !LookingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
