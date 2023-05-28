using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.View;
using UnityEngine;

namespace TurnBasedRPG.Services
{
    public class UnitService
    {
        private readonly World _world;
        private readonly SceneData _sceneData;
        private readonly BattleService _battleService;

        public UnitService(World world,
            SceneData sceneData,
            BattleService battleService)
        {
            _world = world;
            _sceneData = sceneData;
            _battleService = battleService;
        }

        public Entity CreateUnit(UnitConfig config, Vector2Int cellPosition)
        {
            var entity = _world.CreateEntity();
            var cell = _battleService.GetCellByPosition(cellPosition.x, cellPosition.y);

            var position = cell.transform.position;
            position.y += 1f;
            var view = Object.Instantiate(config.prefab, position, Quaternion.identity, _sceneData.UnitContainer);

            cell.UnitView = view;
            view.Entity = entity;

            ref var unit = ref entity.AddComponent<UnitComponent>();

            unit.Config = config;
            unit.View = view;

            entity.AddComponent<VitaComponent>().Value = new CurrentMax
            {
                Current = unit.Config.vita,
                Max = unit.Config.vita
            };
            entity.AddComponent<EnergyComponent>().Value = new CurrentMax
            {
                Current = unit.Config.energy,
                Max = unit.Config.energy
            };

            return entity;
        }
    }
}