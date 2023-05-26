﻿using Scellecs.Morpeh;
using TurnBasedRPG.Installers;
using TurnBasedRPG.View;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Systems.Battle
{
    public class SelectCellSystem : ISystem
    {
        private readonly GlobalConfigInstaller.LayersConfig _layersConfig;

        private readonly RaycastHit[] _raycastHits = new RaycastHit[1];
        private CellView _hoveredCell;

        public World World { get; set; }

        public SelectCellSystem(GlobalConfigInstaller.LayersConfig layersConfig)
        {
            _layersConfig = layersConfig;
        }

        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastNonAlloc(ray, _raycastHits, Mathf.Infinity, _layersConfig.cell);

            if (hits > 0)
            {
                for (var i = 0; i < hits; i++)
                {
                    if (_raycastHits[i].transform.TryGetComponent<CellView>(out var cellView))
                    {
                        if (_hoveredCell == cellView)
                            return;

                        if (_hoveredCell != null)
                            UnhoverCell(_hoveredCell);
                        HoverCell(cellView);
                        _hoveredCell = cellView;
                    }
                }
            }
            else if (_hoveredCell != null)
                UnhoverCell(_hoveredCell);
        }

        private void HoverCell(CellView cell)
        {
            var material = cell.GetComponent<Renderer>().material;
            material.SetColor("_BaseColor", Color.green);
        }

        private void UnhoverCell(CellView cell)
        {
            var material = cell.GetComponent<Renderer>().material;
            material.SetColor("_BaseColor",Color.white);
        }

        public void Dispose()
        {
        }
    }
}