using TurnBasedRPG.View.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedRPG.View
{
    public class CanvasView : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private UiUnitContainer unitContainer;
        [SerializeField] private Button nextTurnButton;

        public UiUnitContainer UnitContainer => unitContainer;
        public Canvas Canvas => canvas;
        public Button NextTurnButton => nextTurnButton;
    }
}