using Scellecs.Morpeh;
using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Signals
{
    public class VitaChangedSignal
    {
        public AUnit entity;

        public VitaChangedSignal(AUnit entity)
        {
            this.entity = entity;
        }
    }
}