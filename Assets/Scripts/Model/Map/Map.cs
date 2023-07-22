using TurnBasedRPG.View;
using UnityEngine;

namespace TurnBasedRPG.Model.Map
{
    public class Map
    {
        public Cell[,] Cells { get; }

        public Map(Cell[,] cells)
        {
            Cells = cells;
        }

        public void InteractWithCell()
        {
        }

        public void InitializeCell(CellView cellView, Vector2Int position)
        {
            var cell = new Cell(cellView, position);
            Cells[position.x, position.y] = cell;
            cellView.Cell = cell;
        }
    }
}