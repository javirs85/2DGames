using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Weapons;
using UnityEngine;

namespace Assets
{
    public static class GameController
    {
        public static IWeapon CurrentWeapon = null;
        public static PlayerMovement HeroReference = null;

        public static void Init(PlayerMovement obj)
        {
            HeroReference = obj;
            CurrentWeapon = new Sword();
        }
    }
}
