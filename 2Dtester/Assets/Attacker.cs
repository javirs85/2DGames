using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public Animator EffectsAnimator;
    public Animator MovementAnimator;

    public Transform attackPos;
    public float AttackRange;
    public LayerMask WhatIsEnamyLayer;

    private CharacterController2D controller;
    float NextValidFireTime;
    float NextComboTime;
    int ComboStep = 0;
    bool isGrounded = true;

    public enum Animations { Slash1, Slash2 };

    private void Awake()
    {
        controller = this.GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Slash(controller.IsGrounded);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPos.position, AttackRange);
    }

    void DoSimpleSlash()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, AttackRange, WhatIsEnamyLayer);
        foreach(var enemy in enemiesToDamage)
        {
            enemy.GetComponent<IAttackable>().ReceiveDamange(GameController.CurrentWeapon.Damage);
        }

        FireAnimation(Animations.Slash1);
        if (isGrounded)
            MovementAnimator.SetTrigger("Attack1");

    }

    void DoGreatSlash()
    {
        FireAnimation(Animations.Slash2);
        if (isGrounded)
            MovementAnimator.SetTrigger("Attack1");
    }

    public void Slash(bool _isGrounded)
    {
        isGrounded = _isGrounded;

        if (Time.time > NextValidFireTime) //we passed the cool down
        {
            if (Time.time < NextComboTime) //we are between cooldown and combo, we make combo
            {
                Debug.Log("Combo!: " + NextValidFireTime + ", " + Time.time + " , " + NextComboTime);

                if (ComboStep == 1)
                {
                    DoGreatSlash();
                    ComboStep = 0;
                }
                else
                    Debug.Log("WTF?");


                NextValidFireTime = Time.time + GameController.CurrentWeapon.CoolDownSeconds;

                //if we already did the combo we set nextcombotime to something unreacheable.
                if (ComboStep != 0)
                    NextComboTime = NextValidFireTime + GameController.CurrentWeapon.ComboSeconds;
                else
                    NextComboTime = NextValidFireTime - 1;

            }
            else   //we are in a valid hit, but not a combo. Perform standard slash
            {

                NextValidFireTime = Time.time + GameController.CurrentWeapon.CoolDownSeconds;
                NextComboTime = NextValidFireTime + GameController.CurrentWeapon.ComboSeconds;
                Debug.Log("Valid hit: " + Time.time + " , " + NextValidFireTime + ", " + NextComboTime);
                ComboStep = 1;

                DoSimpleSlash();
            }
        }
        else
            Debug.Log("CoolingDown: " + Time.time + " , " + NextValidFireTime + ", " + NextComboTime);
    }

    public void FireAnimation(Animations anim)
    {
        switch (anim)
        {
            case Animations.Slash1:
                EffectsAnimator.SetTrigger("Slash1Start");
                break;
            case Animations.Slash2:
                EffectsAnimator.SetTrigger("Slash2Combo");
                break;
            default:
                break;
        }
    }
}
