using Scellecs.Morpeh;

namespace TurnBasedRPG.Signals
{
    public class SetActiveUnitSignal
    {
        public Entity activeUnit;

        public SetActiveUnitSignal(Entity activeUnit)
        {
            this.activeUnit = activeUnit;
        }
    }
}