using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class DamageSystem : ISystem
    {
        public World World { get; set; }
        private Filter _filter;

        public void OnAwake()
        {
            _filter = World
                .Filter
                .With<DamageComponent>()
                .Without<DeadComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                UnityEngine.Debug.Log("damage");

                entity.RemoveComponent<DamageComponent>();
            }
        }

        public void Dispose()
        {
        }
    }
}