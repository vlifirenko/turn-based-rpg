using Scellecs.Morpeh;
using TurnBasedRPG.Signals;
using Zenject;

namespace TurnBasedRPG.Model.Unit
{
    public class AiUnit : AUnit
    {
        private readonly SignalBus _signalBus;
        
        public AiUnit(Entity entity, SignalBus signalBus) : base(entity)
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