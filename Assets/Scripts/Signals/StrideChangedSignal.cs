using TurnBasedRPG.Model;

namespace TurnBasedRPG.Signals
{
    public class StrideChangedSignal
    {
        public CurrentMax value;

        public StrideChangedSignal(CurrentMax value)
        {
            this.value = value;
        }
    }
}