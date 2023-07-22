using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Enums;

namespace TurnBasedRPG.Model.Item
{
    public abstract class AWeapon : AEquipment
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
            var min = Damage.diceCount + Damage.bonus + Owner.DamageBonus;
            var max = Damage.diceCount * (int) Damage.dice + Damage.bonus + Owner.DamageBonus;

            return $"{min}-{max}";
        }
    }
}