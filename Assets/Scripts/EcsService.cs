using System;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Systems.Unit;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG
{
    public class EcsService : IInitializable, ITickable, IDisposable
    {
        private World _world;
        private SystemsGroup _systems;

        public void Initialize()
        {
            _world = World.Create();
            _world.UpdateByUnity = false;
            
            _systems = _world.CreateSystemsGroup();
            
            _systems.AddInitializer(new UnitInitializer());
            
            //systemsGroup.AddSystem(newSystem);

            _world.AddSystemsGroup(order: 0, _systems);
        }

        public void Tick()
        {
            _world.Update(Time.deltaTime);
            _world.Commit();
        }

        public void Dispose()
        {
            _world.RemoveSystemsGroup(_systems);

        }
    }
}