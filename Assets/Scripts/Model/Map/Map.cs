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
    }
}