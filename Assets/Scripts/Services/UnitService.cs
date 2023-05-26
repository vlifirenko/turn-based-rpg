using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model.Config;
using UnityEngine;

namespace TurnBasedRPG.Services
{
    public class UnitService
    {
        private readonly World _world;

        public UnitService(World world)
        {
            _world = world;
        }

        public Entity CreateUnit(UnitConfig config)
        {
            var entity = _world.CreateEntity();
            var view = Object.Instantiate(config.prefab);

            ref var unit = ref entity.AddComponent<UnitComponent>();
            
            unit.Config = config;
            unit.View = view;

            entity.AddComponent<VitaComponent>().Value = unit.Config.vita;

            return entity;
        }
    }
}