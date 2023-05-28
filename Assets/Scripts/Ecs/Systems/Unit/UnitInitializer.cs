using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Services;
using TurnBasedRPG.Utils;
using TurnBasedRPG.View;
using TurnBasedRPG.View.Unit;
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

        public UnitInitializer(
            GlobalConfigInstaller.UnitsConfig unitsConfig,
            UnitService unitService,
            BattleService battleService,
            CanvasView canvasView)
        {
            _unitsConfig = unitsConfig;
            _unitService = unitService;
            _battleService = battleService;
            _canvasView = canvasView;
        }

        public void OnAwake()
        {
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
            var position = _canvasView.Canvas.WorldToCanvasPosition(unit.View.transform.position, Camera.main);

            uiView.GetComponent<RectTransform>().anchoredPosition = position;
            unit.UiView = uiView;
            
            var vita = entity.GetComponent<VitaComponent>();
            uiView.VitaSlider.value = vita.Value.Percent;
            uiView.VitaText.text = vita.Value.PercentText;
        }

        public void Dispose()
        {
        }
    }
}