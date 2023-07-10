using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Model.Item
{
    public abstract class AEquipment : AItem, IEquip
    {
        public AUnit Owner { get; set; }

        public AEquipment(ItemConfig config) : base(config)
        {
        }

        public virtual void Equip(AUnit owner)
        {
            Owner = owner;
        }

        public virtual void Unequip()
        {
            Owner = null;
        }
    }
}