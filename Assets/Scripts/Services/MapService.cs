using TurnBasedRPG.Model.Map;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Services
{
    public class MapService : IInitializable
    {
        private Map _map;

        public void Initialize()

        {
            var cellItems = new ICellItem[10, 10];
            CreateMap(cellItems);
        }

        public void CreateMap(ICellItem[,] items)
        {
            var cells = new Cell[items.GetLength(0), items.GetLength(1)];

            _map = new Map(cells);
            Debug.Log(_map);
        }
    }
}