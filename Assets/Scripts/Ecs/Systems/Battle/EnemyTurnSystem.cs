using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.Services;
using TurnBasedRPG.Signals;
using TurnBasedRPG.View;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Ecs.Systems.Battle
{
    public class EnemyTurnSystem : ISystem
    {
        private readonly BattleService _battleService;
        private readonly UnitService _unitService;
        private readonly SignalBus _signalBus;
        public World World { get; set; }

        private Filter _filter;

        public EnemyTurnSystem(BattleService battleService, UnitService unitService, SignalBus signalBus)
        {
            _battleService = battleService;
            _unitService = unitService;
            _signalBus = signalBus;
        }

        public void OnAwake() => _filter = World.Filter.With<EnemyTurnComponent>();

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var enemyTurn = ref entity.GetComponent<EnemyTurnComponent>();
                if (enemyTurn.stage == EEnemyTurnStage.None)
                {
                    enemyTurn.stage = EEnemyTurnStage.LookingForTarget;
                    LookingForTarget(entity);
                }
            }
        }

        private void LookingForTarget(Entity entity)
        {
            var unit = entity.GetComponent<UnitComponent>().Unit;
            ref var enemyTurn = ref entity.GetComponent<EnemyTurnComponent>();
            
            AUnit nearestUnit = null;
            CellView cellView = null;
            var minDistance = Mathf.Infinity;
            
            foreach (var playerUnit in _unitService.PlayerUnits)
            {
                var distance = Vector3.Distance(playerUnit.View.transform.position, unit.View.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestUnit = playerUnit;
                    cellView = playerUnit.Entity.GetComponent<UnitComponent>().CellView;
                }
            }

            enemyTurn.target = nearestUnit;
            unit.MoveTo(cellView.Position, () =>
            {
                _signalBus.Fire(new NextTurnSignal());
            });
        }

        public void Dispose()
        {
        }
    }
}