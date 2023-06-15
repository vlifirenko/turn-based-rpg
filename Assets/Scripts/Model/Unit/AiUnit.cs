using Scellecs.Morpeh;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Signals;
using Zenject;

namespace TurnBasedRPG.Model.Unit
{
    public class AiUnit : AUnit
    {
        private readonly SignalBus _signalBus;
        
        public AiUnit(Entity entity, UnitConfig config, SignalBus signalBus) : base(entity, config)
        {
            _signalBus = signalBus;
        }

        public void MakeTurn()
        {
            UnityEngine.Debug.Log("ai turn");
            
            _signalBus.Fire(new NextTurnSignal());
        }
    }
}