using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using UnityEngine;

namespace TurnBasedRPG.Extensions
{
    public static class UnitExtensions
    {
        public static void MoveTo(this Entity entity, Vector2Int destination)
        {
            entity.GetComponent<AnimatorComponent>().Value.SetState(EAnimatorState.Move);
            entity.AddComponent<MovementComponent>() = new MovementComponent
            {
                destination = destination
            };
        }
    }
}