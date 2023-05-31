using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class AttackSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                UnityEngine.Debug.Log("attack");
                entity.RemoveComponent<AttackComponent>();
            }
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<AttackComponent>();
        }

        public void Dispose() { }
    }
}