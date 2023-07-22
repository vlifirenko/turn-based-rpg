using TurnBasedRPG.View.Common;
using UnityEngine;

namespace TurnBasedRPG.View
{
    public class SceneData : AView
    {
        [SerializeField] private CellsContainer cellsContainer;
        [SerializeField] private Transform unitContainer;

        public CellsContainer CellsContainer => cellsContainer;
        public Transform UnitContainer => unitContainer;
    }
}