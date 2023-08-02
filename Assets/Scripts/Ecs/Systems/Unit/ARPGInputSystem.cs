using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class ARPGInputSystem : ISystem
    {
        private const float MOVEMENT_THRESHOLD = 0.1f;

        public World World { get; set; }
        private Filter _filter;

        public void OnAwake() => _filter = World.Filter.With<PlayerComponent>();

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                HandleMovement(entity);
                HandleAttack(entity);
            }
        }

        private static void HandleMovement(Entity entity)
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontal) < MOVEMENT_THRESHOLD && Mathf.Abs(vertical) < MOVEMENT_THRESHOLD)
            {
                if (entity.Has<ARPGMovementComponent>())
                    entity.RemoveComponent<ARPGMovementComponent>();
                return;
            }

            var direction = new Vector3(horizontal, 0, vertical);
            
            //UnityEngine.Debug.Log(direction);

            if (entity.Has<ARPGMovementComponent>())
            {
                ref var movement = ref entity.GetComponent<ARPGMovementComponent>();
                movement.direction = direction.normalized;
            }
            else
            {
                entity.AddComponent<ARPGMovementComponent>() = new ARPGMovementComponent
                {
                    direction = direction,
                    speed = entity.GetComponent<StrideComponent>().Value.Current
                };
            }
        }

        private void HandleAttack(Entity entity)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!entity.Has<ARPGAttackComponent>())
                    entity.AddComponent<ARPGAttackComponent>();
            }
        }

        public void Dispose()
        {
        }
    }
}