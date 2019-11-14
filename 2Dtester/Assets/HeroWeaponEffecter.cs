using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroWeaponEffecter : MonoBehaviour
{

    private void Awake()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var dude = collision.gameObject.GetComponent<Assets.IAttackable>();
        if (dude != null)
        {
            dude.ReceiveDamange(Assets.GameController.CurrentWeapon.Damage);
        }

    }
}
