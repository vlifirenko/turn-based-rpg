using TurnBasedRPG.View;

namespace TurnBasedRPG.Model.Map
{
    public class Cell
    {
        public CellView View { get; }
        public ICellItem Item { get; set; }

        public Cell(CellView view)
        {
            View = view;
        }
    }
}