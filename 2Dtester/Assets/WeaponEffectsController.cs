using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attacker;

public class WeaponEffectsController : MonoBehaviour
{
    private AudioSource AudioPlayer;
    private Animator EffectsAnimator;
    private List<AudioClip> smallWooshes;
    private Cinemachine.CinemachineImpulseSource CameraShaker;

    public AudioClip Swoosh1;
    public AudioClip Swoosh2;
    public AudioClip Swoosh3;

    public AudioClip BigSwoosh;


    // Start is called before the first frame update
    void Awake()
    {
        EffectsAnimator = this.GetComponent<Animator>();
        AudioPlayer = this.GetComponent<AudioSource>();
        CameraShaker = this.GetComponent<Cinemachine.CinemachineImpulseSource>();
        smallWooshes = new List<AudioClip>();
        smallWooshes.Add(Swoosh1);
        smallWooshes.Add(Swoosh2);
        smallWooshes.Add(Swoosh3);
    }

    public void FireAnimation(Animations anim)
    {
        switch (anim)
        {
            case Animations.Slash1:
                EffectsAnimator.SetTrigger("Slash1Start");

                var audio = smallWooshes[UnityEngine.Random.Range(0, smallWooshes.Count)];   
                AudioPlayer.PlayOneShot(audio);

                break;
            case Animations.Slash2:
                EffectsAnimator.SetTrigger("Slash2Combo");
                AudioPlayer.PlayOneShot(BigSwoosh);
                break;
            default:
                break;
        }
    }

    internal void HitSimple()
    {
        CameraShaker.GenerateImpulse(this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
           
    }
}
