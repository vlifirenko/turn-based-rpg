using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model.Enums;
using TurnBasedRPG.Services;
using TurnBasedRPG.Signals;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Ecs.Systems.Battle
{
    public class AttackSystem : ISystem
    {
        private readonly DiceService _diceService;
        private readonly SignalBus _signalBus;
        private readonly UnitService _unitService;
        public World World { get; set; }

        private Filter _filter;

        public AttackSystem(DiceService diceService, SignalBus signalBus, UnitService unitService)
        {
            _diceService = diceService;
            _signalBus = signalBus;
            _unitService = unitService;
        }

        public void OnAwake() => _filter = World.Filter.With<AttackComponent>();

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                var unit = entity.GetComponent<UnitComponent>().unit;
                var attack = entity.GetComponent<AttackComponent>();
                ref var attacksLeft = ref entity.GetComponent<AttacksLeftComponent>();
                
                if (attacksLeft.Value.Current > 0 && unit.CheckRange(attack.target))
                {
                    Attack(entity);
                    attacksLeft.Value.Current -= 1;
                    _signalBus.Fire(new AttacksLeftChangedSignal(attacksLeft.Value));
                }

                attack.onComplete?.Invoke();
                entity.RemoveComponent<AttackComponent>();
            }
        }

        private void Attack(Entity entity)
        {
            var config = entity.GetComponent<UnitComponent>().unit.Config;
            var attack = entity.GetComponent<AttackComponent>();
            var might = _diceService.RollDice(EDice.D100) + config.might;
            var defence = attack.target.Entity.GetComponent<UnitComponent>().unit.Config.defence;

            if (might >= defence)
            {
                var damage = _diceService.RollDice(EDice.D4, 2);
                ref var targetVita = ref attack.target.Entity.GetComponent<VitaComponent>();

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