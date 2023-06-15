using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Services;
using TurnBasedRPG.Signals;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class MovementSystem : ISystem
    {
        public World World { get; set; }
        private readonly BattleService _battleService;
        private readonly GlobalConfigInstaller.UnitsConfig _unitsConfig;
        private readonly SignalBus _signalBus;

        private Filter _filter;

        public MovementSystem(BattleService battleService, GlobalConfigInstaller.UnitsConfig unitsConfig,
            SignalBus signalBus)
        {
            _battleService = battleService;
            _unitsConfig = unitsConfig;
            _signalBus = signalBus;
        }

        public void OnAwake() => _filter = World.Filter.With<MovementComponent>();

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var stride = ref entity.GetComponent<StrideComponent>();
                if (stride.Value.Current == 0)
                {
                    MovementEnd(entity);
                    return;
                }

                var movement = entity.GetComponent<MovementComponent>();
                if (movement.targetCell == null)
                    FindNextCell(entity);

                MoveTo(entity, deltaTime);
            }
        }

        private void FindNextCell(Entity entity)
        {
            ref var movement = ref entity.GetComponent<MovementComponent>();
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

        private void MoveTo(Entity entity, float deltaTime)
        {
            var movement = entity.GetComponent<MovementComponent>();
            var targetCell = movement.targetCell;
            ref var unit = ref entity.GetComponent<UnitComponent>();
            var destination = new Vector3(
                targetCell.transform.position.x,
                unit.view.transform.position.y,
                targetCell.transform.position.z);

            var position = Vector3.MoveTowards(
                unit.view.transform.position,
                destination,
                _unitsConfig.moveSpeed * deltaTime);

            if (Vector3.Distance(position, destination) > _unitsConfig.stopDistance)
                unit.view.transform.position = position;
            else
            {
                ref var stride = ref entity.GetComponent<StrideComponent>();
                stride.Value.Current -= 1;
                _signalBus.Fire(new StrideChangedSignal(stride.Value));
                unit.view.transform.position = destination;
                unit.cellView = movement.targetCell;

                if (stride.Value.Current == 0 || unit.cellView.Position == movement.destination)
                    MovementEnd(entity);
                else
                    FindNextCell(entity);
            }
        }

        private static void MovementEnd(Entity entity)
        {
            entity.GetComponent<MovementComponent>().OnMovementComplete?.Invoke();
            entity.RemoveComponent<MovementComponent>();
        }

        public void Dispose()
        {
        }
    }
}