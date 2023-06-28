using System;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Extensions;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Item;
using TurnBasedRPG.View;
using TurnBasedRPG.View.Ui;
using TurnBasedRPG.View.Unit;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace TurnBasedRPG.Model.Unit
{
    public abstract class AUnit
    {
        protected readonly SignalBus SignalBus;
        private readonly Entity _entity;
        private readonly UnitConfig _config;
        private UnitView _view;
        private int _initiative;

        protected AUnit(Entity entity, UnitConfig config, SignalBus signalBus)
        {
            _entity = entity;
            _config = config;
            SignalBus = signalBus;
        }

        public Entity Entity => _entity;
        public UnitConfig Config => _config;
        public UnitView View => _view;
        public UiUnitView UiView { get; set; }
        public bool IsPlayer { get; set; }

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

        public void MoveTo(CellView targetCell, Action onMovementComplete = null)
        {
            if (_entity.Has<MovementComponent>())
                return;

            var animator = _entity.GetComponent<AnimatorComponent>().Value;
            animator.SetState(EAnimatorState.Move);
            onMovementComplete += () => { animator.SetState(EAnimatorState.IdleCombat); };

            _entity.AddComponent<MovementComponent>() = new MovementComponent
            {
                destination = targetCell.Position,
                onMovementComplete = onMovementComplete
            };
        }

        public void MoveTo(AUnit targetUnit, Action onMovementComplete = null)
        {
            if (_entity.Has<MovementComponent>())
                return;

            var animator = _entity.GetComponent<AnimatorComponent>().Value;
            animator.SetState(EAnimatorState.Move);
            onMovementComplete += () => { animator.SetState(EAnimatorState.IdleCombat); };

            var targetCell = targetUnit.Entity.GetComponent<UnitComponent>().cellView;

            _entity.AddComponent<MovementComponent>() = new MovementComponent
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
            var unitCell = Entity.GetComponent<UnitComponent>().cellView;
            var targetCell = target.Entity.GetComponent<UnitComponent>().cellView;
            var distanceToTarget = Vector2.Distance(targetCell.Position, unitCell.Position);

            var result = Mathf.RoundToInt(distanceToTarget) <= weapon.Range;
            if (!result)
                Debug.Log($"[Check Range] distance: {distanceToTarget}, weapon range: {weapon.Range}");

            return result;
        }

        public AWeapon GetEquippedWeapon()
        {
            // todo debug
            var config = Config.items[0];
            var weapon = new SimpleWeapon(config);

            return weapon;
        }

        public void Attack(AUnit target, Action onAttackComplete = null)
        {
            Entity.AddComponent<AttackComponent>() = new AttackComponent
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


                view.WeaponText.text = $"{weapon.Name}\nDamage {weapon.GetDamageText()}";
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
    }
}