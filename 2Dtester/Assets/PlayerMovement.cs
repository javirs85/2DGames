using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IAttackable
{

    private Rigidbody2D rb;
    private CharacterController2D controller;
    private Animator animator;
    private float horizontalMove = 0f;
    private bool jump;
    private bool WasHit = false;
    int _maxHP = 10;
    int _currentHP;
    float LostInmuneNextTime;
    private bool CanDash = true;
    private Material defaultMaterial;
    private List<SpriteRenderer> AllRenderers;
    private Cinemachine.CinemachineImpulseSource CameraShaker;


    public float runSpeed = 40f;
    public float FlyOnHitForce;
    public float HitInmunneSeconds;
    public Material BlackMaterial;
    public GameObject HitEffect;

    public int MaxHP { get => _maxHP; set => throw new System.NotImplementedException(); }
    public int CurrentHP { get =>_currentHP; set => _currentHP = value; }


    


    // Start is called before the first frame update
    void Awake()
    {
        CurrentHP = MaxHP;

        controller = this.GetComponent<CharacterController2D>();
        controller.Landed += Controller_Landed;
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        CameraShaker = this.GetComponent<Cinemachine.CinemachineImpulseSource>();

        AllRenderers = this.GetComponentsInChildren<SpriteRenderer>().ToList();

        defaultMaterial = GetComponent<SpriteRenderer>().material;
        GameController.Init(this);

    }

    private void Controller_Landed(object sender, System.EventArgs e)
    {
        CanDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!WasHit)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            animator.SetFloat("VSpeed", rb.velocity[1]);
            if (Input.GetButtonDown("Jump"))
                jump = true;
            if (Input.GetButtonDown("Fire2") && CanDash && !controller.IsGrounded)
            {
                CanDash = false;
                controller.Dash();
            }
        }
        else
        {
            horizontalMove = 0;
            if (Time.time >= LostInmuneNextTime)
            {
                AllRenderers.ForEach(x => x.material = defaultMaterial);
                WasHit = false;
            }
        }
    }
    

    private void FixedUpdate()
    {
        if (!WasHit)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        }
        
        jump = false;
    }

    public void ReceiveDamange(int HP, Vector3 SourceOfHarm)
    {
        if (WasHit)
            return; //we are in inmune time, we dont give a shit

        Instantiate(HitEffect, transform.position, Quaternion.identity);
        CameraShaker.GenerateImpulse(this.transform.position);
        WasHit = true;
        LostInmuneNextTime = Time.time + HitInmunneSeconds;
        AllRenderers.ForEach(x => x.material = BlackMaterial);

        _currentHP -= HP;
        Debug.Log($"We've been hit with {HP} points, we now have {CurrentHP} HP");

        var vector = transform.position - SourceOfHarm;
        var direction = vector.x;
        var shootDirection = new Vector2(-1, 2);
        if (direction > 0)
            shootDirection = new Vector2(1, 2);

        rb.AddForce(shootDirection * FlyOnHitForce, ForceMode2D.Impulse);


        if (CurrentHP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    
}
