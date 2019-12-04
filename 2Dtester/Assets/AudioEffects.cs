using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffects : MonoBehaviour
{
    public AudioClip LandSound;
    public AudioClip StepSound;
    public AudioClip JumpSound;

    public enum Sounds { land, step, jump};
    public AudioSource player;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlayEffect(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.land:
                player.PlayOneShot(LandSound);
                break;
            case Sounds.step:
                player.PlayOneShot(StepSound);
                break;
            case Sounds.jump:
                player.PlayOneShot(JumpSound);
                break;
            default:
                break;
        }
    }
   
}
