using System;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Services;

namespace TurnBasedRPG.Model.Item
{
    public class ConsumableItem : AItem, IUsable
    {
        public int Amount { get; set; }
        public int MaxAmount { get; set; }
        public bool IsDestroyAfterUsage { get; }

        public ConsumableItem(ItemConfig config) : base(config)
        {
            IsDestroyAfterUsage = config.destroyAfterUsage;
            Amount = config.amount;
            MaxAmount = Amount;
        }

        public bool Use()
        {
            if (IsEmpty())
                throw new Exception("use empty consumable");

            Amount--;

            if (IsEmpty() && IsDestroyAfterUsage)
            {
                Destroy();
                return true;
            }

            return false;
        }

        private bool IsEmpty() => Amount == 0;
    }
}