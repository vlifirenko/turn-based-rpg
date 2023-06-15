using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Extensions;
using UnityEngine;

namespace TurnBasedRPG.Model.Unit
{
    public abstract class AUnit
    {
        private readonly Entity _entity;

        protected AUnit(Entity entity)
        {
            _entity = entity;
        }

        public Entity Entity => _entity;

        public void MoveTo(Vector2Int destination)
        {
            _entity.GetComponent<AnimatorComponent>().Value.SetState(EAnimatorState.Move);
            _entity.AddComponent<MovementComponent>() = new MovementComponent
            {
                destination = destination
            };
        }
    }
}