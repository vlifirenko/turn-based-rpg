using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.Services;
using TurnBasedRPG.Signals;
using TurnBasedRPG.Utils;
using TurnBasedRPG.View;
using UniRx;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class UnitInitializer : IInitializer
    {
        public World World { get; set; }

        private readonly GlobalConfigInstaller.UnitsConfig _unitsConfig;
        private readonly UnitService _unitService;
        private readonly BattleService _battleService;
        private readonly CanvasView _canvasView;
        private readonly SignalBus _signalBus;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public UnitInitializer(GlobalConfigInstaller.UnitsConfig unitsConfig, UnitService unitService,
            BattleService battleService, CanvasView canvasView, SignalBus signalBus)
        {
            _unitsConfig = unitsConfig;
            _unitService = unitService;
            _battleService = battleService;
            _canvasView = canvasView;
            _signalBus = signalBus;
        }

        public void OnAwake()
        {
            _signalBus.GetStream<VitaChangedSignal>()
                .Subscribe(OnVitaChanged)
                .AddTo(_disposable);

            foreach (var item in _unitsConfig.startUnits)
            {
                var unit = _unitService.CreateUnit(item.config, item.position, true);

                unit.Entity.AddComponent<PlayerComponent>();
                _battleService.AddUnit(unit);
                InstantiateUnitUi(unit);
            }

            foreach (var item in _unitsConfig.enemyUnits)
            {
                var unit = _unitService.CreateUnit(item.config, item.position);

                unit.Entity.AddComponent<EnemyComponent>();
                _battleService.AddUnit(unit);
                InstantiateUnitUi(unit);
            }

            _battleService.NextTurn();
        }

        private void InstantiateUnitUi(AUnit unit)
        {
            var uiView = _canvasView.UnitContainer.CreateItem();
            unit.UiView = uiView;
            //_signalBus.Fire(new VitaChangedSignal(unit));
        }

        private void OnVitaChanged(VitaChangedSignal signal)
        {
            var unit = signal.entity;
            var unitComponent = unit.Entity.GetComponent<UnitComponent>();
            var vita = unit.Entity.GetComponent<VitaComponent>();

            unitComponent.uiView.VitaSlider.value = vita.Value.Percent;
            unitComponent.uiView.VitaText.text = vita.Value.PercentText;
        }

        public void Dispose() => _disposable.Dispose();
    }
}