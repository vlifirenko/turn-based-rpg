using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Enums;

namespace TurnBasedRPG.Model.Item
{
    public abstract class AItem
    {
        public string ID { get; private set; }
        public ItemConfig Config { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Cost { get; private set; }
        public EItemSlot Slot { get; private set; }

        protected AItem(ItemConfig config)
        {
            Config = config;
            ID = config.id;
            Name = config.name;
            Description = config.description;
            Cost = config.cost;
            Slot = config.slot;
        }
    }
}