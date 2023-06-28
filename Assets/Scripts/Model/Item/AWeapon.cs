﻿using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Enums;

namespace TurnBasedRPG.Model.Item
{
    public abstract class AWeapon : AItem
    {
        public WeaponDamage Damage { get; private set; }
        public bool IsTwoHanded { get; private set; }
        public EItemDistance Distance { get; private set; }
        public int Range { get; private set; }
        public int Charges { get; private set; }

        protected AWeapon(ItemConfig config) : base(config)
        {
            Damage = config.weaponDamage;
            IsTwoHanded = config.isTwoHanded;
            Distance = config.distance;
            Range = config.range;
            Charges = config.charges;
        }

        public string GetDamageText()
        {
            var min = Damage.diceCount + Damage.bonus;
            var max = Damage.diceCount * (int) Damage.dice + Damage.bonus;

            return $"{min}-{max}";
        }
    }
}