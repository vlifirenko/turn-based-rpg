using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Signals;
using Zenject;

namespace TurnBasedRPG.Model.Unit
{
    public class EnemyUnit : AUnit
    {
        public EnemyUnit(Entity entity, UnitConfig config, SignalBus signalBus) : base(entity, config, signalBus)
        {
        }

        public override void StartTurn()
        {
            UnityEngine.Debug.Log("enemy turn");
            Entity.AddComponent<EnemyTurnComponent>();
        }
    }
}