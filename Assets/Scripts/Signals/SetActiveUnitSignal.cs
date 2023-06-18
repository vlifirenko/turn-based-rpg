using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Signals
{
    public class SetActiveUnitSignal
    {
        public AUnit activeUnit;

        public SetActiveUnitSignal(AUnit activeUnit)
        {
            this.activeUnit = activeUnit;
        }
    }
}