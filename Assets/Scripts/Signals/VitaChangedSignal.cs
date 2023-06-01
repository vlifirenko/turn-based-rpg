using Scellecs.Morpeh;

namespace TurnBasedRPG.Signals
{
    public class VitaChangedSignal
    {
        public Entity entity;

        public VitaChangedSignal(Entity entity)
        {
            this.entity = entity;
        }
    }
}