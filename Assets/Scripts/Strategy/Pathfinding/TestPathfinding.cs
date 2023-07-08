using TurnBasedRPG.Model.Map;

namespace TurnBasedRPG.Strategy.Pathfinding
{
    public class TestPathfinding : AStrategy, IPathfindingStrategy
    {
        private Map _map;
    
        public void SetMap(Map map)
        {
            _map = map;
        }
    }
}