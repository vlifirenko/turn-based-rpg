using TurnBasedRPG.Model;

namespace TurnBasedRPG.Signals
{
    public class AttacksLeftChangedSignal
    {
        public CurrentMax value;

        public AttacksLeftChangedSignal(CurrentMax value)
        {
            this.value = value;
        }
    }
}