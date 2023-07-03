using System;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Services.Facade;
using TurnBasedRPG.Signals;
using TurnBasedRPG.View;
using UniRx;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Services
{
    public class UiService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly CanvasView _canvasView;
        private readonly BattleService _battleService;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public UiService(SignalBus signalBus, CanvasView canvasView, BattleService battleService)
        {
            _signalBus = signalBus;
            _canvasView = canvasView;
            _battleService = battleService;
        }

        public void Initialize()
        {
            _signalBus.GetStream<StrideChangedSignal>()
                .Subscribe(OnStrideChanged)
                .AddTo(_disposable);

            _signalBus.GetStream<UnitUpdatedSignal>()
                .Subscribe(OnUnitUpdated)
                .AddTo(_disposable);

            _signalBus.GetStream<AttacksLeftChangedSignal>()
                .Subscribe(OnAttacksLeftChanged)
                .AddTo(_disposable);

            _canvasView.NextTurnButton.OnClickAsObservable()
                .Subscribe(_ => _battleService.NextTurn())
                .AddTo(_disposable);

            // debug
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(x => _battleService.NextTurn())
                .AddTo(_disposable);
            //
            
            FloatingText.UiService = this;
        }

        private void OnStrideChanged(StrideChangedSignal signal)
            => signal.unit.UiView.StrideText.text = $"Stride: {signal.value.PercentText}";

        private void OnUnitUpdated(UnitUpdatedSignal signal)
        {
            var unit = signal.unit;
            var vita = unit.Entity.GetComponent<VitaComponent>();
            var energy = unit.Entity.GetComponent<EnergyComponent>();
            var stride = unit.Entity.GetComponent<StrideComponent>();
            var attacksLeft = unit.Entity.GetComponent<AttacksLeftComponent>();
            var uiView = unit.UiView;

            uiView.Icon.sprite = unit.Icon;
            uiView.NameText.text = unit.Name;
            uiView.VitaSlider.value = vita.Value.Percent;
            uiView.VitaText.text = vita.Value.PercentText;
            uiView.EnergySlider.value = energy.Value.Percent;
            uiView.EnergyText.text = energy.Value.PercentText;
            uiView.StrideText.text = $"Stride: {stride.Value.PercentText}";
            uiView.AttacksText.text = $"Attacks: {attacksLeft.Value.PercentText}";
        }

        private void OnAttacksLeftChanged(AttacksLeftChangedSignal signal)
        {
            //todo _canvasView.UnitView.AttacksText.text = $"Attacks: {signal.value.PercentText}";
        }

        public void ShowFloatingText(Vector3 position, string text, Color color, float speed, float size)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => _disposable?.Dispose();
    }
}