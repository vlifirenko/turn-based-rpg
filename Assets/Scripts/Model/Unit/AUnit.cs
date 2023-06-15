﻿using System;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Extensions;
using TurnBasedRPG.Model.Config;
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

        protected AUnit(Entity entity, UnitConfig config, SignalBus signalBus)
        {
            _entity = entity;
            _config = config;
            SignalBus = signalBus;
        }

        public Entity Entity => _entity;
        public UnitConfig Config => _config;
        public UnitView View => _view;

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

        public void MoveTo(Vector2Int destination, Action onMovementComplete = null)
        {
            if (_entity.Has<MovementComponent>())
                return;

            _entity.GetComponent<AnimatorComponent>().Value.SetState(EAnimatorState.Move);
            _entity.AddComponent<MovementComponent>() = new MovementComponent
            {
                destination = destination,
                OnMovementComplete = onMovementComplete
            };
        }
    }
}