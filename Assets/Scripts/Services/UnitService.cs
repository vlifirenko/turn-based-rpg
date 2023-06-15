using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Extensions;
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

        public Entity CreateUnit(UnitConfig config, Vector2Int cellPosition, bool isPlayer = false)
        {
            var entity = _world.CreateEntity();
            var cell = _battleService.GetCellByPosition(cellPosition.x, cellPosition.y);

            var position = cell.transform.position;
            var rotation = Quaternion.Euler(0,
                isPlayer ? 0f : 180f,
                0);
            var view = Object.Instantiate(config.prefab, position, rotation, _sceneData.UnitContainer);

            cell.UnitView = view;
            view.Entity = entity;

            ref var unit = ref entity.AddComponent<UnitComponent>();

            unit.config = config;
            unit.view = view;
            unit.cellView = cell;
            
            // test animator
            view.Animator.SetState(EAnimatorState.IdleCombat);

            entity.AddComponent<VitaComponent>().Value = new CurrentMax(unit.config.vita);
            entity.AddComponent<EnergyComponent>().Value = new CurrentMax(unit.config.energy);
            entity.AddComponent<StrideComponent>().Value = new CurrentMax(unit.config.stride);
            entity.AddComponent<AttacksLeftComponent>().Value = new CurrentMax(unit.config.attacks);

            return entity;
        }

        public ItemConfig GetEquippedWeapon(Entity entity)
        {
            // todo debug
            var unit = entity.GetComponent<UnitComponent>();
            var weapon = unit.config.items[0];

            return weapon;
        }
    }
}