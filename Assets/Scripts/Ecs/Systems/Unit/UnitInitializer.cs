using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Services;
using TurnBasedRPG.Signals;
using TurnBasedRPG.Utils;
using TurnBasedRPG.View;
using TurnBasedRPG.View.Unit;
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
                var entity = _unitService.CreateUnit(item.config, item.position);

                entity.AddComponent<PlayerComponent>();
                _battleService.AddUnit(entity);
                InstantiateUnitUi(entity);
            }

            foreach (var item in _unitsConfig.enemyUnits)
            {
                var entity = _unitService.CreateUnit(item.config, item.position);

                entity.AddComponent<EnemyComponent>();
                _battleService.AddUnit(entity);
                InstantiateUnitUi(entity);
            }

            _battleService.InitBattleData();
        }

        private void InstantiateUnitUi(Entity entity)
        {
            ref var unit = ref entity.GetComponent<UnitComponent>();
            var uiView = Object.Instantiate(_canvasView.UnitVitaContainer.Prefab, _canvasView.UnitVitaContainer.transform);
            var position = _canvasView.Canvas.WorldToCanvasPosition(unit.view.transform.position, Camera.main);

            uiView.GetComponent<RectTransform>().anchoredPosition = position;
            unit.uiView = uiView;
            
            _signalBus.Fire(new VitaChangedSignal(entity));
        }

        private void OnVitaChanged(VitaChangedSignal signal)
        {
            var entity = signal.entity;
            var unit = entity.GetComponent<UnitComponent>();
            var vita = entity.GetComponent<VitaComponent>();
            
            unit.uiView.VitaSlider.value = vita.Value.Percent;
            unit.uiView.VitaText.text = vita.Value.PercentText;
        }

        public void Dispose() => _disposable.Dispose();
    }
}