using System;
using Scellecs.Morpeh;

namespace TurnBasedRPG.Ecs.Components.Unit
{
    public struct DamageComponent : IComponent
    {
        public Damage Value;
    }

    [Serializable]
    public class Damage
    {
    }
}