using System.Collections.Generic;
using TurnBasedRPG.Model.Map;
using UnityEngine;

namespace TurnBasedRPG.Strategy.Pathfinding
{
    public class DikstraPathfinding : AStrategy, IPathfindingStrategy
    {
        private Map _map;
        private Vector2Int _origin;
        private Vector2Int _destination;
        private MapCell[,] _mapCells;
        private Vector2Int _position;

        public void SetMap(Map map)
        {
            _map = map;
        }

        public Cell[] BuildPath(Vector2Int origin, Vector2Int destination)
        {
            _origin = origin;
            _destination = destination;
            _mapCells = new MapCell[_map.Cells.GetLength(0), _map.Cells.GetLength(1)];
            for (var i = 0; i < _mapCells.GetLength(0); i++)
            {
                for (var j = 0; j < _mapCells.GetLength(1); j++)
                {
                    _mapCells[i, j].position = new Vector2Int(i, j);
                    _mapCells[i, j].cost = _map.Cells[i, j].Content == null ? 0f : 1f;
                }
            }

            return StartPathBuilding();
        }

        private Cell[] StartPathBuilding()
        {
            var path = new List<Cell>();
            _position = _origin;
            var nextCell = FindNextCell();

            return path.ToArray();
        }

        private Cell FindNextCell()
        {
            var neighbours = GetAllFreeNeighbours(_position);

            foreach (var mapCell in neighbours)
            {
                //Debug.Log(mapCell.position);
            }

            //var targetCell = _mapService.GetCellByPosition(position.x, position.y);
            //movement.targetCell = targetCell.View;
            return null;
        }

        private MapCell[] GetAllFreeNeighbours(Vector2Int position)
        {
            // 1 2 3
            // 8   4
            // 7 6 5
            var result = new List<MapCell>();
            // 1
            if (position.x > 0 && position.y < _map.Cells.GetLength(1) - 1
                               && _mapCells[position.x - 1, position.y + 1].IsAvailable())
                result.Add(_mapCells[position.x - 1, position.y + 1]);
            // 2
            if (position.y < _map.Cells.GetLength(1) - 1
                && _mapCells[position.x, position.y + 1].IsAvailable())
                result.Add(_mapCells[position.x, position.y + 1]);
            // 3
            if (position.x < _mapCells.GetLength(0) - 1 && position.y < _map.Cells.GetLength(1) - 1
                                                        && _mapCells[position.x + 1, position.y + 1].IsAvailable())
                result.Add(_mapCells[position.x + 1, position.y + 1]);
            // 4
            if (position.x < _mapCells.GetLength(0) - 1
                && _mapCells[position.x + 1, position.y].IsAvailable())
                result.Add(_mapCells[position.x + 1, position.y]);
            // 5
            if (position.x < _mapCells.GetLength(0) - 1 && position.y > 0
                                                        && _mapCells[position.x + 1, position.y - 1].IsAvailable())
                result.Add(_mapCells[position.x + 1, position.y - 1]);
            // 6
            if (position.y > 0
                && _mapCells[position.x, position.y - 1].IsAvailable())
                result.Add(_mapCells[position.x, position.y - 1]);
            // 7
            if (position.x > 0 && position.y > 0
                               && _mapCells[position.x, position.y - 1].IsAvailable())
                result.Add(_mapCells[position.x - 1, position.y - 1]);
            // 8
            if (position.x > 0
                && _mapCells[position.x - 1, position.y].IsAvailable())
                result.Add(_mapCells[position.x - 1, position.y]);

            return result.ToArray();
        }

        private struct MapCell
        {
            public float cost;
            public bool isVisited;
            public Vector2Int position;

            public bool IsAvailable() => cost == 0f && !isVisited;
        }
    }
}