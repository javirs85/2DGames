using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Weapons
{
    class Sword : IWeapon
    {
        private int damage = 1;

        public int Damage { get => damage; set => damage = value; }

        public float CoolDownSeconds => 0.2f;
        public float ComboSeconds => 0.1f;
    }
}
