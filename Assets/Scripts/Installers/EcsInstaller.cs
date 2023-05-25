using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Systems.Unit;
using TurnBasedRPG.Services;
using Zenject;

namespace TurnBasedRPG.Installers
{
    public class EcsInstaller : MonoInstaller
    {
        private World _world;
        private SystemsGroup _systems;

        public override void InstallBindings()
        {
            BindWorld();
            BindDebug();
            BindServices();
            BindSystems();
            
            Container
                .BindInterfacesAndSelfTo<EcsService>()
                .AsSingle()
                .WithArguments(_systems);
        }

        private void BindWorld()
        {
            _world = World.Create();
            _world.UpdateByUnity = false;

            Container.Bind<World>().FromInstance(_world);
            
            _systems = _world.CreateSystemsGroup();
            _world.AddSystemsGroup(order: 0, _systems);
        }

        private void BindDebug()
        {
        }

        private void BindSystems()
        {
            // initializers
            BindInitializer<UnitInitializer>();
            
            // systems
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<UnitService>().AsSingle().NonLazy();
        }
        
        private void BindInitializer<T>() where T : class, IInitializer
        {
            Container.Bind<T>().AsSingle().NonLazy();
            _systems.AddInitializer(Container.Resolve<T>());
        }

        private void BindSystem<T>() where T : class, ISystem
        {
            Container.Bind<T>().AsSingle().NonLazy();
            _systems.AddSystem(Container.Resolve<T>());
        }
    }
}