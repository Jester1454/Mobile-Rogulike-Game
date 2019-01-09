using System;
using Environment;

namespace ActiveAbility
{
    public interface IUseAbility
    {
        void UseAbility();
        void PickUpAbility();
        void DropAbility(Room room);
        int Cooldown { get; set; }
        int CurrentCharge { get; set; }
        bool CanUseAbility { get; }
        Action<int> UpdateCharge { get; set; }
    }
}