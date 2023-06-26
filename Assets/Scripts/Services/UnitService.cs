using System.Collections.Generic;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.View;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Services
{
    public class UnitService
    {
        private readonly World _world;
        private readonly SceneData _sceneData;
        private readonly BattleService _battleService;
        private readonly SignalBus _signalBus;

        public List<AUnit> PlayerUnits { get; private set; } = new List<AUnit>();
        public List<AUnit> EnemyUnits { get; private set; } = new List<AUnit>();

        public UnitService(World world,
            SceneData sceneData,
            BattleService battleService,
            SignalBus signalBus)
        {
            _world = world;
            _sceneData = sceneData;
            _battleService = battleService;
            _signalBus = signalBus;
        }

        public AUnit CreateUnit(UnitConfig config, Vector2Int cellPosition, bool isPlayer = false)
        {
            var entity = _world.CreateEntity();

            AUnit unit;
            if (isPlayer)
            {
                unit = new PlayerUnit(entity, config, _signalBus);
                PlayerUnits.Add(unit);
            }
            else
            {
                unit = new EnemyUnit(entity, config, _signalBus);
                EnemyUnits.Add(unit);
            }
            
            var cell = _battleService.GetCellByPosition(cellPosition.x, cellPosition.y);

            var position = cell.transform.position;
            var rotation = Quaternion.Euler(0,
                isPlayer ? 0f : 180f,
                0);
            unit.CreateView(position, rotation, _sceneData.UnitContainer);
            unit.InitializeView();

            cell.UnitView = unit.View;

            ref var unitComponent = ref entity.AddComponent<UnitComponent>();

            unitComponent.Unit = unit;
            unitComponent.CellView = cell;

            entity.AddComponent<AnimatorComponent>().Value = unit.View.Animator;
            
            entity.AddComponent<VitaComponent>().Value = new CurrentMax(config.vita);
            entity.AddComponent<EnergyComponent>().Value = new CurrentMax(config.energy);
            entity.AddComponent<StrideComponent>().Value = new CurrentMax(config.stride);
            entity.AddComponent<AttacksLeftComponent>().Value = new CurrentMax(config.attacks);

            return unit;
        }

        public ItemConfig GetEquippedWeapon(Entity entity)
        {
            // todo debug
            var unit = entity.GetComponent<UnitComponent>().Unit;
            var weapon = unit.Config.items[0];

            return weapon;
        }
    }
}