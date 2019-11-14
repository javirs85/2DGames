using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Weapons
{
    public class AnimalWar : IWeapon
    {
        private int damage = 1;

        public int Damage { get => damage; set => damage = value; }

        public float CoolDownSeconds => 0;
        public float ComboSeconds => 0;
    }
}
