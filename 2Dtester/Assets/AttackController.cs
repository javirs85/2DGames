using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    class AttackController
    {
        private Animator EffectsAnimator;
        private Animator MovementAnimator;

        float NextValidFireTime;
        float NextComboTime;
        int ComboStep = 0;

        public enum Animations { Slash1, Slash2 };

        public AttackController(Animator movement, Animator anim)
        {
            EffectsAnimator = anim;
            MovementAnimator = movement;
        }


        public void Slash(bool isGrounded)
        {
            if (Time.time > NextValidFireTime) //we passed the cool down
            {
                if (Time.time < NextComboTime) //we are between cooldown and combo, we make combo
                {
                    Debug.Log("Combo!: "+ NextValidFireTime + ", " + Time.time + " , " + NextComboTime);

                    if (ComboStep == 1)
                    {
                        FireAnimation(Animations.Slash2);
                        if (isGrounded)
                            MovementAnimator.SetTrigger("Attack1");
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
                    FireAnimation(Animations.Slash1);
                    if (isGrounded)
                        MovementAnimator.SetTrigger("Attack1");
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
}
