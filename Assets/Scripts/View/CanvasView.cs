using TurnBasedRPG.View.Ui;
using UnityEngine;

namespace TurnBasedRPG.View
{
    public class CanvasView : MonoBehaviour
    {
        [SerializeField] private UiActiveUnit activeUnit;

        public UiActiveUnit ActiveUnit => activeUnit;
    }
}