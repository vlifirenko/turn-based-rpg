using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.Services;
using TurnBasedRPG.Signals;
using TurnBasedRPG.Utils;
using TurnBasedRPG.View;
using TurnBasedRPG.View.Ui;
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
            foreach (var item in _unitsConfig.startUnits)
            {
                var unit = _unitService.CreateUnit(item.config, item.position, true);

                unit.Entity.AddComponent<PlayerComponent>();
                unit.IsPlayer = true;
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
            var uiView = unit.IsPlayer ? _canvasView.PlayerUnits.CreateItem() : _canvasView.EnemyUnits.CreateItem();
            unit.UiView = uiView;
            
            _signalBus.Fire(new UnitUpdatedSignal(unit));
        }

        public void Dispose() => _disposable.Dispose();
    }
}