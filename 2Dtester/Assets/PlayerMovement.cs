using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private CharacterController2D controller;
    private Animator animator;
    private float horizontalMove = 0f;
    private bool jump;
    private AttackController attackController;

    public float runSpeed = 40f;
    public Animator WeaponEffectsAnimator;
   

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController2D>();
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();

        attackController = new AttackController(animator, WeaponEffectsAnimator);

        GameController.Init();

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animator.SetFloat("VSpeed", rb.velocity[1]);
        if (Input.GetButtonDown("Jump"))
            jump = true;

        if (Input.GetButtonDown("Fire1"))
        {
            attackController.Slash(controller.IsGrounded);           
        }
    }


    /*

    // hooks for animation events

    void Slash1Finished(string animation)
    {
        if (animation == "Slash1")
            animator.SetBool("Atack1", false);
    }
   */
    

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
