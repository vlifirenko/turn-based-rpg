using System;
using System.Collections.Generic;
using TurnBasedRPG.Installers;
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

        public BattleService(SceneData sceneData, GlobalConfigInstaller.MapConfig mapConfig)
        {
            _sceneData = sceneData;
            _mapConfig = mapConfig;
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

            throw new Exception("Cell not found");
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
    }
}