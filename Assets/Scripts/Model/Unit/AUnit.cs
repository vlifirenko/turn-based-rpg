using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Extensions;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Enums;
using TurnBasedRPG.Model.Item;
using TurnBasedRPG.Model.Map;
using TurnBasedRPG.View.Ui;
using TurnBasedRPG.View.Unit;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace TurnBasedRPG.Model.Unit
{
    public abstract class AUnit : ICellItem
    {
        protected readonly SignalBus SignalBus;
        private readonly Entity _entity;
        private readonly UnitConfig _config;
        private UnitView _view;
        private int _initiative;

        public Entity Entity => _entity;
        public UnitView View => _view;
        public Cell Cell { get; set; }
        public Sprite Icon { get; }
        public string Name { get; }
        public UiUnitView UiView { get; set; }
        public bool IsPlayer { get; set; }
        public int Defence { get; }
        public int Might { get; }
        public int DamageBonus { get; }

        protected abstract EItemSlot[] AvailableSlots { get; }
        private readonly Dictionary<EItemSlot, AEquipment> _equipment = new();

        protected AUnit(Entity entity, UnitConfig config, SignalBus signalBus)
        {
            _entity = entity;
            _config = config;
            SignalBus = signalBus;

            Name = config.name;
            Icon = config.icon;
            Defence = config.defence;
            Might = config.might;
            DamageBonus = config.damageBonus;

            foreach (var slot in AvailableSlots)
                _equipment.Add(slot, null);
        }

        public int Initiative
        {
            get => _initiative;
            set
            {
                if (_entity.Has<InitiativeComponent>())
                    _entity.GetComponent<InitiativeComponent>().Value = value;
                else
                    _entity.AddComponent<InitiativeComponent>().Value = value;
                _initiative = value;
            }
        }
        
        public void CreateView(Vector3 position, Quaternion rotation, Transform parent)
        {
            _view = Object.Instantiate(_config.prefab, position, rotation, parent);
            _view.Unit = this;
        }

        public void InitializeView()
        {
            _view.Animator.SetState(EAnimatorState.IdleCombat);
        }

        public virtual void StartTurn()
        {
        }

        public void MoveTo(Cell targetCell, float range = 0, Action onMovementComplete = null)
        {
            if (_entity.Has<TBMovementComponent>())
                return;

            var animator = _entity.GetComponent<AnimatorComponent>().Value;
            animator.SetState(EAnimatorState.Move);
            onMovementComplete += () => { animator.SetState(EAnimatorState.IdleCombat); };

            _entity.AddComponent<TBMovementComponent>() = new TBMovementComponent
            {
                destination = targetCell.Position,
                onMovementComplete = onMovementComplete,
                range = range
            };
        }

        public void MoveTo(AUnit targetUnit, Action onMovementComplete = null)
        {
            if (_entity.Has<TBMovementComponent>())
                return;

            var animator = _entity.GetComponent<AnimatorComponent>().Value;
            animator.SetState(EAnimatorState.Move);
            onMovementComplete += () => { animator.SetState(EAnimatorState.IdleCombat); };

            var targetCell = targetUnit.Cell;

            _entity.AddComponent<TBMovementComponent>() = new TBMovementComponent
            {
                destination = targetCell.Position,
                onMovementComplete = onMovementComplete
            };
        }

        public void Select() => _view.Selected.SetActive(true);

        public void Deselect() => _view.Selected.SetActive(false);

        public bool CheckRange(AUnit target)
        {
            var weapon = GetEquippedWeapon();
            var distanceToTarget = Vector2.Distance(target.Cell.Position, Cell.Position);

            var result = Mathf.RoundToInt(distanceToTarget) <= weapon.Range;
            if (!result)
                Debug.Log($"[Check Range] distance: {distanceToTarget}, weapon range: {weapon.Range}");

            return result;
        }

        public int GetRange() => GetEquippedWeapon().Range;

        public AWeapon GetEquippedWeapon()
        {
            // todo debug
            var config = _config.items[0];
            var weapon = new SimpleWeapon(config)
            {
                Owner = this
            };

            return weapon;
        }

        public void Attack(AUnit target, Action onAttackComplete = null)
        {
            Entity.AddComponent<TBAttackComponent>() = new TBAttackComponent
            {
                target = target,
                onComplete = onAttackComplete
            };
        }

        public void Hover(AUnit activeUnit = null)
        {
            UiView.Selected.SetActive(true);
            if (activeUnit != null)
            {
                var weapon = activeUnit.GetEquippedWeapon();
                var view = activeUnit.UiView;
                var chance = activeUnit.GetHitChance(this);

                //chance = Mathf.FloorToInt(chance * 100f);
                view.WeaponText.text =
                    $"{weapon.Name}\nDamage {weapon.GetDamageText()}\nRange {weapon.Range}\nChance {chance}%";
                view.WeaponPanel.SetActive(true);
            }
        }

        public void Unhover(AUnit activeUnit = null)
        {
            UiView.Selected.SetActive(false);
            if (activeUnit != null)
            {
                activeUnit.UiView.WeaponPanel.SetActive(false);
            }
        }

        private int GetHitChance(AUnit target)
        {
            var chance = 100 - target.Defence + Might;
            return chance;
        }

        public AItem Equip(AEquipment item)
        {
            AItem unequippedItem = null;
            
            if (_equipment[item.Slot] != null)
                unequippedItem = _equipment[item.Slot];
            _equipment[item.Slot] = item;

            return unequippedItem;
        }

        public void Unequip(AEquipment item) => _equipment[item.Slot] = null;

        public AItem GetEquipmentInSlot(EItemSlot slot) => _equipment[slot];
    }
}