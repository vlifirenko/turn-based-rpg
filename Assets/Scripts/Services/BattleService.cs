using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Model;
using TurnBasedRPG.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TurnBasedRPG.Services
{
    public class BattleService
    {
        private readonly SceneData _sceneData;
        private readonly GlobalConfigInstaller.MapConfig _mapConfig;
        private readonly List<CellView> _cells = new List<CellView>();

        private CanvasView _canvasView;
        private BattleData _battleData;

        public BattleService(SceneData sceneData, GlobalConfigInstaller.MapConfig mapConfig, CanvasView canvasView)
        {
            _sceneData = sceneData;
            _mapConfig = mapConfig;
            _canvasView = canvasView;
            
            _battleData = new BattleData();
        }

        public void CreateMap()
        {
            for (var i = 0; i < _mapConfig.width; i++)
            {
                for (var j = 0; j < _mapConfig.height; j++)
                {
                    var position = new Vector3(
                        i * (_mapConfig.cellSize.x + _mapConfig.cellSpacing.x),
                        0f,
                        j * (_mapConfig.cellSize.y + _mapConfig.cellSpacing.y));
                    var cell = InstantiateCell(position);

                    cell.Position = new Vector2Int(i, j);
                    _cells.Add(cell);
                }
            }
        }

        public void InitBattleData()
        {
            var currentUnit = _battleData.GetCurrentUnit();
            UpdateUnitUi(currentUnit);
        }

        public CellView GetCellByPosition(int x, int y)
        {
            foreach (var cell in _cells)
            {
                if (cell.Position.x == x && cell.Position.y == y)
                    return cell;
            }

            throw new Exception($"Cell not found, cells size: {_cells.Count}");
        }

        public void AddUnit(Entity entity)
        {
            _battleData.UnitOrder.Add(entity);
        }
        
        public void SelectUnit(Entity entity)
        {
        }

        public void NextUnit()
        {
            _battleData.CurrentUnitIndex += 1;
            _battleData.CurrentUnitIndex %= _battleData.UnitOrder.Count;
            
            var currentUnit = _battleData.GetCurrentUnit();
            UpdateUnitUi(currentUnit);
        }

        private CellView InstantiateCell(Vector3 position)
        {
            var prefab = _sceneData.CellsContainer.Prefab;
            var cell = Object.Instantiate(prefab, _sceneData.CellsContainer.transform);
            var scale = new Vector3(
                _mapConfig.cellSize.x,
                prefab.transform.localScale.y,
                _mapConfig.cellSize.y);

            cell.transform.localPosition = position;
            cell.transform.localScale = scale;

            return cell;
        }

        private void UpdateUnitUi(Entity entity)
        {
           var config = entity.GetComponent<UnitComponent>().Config;
           var vita = entity.GetComponent<VitaComponent>();
           var energy = entity.GetComponent<EnergyComponent>();
           var uiView = _canvasView.ActiveUnit;

           uiView.Icon.sprite = config.icon;
           uiView.NameText.text = config.name;
           uiView.VitaSlider.value = vita.Value.Percent;
           uiView.VitaText.text = vita.Value.PercentText;
           uiView.EnergySlider.value = energy.Value.Percent;
           uiView.EnergyText.text = energy.Value.PercentText;
        }
    }
}