using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model.Enums;
using TurnBasedRPG.Services;
using TurnBasedRPG.Signals;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class AttackSystem : ISystem
    {
        private readonly DiceService _diceService;
        private readonly SignalBus _signalBus;
        public World World { get; set; }

        private Filter _filter;

        public AttackSystem(DiceService diceService, SignalBus signalBus)
        {
            _diceService = diceService;
            _signalBus = signalBus;
        }

        public void OnAwake() => _filter = World.Filter.With<AttackComponent>();

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                Attack(entity);
                entity.RemoveComponent<AttackComponent>();
            }
        }

        private void Attack(Entity entity)
        {
            var config = entity.GetComponent<UnitComponent>().Config;
            var attack = entity.GetComponent<AttackComponent>();
            var might = _diceService.RollDice(EDice.D100) + config.might;
            var defence = attack.target.GetComponent<UnitComponent>().Config.defence;

            var success = false;
            if (might >= defence)
            {
                var damage = _diceService.RollDice(EDice.D4, 2);
                ref var targetVita = ref attack.target.GetComponent<VitaComponent>();

                targetVita.Value.Current = Mathf.Clamp(targetVita.Value.Current - damage, 0, targetVita.Value.Max);
                _signalBus.Fire(new VitaChangedSignal(attack.target));
                
                UnityEngine.Debug.Log($"Attack: {might}/{defence}, Damage: {damage}");
            }
            else
            {
                UnityEngine.Debug.Log($"Miss {might}/{defence}");
            }
        }

        public void Dispose()
        {
        }
    }
}