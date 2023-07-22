using System;
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
        private float _range;

        public void SetMap(Map map)
        {
            _map = map;
        }

        public Cell[] BuildPath(Vector2Int origin, Vector2Int destination, float range)
        {
            _origin = origin;
            _destination = destination;
            _range = range;
            
            _mapCells = new MapCell[_map.Cells.GetLength(0), _map.Cells.GetLength(1)];
            for (var i = 0; i < _mapCells.GetLength(0); i++)
            {
                for (var j = 0; j < _mapCells.GetLength(1); j++)
                {
                    _mapCells[i, j] = new MapCell
                    {
                        position = new Vector2Int(i, j),
                        cost = _map.Cells[i, j].Content == null ? 0f : 1f
                    };
                }
            }

            return StartPathBuilding();
        }

        private Cell[] StartPathBuilding()
        {
            var path = new List<Cell>();

            _position = _origin;
            var nextCell = FindNextCell();
            path.Add(nextCell);
            _position = nextCell.Position;

            var distance = Vector2Int.Distance(_position, _destination);
            if (distance > _range)
            {
                var i = 0;
                while (distance > _range)
                {
                    nextCell = FindNextCell();
                    path.Add(nextCell);
                    _position = nextCell.Position;

                    distance = Vector2Int.Distance(_position, _destination);
                    
                    i++;
                    if (i == 50)
                        throw new Exception();
                }
            }

            return path.ToArray();
        }

        private Cell FindNextCell()
        {
            var neighbours = GetAllFreeNeighbours(_position);
            var nearestCell = GetNearestPathNeighbour(neighbours, _destination);
            var cell = _map.Cells[nearestCell.position.x, nearestCell.position.y];

            return cell;
        }

        private MapCell[] GetAllFreeNeighbours(Vector2Int position)
        {
            // 1 2 3
            // 8 * 4
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

        private MapCell GetNearestPathNeighbour(MapCell[] neighbours, Vector2Int target)
        {
            MapCell nearestCell = default;
            var nearestDistance = Mathf.Infinity;
            foreach (var mapCell in neighbours)
            {
                var distance = Vector2Int.Distance(mapCell.position, target);
                if (distance < nearestDistance)
                {
                    nearestCell = mapCell;
                    nearestDistance = distance;
                }
            }

            nearestCell.isVisited = true;

            return nearestCell;
        }

        private class MapCell
        {
            public float cost;
            public bool isVisited;
            public Vector2Int position;

            public bool IsAvailable() => cost == 0f && !isVisited;
        }
    }
}