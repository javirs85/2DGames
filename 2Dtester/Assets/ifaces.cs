using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public interface IAttackable
    {
        int MaxHP { get; set; }
        int CurrentHP { get; set; }
        void ReceiveDamange(int HP);

    }

    public interface IEnemy
    {
        IWeapon CurrentWeapon { get; set; }

    }

    public interface IWeapon
    {
        int Damage { get; set; }
        float CoolDownSeconds { get; }
        float ComboSeconds { get; }
    }
}
