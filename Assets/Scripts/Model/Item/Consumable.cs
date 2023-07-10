using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Services;

namespace TurnBasedRPG.Model.Item
{
    public class Consumable : AItem, IUsable
    {
        public int Amount { get; set; }
        public int MaxAmount { get; set; }

        public Consumable(ItemConfig config) : base(config)
        {
        }

        public void Use()
        {
            if (IsEmpty())
                return;

            Amount--;
        }

        private bool IsEmpty() => Amount == 0;
    }
}