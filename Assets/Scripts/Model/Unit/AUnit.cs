using System;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Extensions;
using TurnBasedRPG.Model.Config;
using UnityEngine;

namespace TurnBasedRPG.Model.Unit
{
    public abstract class AUnit
    {
        private readonly Entity _entity;
        private readonly UnitConfig _config;

        protected AUnit(Entity entity, UnitConfig config)
        {
            _entity = entity;
            _config = config;
        }

        public Entity Entity => _entity;
        public UnitConfig Config => _config;

        public void MoveTo(Vector2Int destination, Action onMovementComplete = null)
        {
            if (_entity.Has<MovementComponent>())
                return;
            
            _entity.GetComponent<AnimatorComponent>().Value.SetState(EAnimatorState.Move);
            _entity.AddComponent<MovementComponent>() = new MovementComponent
            {
                destination = destination,
                OnMovementComplete = onMovementComplete
            };
        }
    }
}