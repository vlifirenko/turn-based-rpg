using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Enums;
using Zenject;

namespace TurnBasedRPG.Model.Unit
{
    public class EnemyUnit : AUnit
    {
        protected override EItemSlot[] AvailableSlots => new[] {EItemSlot.Weapon};

        public EnemyUnit(Entity entity, UnitConfig config, SignalBus signalBus) : base(entity, config, signalBus)
        {
        }

        public override void StartTurn() => Entity.AddComponent<EnemyTurnComponent>();
    }
}