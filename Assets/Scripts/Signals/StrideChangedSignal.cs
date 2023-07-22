using TurnBasedRPG.Model;
using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Signals
{
    public class StrideChangedSignal
    {
        public CurrentMax value;
        public AUnit unit;

        public StrideChangedSignal(CurrentMax value, AUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }
    }
}