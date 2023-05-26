using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.View;
using UnityEngine;

namespace TurnBasedRPG.Services
{
    public class UnitService
    {
        private readonly World _world;
        private readonly SceneData _sceneData;

        public UnitService(World world,
            SceneData sceneData)
        {
            _world = world;
            _sceneData = sceneData;
        }

        public Entity CreateUnit(UnitConfig config)
        {
            var entity = _world.CreateEntity();
            var view = Object.Instantiate(config.prefab, _sceneData.UnitContainer);

            ref var unit = ref entity.AddComponent<UnitComponent>();
            
            unit.Config = config;
            unit.View = view;

            entity.AddComponent<VitaComponent>().Value = unit.Config.vita;

            return entity;
        }
    }
}