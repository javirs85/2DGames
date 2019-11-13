using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public static class GameController
    {
        public static IWeapon CurrentWeapon = null;

        public static void Init()
        {
            CurrentWeapon = new Sword();
        }
    }
}
