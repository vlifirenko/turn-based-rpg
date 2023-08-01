using TurnBasedRPG.Installers;
using TurnBasedRPG.Model.Map;
using TurnBasedRPG.Strategy.Pathfinding;
using TurnBasedRPG.View;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Services
{
    public class TBMapService : IInitializable
    {
        private readonly GlobalConfigInstaller.MapConfig _mapConfig;
        private readonly SceneData _sceneData;
        private readonly IPathfindingStrategy _pathfinding;
        
        private Map _map;

        public TBMapService(GlobalConfigInstaller.MapConfig mapConfig, SceneData sceneData)
        {
            _mapConfig = mapConfig;
            _sceneData = sceneData;

            _pathfinding = new DikstraPathfinding();
        }

        public void Initialize()
        {
            var cellItems = new ICellItem[_mapConfig.width, _mapConfig.height];
            
            CreateMap(cellItems);
            _pathfinding.SetMap(_map);
        }

        private void CreateMap(ICellItem[,] items)
        {
            var cells = new Cell[items.GetLength(0), items.GetLength(1)];

            _map = new Map(cells);

            for (var i = 0; i < cells.GetLength(0); i++)
            {
                for (var j = 0; j < cells.GetLength(1); j++)
                {
                    var position = new Vector3(
                        i * (_mapConfig.cellSize.x + _mapConfig.cellSpacing.x),
                        0f,
                        j * (_mapConfig.cellSize.y + _mapConfig.cellSpacing.y));
                    var cellView = InstantiateCell(position);

                    _map.InitializeCell(cellView, new Vector2Int(i, j));
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

        public Cell[] BuildPath(Vector2Int origin, Vector2Int destination, float range) 
            => _pathfinding.BuildPath(origin, destination, range);
    }
}