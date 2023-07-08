using TurnBasedRPG.View.Common;
using TurnBasedRPG.View.Unit;
using UnityEngine;

namespace TurnBasedRPG.View
{
    public class CellView : AView
    {
        public Vector2Int Position { get; set; }
        public UnitView UnitView { get; set; }
    }
}