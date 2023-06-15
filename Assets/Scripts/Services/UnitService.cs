using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Extensions;
using TurnBasedRPG.Model;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Unit;
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

        public AUnit CreateUnit(UnitConfig config, Vector2Int cellPosition, bool isPlayer = false)
        {
            var entity = _world.CreateEntity();

            AUnit unit;
            if (isPlayer)
                unit = new PlayerUnit(entity);
            else
                unit = new EnemyUnit(entity);
            
            var cell = _battleService.GetCellByPosition(cellPosition.x, cellPosition.y);

            var position = cell.transform.position;
            var rotation = Quaternion.Euler(0,
                isPlayer ? 0f : 180f,
                0);
            var view = Object.Instantiate(config.prefab, position, rotation, _sceneData.UnitContainer);

            cell.UnitView = view;
            view.Unit = unit;

            ref var unitComponent = ref entity.AddComponent<UnitComponent>();

            unitComponent.Unit = unit;
            unitComponent.config = config;
            unitComponent.view = view;
            unitComponent.cellView = cell;

            entity.AddComponent<AnimatorComponent>().Value = view.Animator;
            // test animator
            view.Animator.SetState(EAnimatorState.IdleCombat);

            entity.AddComponent<VitaComponent>().Value = new CurrentMax(unitComponent.config.vita);
            entity.AddComponent<EnergyComponent>().Value = new CurrentMax(unitComponent.config.energy);
            entity.AddComponent<StrideComponent>().Value = new CurrentMax(unitComponent.config.stride);
            entity.AddComponent<AttacksLeftComponent>().Value = new CurrentMax(unitComponent.config.attacks);

            return unit;
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