using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IAttackable
{

    private Rigidbody2D rb;
    private CharacterController2D controller;
    private Animator animator;
    private float horizontalMove = 0f;
    private bool jump;

    public float runSpeed = 40f;

    int _maxHP = 10;
    int _currentHP;

    public int MaxHP { get => _maxHP; set => throw new System.NotImplementedException(); }
    public int CurrentHP { get =>_currentHP; set => _currentHP = value; }


    // Start is called before the first frame update
    void Awake()
    {
        CurrentHP = MaxHP;

        controller = this.GetComponent<CharacterController2D>();
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        

        GameController.Init(this);

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animator.SetFloat("VSpeed", rb.velocity[1]);
        if (Input.GetButtonDown("Jump"))
            jump = true;
    }
    

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    public void ReceiveDamange(int HP)
    {
        _currentHP -= HP;
        Debug.Log($"We've been hit with {HP} points, we now have {CurrentHP} HP");

        if (CurrentHP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    
}
