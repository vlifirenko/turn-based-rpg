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
                ref var attacksLeft = ref entity.GetComponent<AttacksLeftComponent>();
                if (attacksLeft.Value.Current > 0 && CheckRange(entity))
                {
                    Attack(entity);
                    attacksLeft.Value.Current -= 1;
                    _signalBus.Fire(new AttacksLeftChangedSignal(attacksLeft.Value));
                }

                entity.RemoveComponent<AttackComponent>();
            }
        }

        private void Attack(Entity entity)
        {
            var config = entity.GetComponent<UnitComponent>().Unit.Config;
            var attack = entity.GetComponent<AttackComponent>();
            var might = _diceService.RollDice(EDice.D100) + config.might;
            var defence = attack.Target.Entity.GetComponent<UnitComponent>().Unit.Config.defence;

            if (might >= defence)
            {
                var damage = _diceService.RollDice(EDice.D4, 2);
                ref var targetVita = ref attack.Target.Entity.GetComponent<VitaComponent>();

                targetVita.Value.Current = Mathf.Clamp(targetVita.Value.Current - damage, 0, targetVita.Value.Max);
                _signalBus.Fire(new VitaChangedSignal(attack.Target));

                UnityEngine.Debug.Log($"Attack: {might}/{defence}, Damage: {damage}");
            }
            else
            {
                UnityEngine.Debug.Log($"Miss {might}/{defence}");
            }
        }

        private bool CheckRange(Entity entity)
        {
            var weapon = _unitService.GetEquippedWeapon(entity);
            var unitCell = entity.GetComponent<UnitComponent>().CellView;
            var targetEntity = entity.GetComponent<AttackComponent>().Target;
            var targetCell = targetEntity.Entity.GetComponent<UnitComponent>().CellView;
            var distanceToTarget = Vector2.Distance(targetCell.Position, unitCell.Position);

            var result = Mathf.RoundToInt(distanceToTarget) <= weapon.range;
            if (!result)
                UnityEngine.Debug.Log($"Distance: {distanceToTarget}, weapon range: {weapon.range}");

            return result;
        }

        public void Dispose()
        {
        }
    }
}