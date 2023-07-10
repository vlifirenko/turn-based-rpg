using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Systems.Battle;
using TurnBasedRPG.Ecs.Systems.Debug;
using TurnBasedRPG.Ecs.Systems.Unit;
using TurnBasedRPG.Services;
using TurnBasedRPG.Signals;
using TurnBasedRPG.View;
using TurnBasedRPG.View.Canvas;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Installers
{
    public class EcsInstaller : MonoInstaller
    {
        [SerializeField] private SceneData sceneData; 
        [SerializeField] private CanvasView canvasView; 
        [SerializeField] private CharactersCanvasView charactersCanvasView; 
        
        private World _world;
        private SystemsGroup _systems;

        public override void InstallBindings()
        {
            BindSignals();
            BindWorld();
            BindScene();
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

            Container.BindInstance(_world);
            
            _systems = _world.CreateSystemsGroup();
            _world.AddSystemsGroup(order: 0, _systems);
        }

        private void BindScene()
        {
            Container.BindInstance(sceneData);
            Container.BindInstance(canvasView);
            Container.BindInstance(charactersCanvasView);
        }

        private void BindSystems()
        {
            // initializers
            BindInitializer<BattleInitializer>(); // first create map and then units FIX
            BindInitializer<UnitInitializer>();
            
            // systems
            BindSystem<SelectCellSystem>();
            BindSystem<AttackSystem>();
            BindSystem<EnemyTurnSystem>();
            BindSystem<MovementSystem>();
            BindSystem<DebugSystem>();
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<UiService>().AsSingle();
            Container.Bind<UnitService>().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleService>().AsSingle();
            Container.Bind<DiceService>().AsSingle();
        }

        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<StrideChangedSignal>();
            Container.DeclareSignal<SetActiveUnitSignal>();
            Container.DeclareSignal<AttacksLeftChangedSignal>();
            Container.DeclareSignal<NextTurnSignal>();
            Container.DeclareSignal<UnitUpdatedSignal>();
            Container.DeclareSignal<InventoryUpdatedSignal>();
        }
        
        private void BindInitializer<T>() where T : class, IInitializer
        {
            Container.Bind<T>().AsSingle();
            _systems.AddInitializer(Container.Resolve<T>());
        }

        private void BindSystem<T>() where T : class, ISystem
        {
            Container.Bind<T>().AsSingle();
            _systems.AddSystem(Container.Resolve<T>());
        }
    }
}