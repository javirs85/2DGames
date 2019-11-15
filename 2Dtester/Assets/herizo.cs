using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Enemies
{
    public class herizo : MonoBehaviour, IEnemy, IAttackable
    {

        private Material DefaultMaterial;

        IWeapon _currentWeapon = new Assets.Weapons.AnimalWar();
        AudioSource AudioPlayer;
        Animator animator;
        Rigidbody2D rb;
        BoxCollider2D HeroCollider;
        List<SpriteRenderer> AllRenderers;
        private bool deactivated = false; //we've been hit


        int _maxHP = 10;
        public int MaxHP { get => _maxHP; set => throw new System.NotImplementedException(); }

        int _currentHP;
        public int CurrentHP { get => _currentHP; set => _currentHP = value; }


        public ParticleSystem BloodSpat;
        public AudioClip DieAudio;
        public AudioClip HitAudio;
        public Material WhiteMaterial;
        public Material OrangeMaterial;
        public SpriteRenderer SprRenderer;

        IWeapon IEnemy.CurrentWeapon { get => _currentWeapon; set => _currentWeapon = value; }
        public int JumpWhenHitXFactor;
        public int JumpWhenHitYFactor;
        public int MovementSpeed;

        //move related private variables
        private Vector3 m_Velocity = Vector3.zero;
        [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
        private bool m_FacingRight = false;  // For determining which way the player is currently facing.
        

        private void Awake()
        {
            CurrentHP = MaxHP;
            AudioPlayer = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            DefaultMaterial = SprRenderer.material;
            HeroCollider = GetComponent<BoxCollider2D>();

            AllRenderers = this.GetComponentsInChildren<SpriteRenderer>().ToList();
        }

        public void ReceiveDamange(int HP, Vector3 SourceOfHarm)
        {
            animator.SetTrigger("Hit");

            if (HP >= 2)
                SetToWhiteSeconds(WhiteMaterial, 0.1f);
            else
                SetToWhiteSeconds(OrangeMaterial, 0.1f);

            DeactivateColliderSeconds(0.2f);

            Instantiate(BloodSpat, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(HitAudio, transform.position);

            PushAwayFromAttacker(SourceOfHarm);

            CurrentHP -= HP;
            Debug.Log($"{this.name} hit with {HP} points, we now have {CurrentHP} HP");
            if (CurrentHP < 0)
                Die();
        }


        private void PushAwayFromAttacker(Vector3 SourceOfDanger)
        {
            var vector = transform.position - SourceOfDanger;
            var direction = vector.x;
            var shootDirection = new Vector2(-1, 1);
            if (direction > 0)
                shootDirection = new Vector2(1, 1);

            //xFactor = 400;
            //yFactor = 50;
            shootDirection = new Vector2(shootDirection.x * JumpWhenHitXFactor, shootDirection.y * JumpWhenHitYFactor);
            rb.AddForce(shootDirection, ForceMode2D.Impulse);
        }

        private void DeactivateColliderSeconds(float seconds)
        {
            deactivated = true;
            HeroCollider.enabled = false;
            Invoke("ResetCollider", seconds);
        }
        void ResetCollider()
        {
            deactivated = false;
            HeroCollider.enabled = true;
        }
        private void SetToWhiteSeconds(Material mat, float seconds)
        {
            AllRenderers.ForEach(x => x.material = mat);
            Invoke("ResetSprite", seconds);
        }
        void ResetSprite()
        {
            AllRenderers.ForEach(x => x.material = DefaultMaterial);
        }

        void Die()
        {
            AudioSource.PlayClipAtPoint(DieAudio, transform.position);
            Destroy(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            CurrentHP = MaxHP;
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            var hero = col.collider.gameObject.GetComponent<PlayerMovement>();
            if (hero != null)
            {
                hero.ReceiveDamange(this._currentWeapon.Damage, transform.position);
                animator.SetTrigger("ContactAttack");
            }
            Debug.Log(col.gameObject.name + " has collided with " + this.name);
        }

        // Update is called once per frame
        void Update()
        {
            if(!deactivated)
                {
                var distanceToHeroVector = (GameController.HeroReference.transform.position - transform.position);
                var distanceToHero = distanceToHeroVector.magnitude;
                if (distanceToHero < 6)
                {
                    int move = 0;
                    if (distanceToHeroVector.x > 0)
                    {
                        move = 1;
                        if (!m_FacingRight) Flip();
                    }
                    else
                    {
                        move = -1;
                        if (m_FacingRight) Flip();
                    }


                    Vector3 targetVelocity = new Vector2(move * MovementSpeed, rb.velocity.y);
                    rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
                }
            }
        }

        void Flip()
        {
            this.transform.Rotate(new Vector3(0, 180, 0));
            m_FacingRight = !m_FacingRight;
        }
    }
}