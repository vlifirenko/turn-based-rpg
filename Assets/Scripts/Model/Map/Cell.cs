using TurnBasedRPG.View;
using UnityEngine;

namespace TurnBasedRPG.Model.Map
{
    public class Cell
    {
        public CellView View { get; }
        public ICellItem Content { get; set; }
        public Vector2Int Position { get; set; }

        public Cell(CellView view)
        {
            View = view;
        }   

        public Cell(CellView view, Vector2Int position)
        {
            View = view;
            Position = position;
        }
    }
}