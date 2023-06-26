﻿using System;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Extensions;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.View;
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
            Debug.Log("default start turn");
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
                OnMovementComplete = onMovementComplete
            };
        }
        
        public void MoveTo(AUnit targetUnit, Action onMovementComplete = null)
        {
            if (_entity.Has<MovementComponent>())
                return;

            var animator = _entity.GetComponent<AnimatorComponent>().Value;
            animator.SetState(EAnimatorState.Move);
            onMovementComplete += () => { animator.SetState(EAnimatorState.IdleCombat); };

            var targetCell = targetUnit.Entity.GetComponent<UnitComponent>().CellView;
            
            _entity.AddComponent<MovementComponent>() = new MovementComponent
            {
                destination = targetCell.Position,
                OnMovementComplete = onMovementComplete
            };
        }

        public void Select() => _view.Selected.SetActive(true);

        public void Deselect() => _view.Selected.SetActive(false);

        public bool CheckRange(AUnit target)
        {
            var weapon = GetEquippedWeapon();
            var unitCell = Entity.GetComponent<UnitComponent>().CellView;
            var targetCell = target.Entity.GetComponent<UnitComponent>().CellView;
            var distanceToTarget = Vector2.Distance(targetCell.Position, unitCell.Position);

            var result = Mathf.RoundToInt(distanceToTarget) <= weapon.range;
            if (!result)
                UnityEngine.Debug.Log($"[Check Range] distance: {distanceToTarget}, weapon range: {weapon.range}");

            return result;
        }
        
        public ItemConfig GetEquippedWeapon()
        {
            // todo debug
            var weapon = Config.items[0];

            return weapon;
        }
    }
}