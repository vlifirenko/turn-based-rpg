using System;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Signals;
using TurnBasedRPG.View;
using UniRx;
using Zenject;

namespace TurnBasedRPG.Services
{
    public class UiService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly CanvasView _canvasView;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public UiService(SignalBus signalBus, CanvasView canvasView)
        {
            _signalBus = signalBus;
            _canvasView = canvasView;
        }

        public void Initialize()
        {
            _signalBus.GetStream<StrideChangedSignal>()
                .Subscribe(OnStrideChanged)
                .AddTo(_disposable);
            
            _signalBus.GetStream<SetActiveUnitSignal>()
                .Subscribe(OnSetActiveUnit)
                .AddTo(_disposable);
            
            _signalBus.GetStream<AttacksLeftChangedSignal>()
                .Subscribe(OnAttacksLeftChanged)
                .AddTo(_disposable);
        }

        private void OnStrideChanged(StrideChangedSignal signal) 
            => _canvasView.ActiveUnit.StrideText.text = $"Stride: {signal.value.PercentText}";

        private void OnSetActiveUnit(SetActiveUnitSignal signal)
        {
            var entity = signal.activeUnit;
            var config = entity.GetComponent<UnitComponent>().config;
            var vita = entity.GetComponent<VitaComponent>();
            var energy = entity.GetComponent<EnergyComponent>();
            var stride = entity.GetComponent<StrideComponent>();
            var attacksLeft = entity.GetComponent<AttacksLeftComponent>();
            var uiView = _canvasView.ActiveUnit;

            uiView.Icon.sprite = config.icon;
            uiView.NameText.text = config.name;
            uiView.VitaSlider.value = vita.Value.Percent;
            uiView.VitaText.text = vita.Value.PercentText;
            uiView.EnergySlider.value = energy.Value.Percent;
            uiView.EnergyText.text = energy.Value.PercentText;
            uiView.StrideText.text = $"Stride: {stride.Value.PercentText}";
            uiView.AttacksText.text = $"Attacks: {attacksLeft.Value.PercentText}";
        }

        private void OnAttacksLeftChanged(AttacksLeftChangedSignal signal)
            => _canvasView.ActiveUnit.AttacksText.text = $"Attacks: {signal.value.PercentText}";

        public void Dispose() => _disposable?.Dispose();
    }
}