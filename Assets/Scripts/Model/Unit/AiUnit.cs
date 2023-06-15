using Scellecs.Morpeh;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Signals;
using Zenject;

namespace TurnBasedRPG.Model.Unit
{
    public class AiUnit : AUnit
    {
        public AiUnit(Entity entity, UnitConfig config, SignalBus signalBus) : base(entity, config, signalBus)
        {
        }

        public override void StartTurn()
        {
            UnityEngine.Debug.Log("ai turn");
            SignalBus.Fire(new NextTurnSignal());
        }
    }
}