using System.Collections.Generic;
using System.Linq;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Enums;
using TurnBasedRPG.Model.Item;
using TurnBasedRPG.Signals;
using Zenject;

namespace TurnBasedRPG.Services
{
    public class InventoryService : IInitializable
    {
        private readonly GlobalConfigInstaller.UnitsConfig _unitsConfig;
        private readonly SignalBus _signalBus;
        private readonly BattleService _battleService;
        private readonly List<AItem> _items = new();

        public InventoryService(GlobalConfigInstaller.UnitsConfig unitsConfig, SignalBus signalBus,
            BattleService battleService)
        {
            _unitsConfig = unitsConfig;
            _signalBus = signalBus;
            _battleService = battleService;
        }

        public void Initialize()
        {
            foreach (var itemConfig in _unitsConfig.inventory)
            {
                var item = CreateItemFromConfig(itemConfig);

                item.IsInInventory = true;
                _items.Add(item);
            }

            _signalBus.Fire(new InventoryUpdatedSignal());
        }

        public AItem[] GetAllInventoryItems() => _items.Where(item => item.IsInInventory).ToArray();

        private static AItem CreateItemFromConfig(ItemConfig config)
        {
            switch (config.slot)
            {
                case EItemSlot.Weapon:
                    return new SimpleWeapon(config);
                case EItemSlot.Consumable:
                    return new Consumable(config);
                default:
                    return new SimpleArmor(config);
            }
        }

        public void InventoryItemClick(AItem item)
        {
            switch (item)
            {
                case IUsable usable:
                    usable.Use();
                    break;
                case IEquip equip:
                    item.IsInInventory = false;
                    equip.Equip(_battleService.ActiveUnit);
                    break;
            }

            _signalBus.Fire(new InventoryUpdatedSignal());
        }

        public void EquipmentItemClick(AItem item)
        {
            if (item is IEquip equip)
            {
                item.IsInInventory = true;
                equip.Unequip();
            }

            _signalBus.Fire(new InventoryUpdatedSignal());
        }
    }
}