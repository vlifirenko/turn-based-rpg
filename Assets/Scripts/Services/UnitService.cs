using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model.Config;
using UnityEngine;

namespace TurnBasedRPG.Services
{
    public class UnitService
    {
        private World _world;

        public UnitService(World world)
        {
            _world = world;
        }

        public Entity CreateUnit(UnitConfig config)
        {
            var entity = _world.CreateEntity();

            ref var unit = ref entity.AddComponent<UnitComponent>();
            unit.Config = config;
            // todo unit.View = _debugUnitView;

            entity.AddComponent<VitaComponent>().Value = unit.Config.vita;

            Debug.Log(entity.GetComponent<VitaComponent>());

            return entity;
        }
    }
}