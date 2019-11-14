using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemies
{
    public class herizo : MonoBehaviour, IEnemy, IAttackable
    {
        IWeapon _currentWeapon = new Assets.Weapons.AnimalWar();

        int _maxHP = 10;
        public int MaxHP { get => _maxHP; set => throw new System.NotImplementedException(); }

        int _currentHP;
        public int CurrentHP { get => _currentHP; set => _currentHP = value; }


        public ParticleSystem BloodSpat;


        IWeapon IEnemy.CurrentWeapon { get => _currentWeapon; set => _currentWeapon = value; }
        

        public void ReceiveDamange(int HP)
        {
            Instantiate(BloodSpat, transform.position, Quaternion.identity);

            CurrentHP -= HP;
            Debug.Log($"{this.name} hit with {HP} points, we now have {CurrentHP} HP");
            if (CurrentHP < 0)
                Die();
        }

        void Die()
        {
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
                hero.ReceiveDamange(this._currentWeapon.Damage);
            }
            Debug.Log(col.gameObject.name + " has collided with " + this.name);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}