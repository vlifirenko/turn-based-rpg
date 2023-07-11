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

        public virtual AItem Equip(AUnit owner)
        {
            Owner = owner;
            var unequppedItem = owner.Equip(this);
            foreach (var itemEffect in Effects)
            {
            }

            return unequppedItem;
        }

        public virtual void Unequip()
        {
            Owner.Unequip(this);
            foreach (var itemEffect in Effects)
            {
            }

            Owner = null;
        }
    }
}