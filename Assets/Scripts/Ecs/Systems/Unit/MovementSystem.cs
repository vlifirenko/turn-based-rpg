﻿using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Services;
using TurnBasedRPG.Services.Facade;
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
        private readonly MapService _mapService;

        private Filter _filter;

        public MovementSystem(BattleService battleService, GlobalConfigInstaller.UnitsConfig unitsConfig,
            SignalBus signalBus, MapService mapService)
        {
            _battleService = battleService;
            _unitsConfig = unitsConfig;
            _signalBus = signalBus;
            _mapService = mapService;
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
                if (movement.path == null)
                    BuildPath(entity);

                if (movement.targetCell == null)
                    FindNextCell(entity);

                MoveTo(entity, deltaTime);
            }
        }

        private void BuildPath(Entity entity)
        {
            var unit = entity.GetComponent<UnitComponent>().value;
            ref var movement = ref entity.GetComponent<MovementComponent>();
            
            var path = _mapService.BuildPath(unit.GetMapPosition, movement.destination);
            movement.path = path;
            
            UnityEngine.Debug.Log("path " + path.Length);
        }

        private void FindNextCell(Entity entity)
        {
            ref var movement = ref entity.GetComponent<MovementComponent>();
            var destinationCell = _mapService.GetCellByPosition(movement.destination.x,
                movement.destination.y);
            var unitCell = entity.GetComponent<UnitComponent>().cellView;
            var position = unitCell.Position;

            if (destinationCell.View.Position.x > position.x)
                position.x++;
            else if (destinationCell.View.Position.x < position.x)
                position.x--;
            if (destinationCell.View.Position.y > position.y)
                position.y++;
            else if (destinationCell.View.Position.y < position.y)
                position.y--;

            var targetCell = _mapService.GetCellByPosition(position.x, position.y);
            movement.targetCell = targetCell.View;
        }

        private void MoveTo(Entity entity, float deltaTime)
        {
            var movement = entity.GetComponent<MovementComponent>();
            var targetCell = movement.targetCell;
            ref var unit = ref entity.GetComponent<UnitComponent>().value;
            ref var cellView = ref entity.GetComponent<UnitComponent>().cellView;
            var destination = new Vector3(
                targetCell.transform.position.x,
                unit.View.transform.position.y,
                targetCell.transform.position.z);

            var position = Vector3.MoveTowards(
                unit.View.transform.position,
                destination,
                _unitsConfig.moveSpeed * deltaTime);

            cellView.UnitView = null;

            if (Vector3.Distance(position, destination) > _unitsConfig.stopDistance)
                unit.View.transform.position = position;
            else
            {
                ref var stride = ref entity.GetComponent<StrideComponent>();
                stride.Value.Current -= 1;
                FloatText.Show(unit.View.transform.position, "Stride -1");
                _signalBus.Fire(new StrideChangedSignal(stride.Value, entity.GetComponent<UnitComponent>().value));
                unit.View.transform.position = destination;
                cellView = movement.targetCell;
                cellView.UnitView = unit.View;

                if (stride.Value.Current == 0 || cellView.Position == movement.destination)
                    MovementEnd(entity);
                else
                    FindNextCell(entity);
            }
        }

        private static void MovementEnd(Entity entity)
        {
            entity.GetComponent<MovementComponent>().onMovementComplete?.Invoke();
            entity.RemoveComponent<MovementComponent>();
        }

        public void Dispose()
        {
        }
    }
}