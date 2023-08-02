using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.View.Unit;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class ARPGAttackSystem : ISystem
    {
        public World World { get; set; }
        private readonly GlobalConfigInstaller.LayersConfig _layersConfig;
        private Filter _filter;

        public ARPGAttackSystem(GlobalConfigInstaller.LayersConfig layersConfig)
        {
            _layersConfig = layersConfig;
        }

        public void OnAwake() =>
            _filter = World.Filter
                .With<ARPGAttackComponent>()
                .Without<DeadComponent>();

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                var unit = entity.GetComponent<UnitComponent>().value;
                var position = unit.View.transform.position;

                var colliders = Physics.OverlapSphere(position, 1.5f, _layersConfig.damageable);
                UnityEngine.Debug.Log(colliders.Length);
                foreach (var collider in colliders)
                {
                    if (collider.transform == unit.View.transform)
                        continue;

                    var targetView = collider.GetComponent<UnitView>();
                    var targetEntity = targetView.Unit.Entity;
                    var damage = new Damage();

                    if (!targetEntity.Has<DamageComponent>())
                        targetEntity.AddComponent<DamageComponent>().Value = damage;
                }

                entity.RemoveComponent<ARPGAttackComponent>();
            }
        }

        public void Dispose()
        {
        }
    }
}