using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Signals
{
    public class UnitUpdatedSignal
    {
        public AUnit unit;

        public UnitUpdatedSignal(AUnit unit)
        {
            this.unit = unit;
        }
    }
}