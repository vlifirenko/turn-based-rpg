using TurnBasedRPG.Model.Map;
using UnityEngine;

namespace TurnBasedRPG.Strategy.Pathfinding
{
    public interface IPathfindingStrategy
    {
        void SetMap(Map map);
        Cell[] BuildPath(Vector2Int origin, Vector2Int destination);
    }
}