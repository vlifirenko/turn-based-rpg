using System;
using Scellecs.Morpeh;
using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Ecs.Components.Unit
{
    public struct TBAttackComponent : IComponent
    {
        public AUnit target;
        public Action onComplete;
    }
}