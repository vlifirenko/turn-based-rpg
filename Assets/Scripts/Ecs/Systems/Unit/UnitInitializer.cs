using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.View.Unit;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class UnitInitializer : IInitializer 
    {
        public World World { get; set; }

        private UnitView _debugUnitView;
        
        [Inject]
        public void Construct(UnitView view)
        {
            _debugUnitView = view;
        }

        public void OnAwake()
        {
            var entity = World.CreateEntity();

            ref var unit  = ref entity.AddComponent<UnitComponent>();

            unit.Config = _debugUnitView.Config;
            unit.View = _debugUnitView;
            
            Debug.Log(unit);
        }

        public void Dispose() { }
    }
}