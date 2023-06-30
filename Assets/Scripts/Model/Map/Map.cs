using TurnBasedRPG.View;

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

        public void InitializeCell(CellView cellView)
            => Cells[cellView.Position.x, cellView.Position.y] = new Cell(cellView);
    }
}