using TurnBasedRPG.Installers;
using TurnBasedRPG.Model.Map;
using TurnBasedRPG.View;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Services
{
    public class MapService : IInitializable
    {
        private readonly GlobalConfigInstaller.MapConfig _mapConfig;
        private readonly SceneData _sceneData;
        
        private Map _map;

        public MapService(GlobalConfigInstaller.MapConfig mapConfig, SceneData sceneData)
        {
            _mapConfig = mapConfig;
            _sceneData = sceneData;
        }

        public void Initialize()
        {
            var cellItems = new ICellItem[_mapConfig.width, _mapConfig.height];
            CreateMap(cellItems);
        }

        public void CreateMap(ICellItem[,] items)
        {
            var cells = new Cell[items.GetLength(0), items.GetLength(1)];

            _map = new Map(cells);
            Debug.Log(_map);

            for (var i = 0; i < cells.GetLength(0); i++)
            {
                for (var j = 0; j < cells.GetLength(1); j++)
                {
                    var position = new Vector3(
                        i * (_mapConfig.cellSize.x + _mapConfig.cellSpacing.x),
                        0f,
                        j * (_mapConfig.cellSize.y + _mapConfig.cellSpacing.y));
                    var cellView = InstantiateCell(position);

                    cellView.Position = new Vector2Int(i, j);
                    _map.InitializeCell(cellView);
                }
            }
        }
        
        public Cell GetCellByPosition(int x, int y) => _map.Cells[x, y];

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