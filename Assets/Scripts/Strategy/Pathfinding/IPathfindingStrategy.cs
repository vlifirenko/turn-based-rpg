using TurnBasedRPG.Model.Map;

namespace TurnBasedRPG.Strategy.Pathfinding
{
    public interface IPathfindingStrategy
    {
        void SetMap(Map map);
    }
}