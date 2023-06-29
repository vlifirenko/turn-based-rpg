using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Enums;
using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Model.Item
{
    public abstract class AItem
    {
        public string ID { get; }
        public ItemConfig Config { get; }
        public string Name { get; }
        public string Description { get; }
        public int Cost { get; }
        public EItemSlot Slot { get; }
        public AUnit Owner { get; set; }

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