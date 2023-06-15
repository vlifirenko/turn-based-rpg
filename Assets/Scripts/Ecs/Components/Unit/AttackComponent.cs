using Scellecs.Morpeh;
using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Ecs.Components.Unit
{
    public struct AttackComponent : IComponent
    {
        public AUnit Target;
    }
}