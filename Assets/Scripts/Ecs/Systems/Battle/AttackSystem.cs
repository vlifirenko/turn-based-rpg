using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model.Enums;
using TurnBasedRPG.Services;
using TurnBasedRPG.Services.Facade;
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

        public void OnAwake() => _filter = World.Filter.With<TBAttackComponent>();

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                var unit = entity.GetComponent<UnitComponent>().value;
                var attack = entity.GetComponent<TBAttackComponent>();
                ref var attacksLeft = ref entity.GetComponent<AttacksLeftComponent>();
                
                if (attacksLeft.Value.Current > 0 && unit.CheckRange(attack.target))
                {
                    Attack(entity);
                    attacksLeft.Value.Current -= 1;
                    _signalBus.Fire(new AttacksLeftChangedSignal(attacksLeft.Value));
                }

                attack.onComplete?.Invoke();
                entity.RemoveComponent<TBAttackComponent>();
            }
        }

        private void Attack(Entity entity)
        {
            var unit = entity.GetComponent<UnitComponent>().value;
            var attack = entity.GetComponent<TBAttackComponent>();
            var attackRate = _diceService.RollDice(EDice.D100) + unit.Might;
            var defence = attack.target.Entity.GetComponent<UnitComponent>().value.Defence;
            var position = unit.View.transform.position;

            if (attackRate >= defence)
            {
                var damage = _diceService.RollDice(EDice.D4, 2);
                ref var targetVita = ref attack.target.Entity.GetComponent<VitaComponent>();

                targetVita.Value.Current = Mathf.Clamp(targetVita.Value.Current - damage, 0, targetVita.Value.Max);
                _signalBus.Fire(new UnitUpdatedSignal(attack.target));

                FloatText.Show(position, $"Hit: {attackRate}/{defence}, Damage: {damage}");
                UnityEngine.Debug.Log($"Hit: {attackRate}/{defence}, Damage: {damage}");
            }
            else
            {
                FloatText.Show(position, $"Miss {attackRate}/{defence}");
                UnityEngine.Debug.Log($"Miss {attackRate}/{defence}");
            }
        }
        
        public void Dispose()
        {
        }
    }    
}