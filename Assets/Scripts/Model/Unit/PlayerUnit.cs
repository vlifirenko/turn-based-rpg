using Scellecs.Morpeh;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Enums;
using Zenject;

namespace TurnBasedRPG.Model.Unit
{
    public class PlayerUnit : AUnit
    {
        protected override EItemSlot[] AvailableSlots => new[]
        {
            EItemSlot.Back, EItemSlot.Belt, EItemSlot.Chest, EItemSlot.Hand, EItemSlot.Head, EItemSlot.Legs,
            EItemSlot.Neck, EItemSlot.Ring, EItemSlot.Shield, EItemSlot.Weapon
        };

        public PlayerUnit(Entity entity, UnitConfig config, SignalBus signalBus) : base(entity, config, signalBus)
        {
        }
    }
}