using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Services;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class MovementSystem : ISystem
    {
        public World World { get; set; }
        private readonly BattleService _battleService;

        private Filter _filter;

        public MovementSystem(BattleService battleService)
        {
            _battleService = battleService;
        }

        public void OnAwake() => _filter = World.Filter.With<MovementComponent>();

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var stride = ref entity.GetComponent<StrideComponent>();
                if (stride.Value.Current == 0)
                {
                    entity.RemoveComponent<MovementComponent>();
                    return;
                }

                ref var movement = ref entity.GetComponent<MovementComponent>();
                if (movement.targetCell == null)
                {
                    var destinationCell = _battleService.GetCellByPosition(movement.destination.x,
                        movement.destination.y);
                    var unitCell = entity.GetComponent<UnitComponent>().cellView;
                    var position = unitCell.Position;

                    if (destinationCell.Position.x > position.x)
                        position.x++;
                    else if (destinationCell.Position.x < position.x)
                        position.x--;
                    if (destinationCell.Position.y > position.y)
                        position.y++;
                    else if (destinationCell.Position.y < position.y)
                        position.y--;

                    var targetCell = _battleService.GetCellByPosition(position.x, position.y);
                    movement.targetCell = targetCell;
                }

                MoveTo(entity, deltaTime);
            }
        }

        private void MoveTo(Entity entity, float deltaTime)
        {
            var movement = entity.GetComponent<MovementComponent>();
            var targetCell = movement.targetCell;
            ref var unit = ref entity.GetComponent<UnitComponent>();

            var position = Vector3.Lerp(
                unit.view.transform.position,
                new Vector3(targetCell.transform.position.x, unit.view.transform.position.y,
                    targetCell.transform.position.z),
                2f * deltaTime);
            unit.view.transform.position = position;
        }


        public void Dispose()
        {
        }
    }
}