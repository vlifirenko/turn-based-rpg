using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Extensions;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Model;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.Signals;
using TurnBasedRPG.View;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace TurnBasedRPG.Services
{
    public class BattleService : IInitializable, IDisposable
    {
        private readonly SceneData _sceneData;
        private readonly GlobalConfigInstaller.MapConfig _mapConfig;
        private readonly List<CellView> _cells = new List<CellView>();
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly CanvasView _canvasView;
        private readonly SignalBus _signalBus;

        private BattleData _battleData;
        private AUnit _activeUnit;
        private readonly List<AUnit> _allUnits = new List<AUnit>();

        public BattleData BattleData => _battleData;

        public BattleService(SceneData sceneData, GlobalConfigInstaller.MapConfig mapConfig, CanvasView canvasView,
            SignalBus signalBus)
        {
            _sceneData = sceneData;
            _mapConfig = mapConfig;
            _canvasView = canvasView;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _battleData = new BattleData();
            
            _canvasView.NextTurnButton.OnClickAsObservable()
                .Subscribe(_ => NextTurn())
                .AddTo(_disposable);
            
            _signalBus.GetStream<NextTurnSignal>()
                .Subscribe(_ => NextTurn())
                .AddTo(_disposable);
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

        public CellView GetCellByPosition(int x, int y)
        {
            foreach (var cell in _cells)
            {
                if (cell.Position.x == x && cell.Position.y == y)
                    return cell;
            }

            throw new Exception($"Cell not found, cells size: {_cells.Count}");
        }

        public void AddUnit(AUnit unit) => _allUnits.Add(unit);

        public void SelectUnit(AUnit unit)
        {
        }

        public void MoveTo(CellView targetCell) => _activeUnit.MoveTo(targetCell);

        public void Attack(CellView targetCell)
        {
            var unit = targetCell.UnitView.Unit;
            if (unit.Entity.Has<PlayerComponent>())
                return;

            _activeUnit.Entity.AddComponent<AttackComponent>() = new AttackComponent
            {
                Target = unit
            };
        }

        public void NextTurn()
        {
            if (_activeUnit != null)
            {
                ref var stride = ref _activeUnit.Entity.GetComponent<StrideComponent>();
                stride.Value.SetMax();
                ref var attackLeft = ref _activeUnit.Entity.GetComponent<AttacksLeftComponent>();
                attackLeft.Value.SetMax();
                
                var currentUnitIndex = _allUnits.IndexOf(_activeUnit);
                currentUnitIndex += 1;
                currentUnitIndex %= _allUnits.Count;

                SetActiveUnit(_allUnits[currentUnitIndex]);
            }
            else
            {
                _allUnits.Sort((unit, aUnit) => unit.Initiative.CompareTo(aUnit.Initiative));
                SetActiveUnit(_allUnits[0]);
            }
            
            _activeUnit.StartTurn();
            _signalBus.Fire(new SetActiveUnitSignal(_activeUnit));
        }

        private void SetActiveUnit(AUnit activeUnit)
        {
            _activeUnit?.Deselect();
            _activeUnit = activeUnit;
            _activeUnit.Select();
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

        public void Dispose() => _disposable?.Dispose();
    }
}