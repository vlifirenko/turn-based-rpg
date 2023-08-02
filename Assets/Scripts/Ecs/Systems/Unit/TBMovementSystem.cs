using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Services;
using TurnBasedRPG.Services.Facade;
using TurnBasedRPG.Signals;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class TBMovementSystem : ISystem
    {
        public World World { get; set; }
        private readonly BattleService _battleService;
        private readonly GlobalConfigInstaller.UnitsConfig _unitsConfig;
        private readonly SignalBus _signalBus;
        private readonly TBMapService _tbMapService;

        private Filter _filter;

        public TBMovementSystem(BattleService battleService, GlobalConfigInstaller.UnitsConfig unitsConfig,
            SignalBus signalBus, TBMapService tbMapService)
        {
            _battleService = battleService;
            _unitsConfig = unitsConfig;
            _signalBus = signalBus;
            _tbMapService = tbMapService;
        }

        public void OnAwake() => _filter = World.Filter.With<TBMovementComponent>();

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

                var movement = entity.GetComponent<TBMovementComponent>();
                if (movement.path == null)
                    BuildPath(entity);

                /*if (movement.targetCell == null)
                    FindNextCell(entity);*/

                MoveTo(entity, deltaTime);
            }
        }

        private void BuildPath(Entity entity)
        {
            var unit = entity.GetComponent<UnitComponent>().value;
            ref var movement = ref entity.GetComponent<TBMovementComponent>();

            var path = _tbMapService.BuildPath(unit.Cell.Position, movement.destination, movement.range);
            movement.path = path;

            //UnityEngine.Debug.Log("path " + path.Length);
        }

        /*private void FindNextCell(Entity entity)
        {
            var unit = entity.GetComponent<UnitComponent>().value;
            ref var movement = ref entity.GetComponent<MovementComponent>();
            var destinationCell = _mapService.GetCellByPosition(movement.destination.x,
                movement.destination.y);
            var position = unit.Cell.Position;
            
            if (destinationCell.Position.x > position.x)
                position.x++;
            else if (destinationCell.Position.x < position.x)
                position.x--;
            if (destinationCell.Position.y > position.y)
                position.y++;
            else if (destinationCell.Position.y < position.y)
                position.y--;
            
            var targetCell = _mapService.GetCellByPosition(position.x, position.y);
            movement.targetCell = targetCell;
        }*/

        private void MoveTo(Entity entity, float deltaTime)
        {
            ref var unit = ref entity.GetComponent<UnitComponent>().value;
            ref var movement = ref entity.GetComponent<TBMovementComponent>();

            if (movement.pathIndex == movement.path.Length)
            {
                MovementEnd(entity);
                return;
            }

            var targetCell = movement.path[movement.pathIndex];
            var destination = new Vector3(
                targetCell.View.transform.position.x,
                unit.View.transform.position.y,
                targetCell.View.transform.position.z);

            var position = Vector3.MoveTowards(
                unit.View.transform.position,
                destination,
                _unitsConfig.moveSpeed * deltaTime);

            // fix: clear cell only once
            if (unit.Cell != null)
            {
                unit.Cell.Content = null;
                unit.Cell = null;
            }
            //

            if (Vector3.Distance(position, destination) > _unitsConfig.stopDistance)
                unit.View.transform.position = position;
            else
            {
                ref var stride = ref entity.GetComponent<StrideComponent>();
                stride.Value.Current -= 1;
                FloatText.Show(unit.View.transform.position, "Stride -1");
                _signalBus.Fire(new StrideChangedSignal(stride.Value, entity.GetComponent<UnitComponent>().value));

                unit.View.transform.position = destination;

                targetCell.Content = unit;
                unit.Cell = targetCell;

                if (stride.Value.Current == 0 || unit.Cell.Position == movement.destination)
                    MovementEnd(entity);
                else
                {
                    movement.pathIndex += 1;
                    //MoveTo(entity, deltaTime);
                }
            }
        }

        private static void MovementEnd(Entity entity)
        {
            entity.GetComponent<TBMovementComponent>().onMovementComplete?.Invoke();
            entity.RemoveComponent<TBMovementComponent>();
        }

        public void Dispose()
        {
        }
    }
}