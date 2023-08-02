using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.View.Unit;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class ARPGMovementSystem : ISystem
    {
        public World World { get; set; }
        private Filter _filter;

        public void OnAwake() => _filter = World.Filter
            .With<ARPGMovementComponent>()
            .Without<DeadComponent>();

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                var movement = entity.GetComponent<ARPGMovementComponent>();
                var unit = entity.GetComponent<UnitComponent>().value;
                var view = unit.View as DiabloKillerView;

                var movementVector = new Vector3(movement.direction.x, 0f, movement.direction.z);

                movementVector *= movement.speed * deltaTime;
                //UnityEngine.Debug.Log(movementVector);
                view.CharacterController.Move(movementVector);
                view.transform.rotation = Quaternion.LookRotation(
                    new Vector3(movementVector.x, 0, movementVector.z),
                    Vector3.up);
            }
        }

        public void Dispose()
        {
        }
    }
}