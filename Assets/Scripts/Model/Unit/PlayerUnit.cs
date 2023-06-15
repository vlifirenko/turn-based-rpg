using Scellecs.Morpeh;
using TurnBasedRPG.Model.Config;
using Zenject;

namespace TurnBasedRPG.Model.Unit
{
    public class PlayerUnit : AUnit
    {
        public PlayerUnit(Entity entity, UnitConfig config, SignalBus signalBus) : base(entity, config, signalBus)
        {
        }
    }
}