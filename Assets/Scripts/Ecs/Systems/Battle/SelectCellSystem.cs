using Scellecs.Morpeh;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Model.Unit;
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
                if (cellView.Cell.Content == null)
                    return;

                if (cellView.Cell.Content is AUnit unit)
                    _battleService.SelectUnit(unit);
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
                //UnityEngine.Debug.Log($"{cellView.name}:{cellView.UnitView?.name}");
                if (cellView.Cell.Content == null)
                    _battleService.MoveTo(cellView.Cell);
                else
                    _battleService.Attack(cellView.Cell);
            }
        }

        private void HoverCell(CellView cellView)
        {
            var material = cellView.GetComponent<Renderer>().material;
            material.SetColor("_BaseColor", Color.green);
            
            if (cellView.Cell.Content is AUnit unit)
            {
                if (unit.IsPlayer)
                    unit.Hover();
                else
                    unit.Hover(_battleService.ActiveUnit);
            }
        }

        private void UnhoverCell(CellView cellView)
        {
            var material = cellView.GetComponent<Renderer>().material;
            material.SetColor("_BaseColor", Color.white);

            if (cellView.Cell.Content is AUnit unit)
            {
                if (unit.IsPlayer)
                    unit.Unhover();
                else
                    unit.Unhover(_battleService.ActiveUnit);
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