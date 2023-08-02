using Scellecs.Morpeh;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Components.Unit
{
    public struct ARPGMovementComponent : IComponent
    {
        public Vector3 direction;
        public float speed;
    }
}