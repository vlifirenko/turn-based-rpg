using TurnBasedRPG.View.Common;
using UnityEngine;

namespace TurnBasedRPG.View
{
    public class CellsContainer : AView
    {
        [SerializeField] private CellView prefab;

        public CellView Prefab => prefab;
    }
}