using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Unit;
using UnityEngine;

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
                ApplyEffect(itemEffect);

            return unequppedItem;
        }

        public virtual void Unequip()
        {
            Owner.Unequip(this);
            foreach (var itemEffect in Effects)
                CancelEffect(itemEffect);

            Owner = null;
        }

        private void ApplyEffect(ItemEffect itemEffect)
        {
            throw new System.NotImplementedException();
        }

        private void CancelEffect(ItemEffect itemEffect)
        {
            throw new System.NotImplementedException();
        }
    }
}