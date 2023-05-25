using System;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Systems.Unit;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG
{
    public class EcsService : IInitializable, ITickable, IDisposable
    {
        private readonly World _world;
        private readonly SystemsGroup _systems;
        
        public EcsService(SystemsGroup systems, World world)
        {
            _systems = systems;
            _world = world;
        }

        public void Initialize()
        {
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