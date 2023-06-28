using Scellecs.Morpeh;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Services;
using TurnBasedRPG.View;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Systems.Battle
{
    public class SelectCellSystem : ISystem
    {
        private readonly GlobalConfigInstaller.LayersConfig _layersConfig;
        private readonly BattleService _battleService;

        private readonly RaycastHit[] _raycastHits = new RaycastHit[1];
        private CellView _hoveredCell;

        public World World { get; set; }

        public SelectCellSystem(GlobalConfigInstaller.LayersConfig layersConfig, BattleService battleService,
            UnitService unitService)
        {
            _layersConfig = layersConfig;
            _battleService = battleService;
        }

        public void OnUpdate(float deltaTime)
        {
            MouseRaycast();
            MouseLeftClick();
            MouseRightClick();
        }

        private void MouseRaycast()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastNonAlloc(ray, _raycastHits, Mathf.Infinity, _layersConfig.cell);

            if (hits > 0 && _raycastHits[0].transform.TryGetComponent<CellView>(out var cellView))
            {
                if (_hoveredCell == cellView)
                    return;

                if (_hoveredCell != null)
                    UnhoverCell(_hoveredCell);
                HoverCell(cellView);
                _hoveredCell = cellView;
            }
            else if (_hoveredCell != null)
                UnhoverCell(_hoveredCell);
        }

        private void MouseLeftClick()
        {
            if (!Input.GetMouseButtonUp(0))
                return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastNonAlloc(ray, _raycastHits, Mathf.Infinity, _layersConfig.cell);

            if (hits > 0 && _raycastHits[0].transform.TryGetComponent<CellView>(out var cellView))
            {
                if (cellView.UnitView == null)
                    return;

                _battleService.SelectUnit(cellView.UnitView.Unit);
            }
        }

        private void MouseRightClick()
        {
            if (!Input.GetMouseButtonUp(1))
                return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastNonAlloc(ray, _raycastHits, Mathf.Infinity, _layersConfig.cell);

            if (hits > 0 && _raycastHits[0].transform.TryGetComponent<CellView>(out var cellView))
            {
                if (cellView.UnitView == null)
                    _battleService.MoveTo(cellView);
                else
                    _battleService.Attack(cellView);
            }
        }

        private void HoverCell(CellView cell)
        {
            var material = cell.GetComponent<Renderer>().material;
            material.SetColor("_BaseColor", Color.green);

            if (cell.UnitView != null)
            {
                if (cell.UnitView.Unit.IsPlayer)
                    cell.UnitView.Unit.Hover();
                else
                    cell.UnitView.Unit.Hover(_battleService.ActiveUnit);
            }
        }

        private void UnhoverCell(CellView cell)
        {
            var material = cell.GetComponent<Renderer>().material;
            material.SetColor("_BaseColor", Color.white);

            if (cell.UnitView != null)
            {
                if (cell.UnitView.Unit.IsPlayer)
                    cell.UnitView.Unit.Unhover();
                else
                    cell.UnitView.Unit.Unhover(_battleService.ActiveUnit);
            }
        }

        public void OnAwake()
        {
        }

        public void Dispose()
        {
        }
    }
}