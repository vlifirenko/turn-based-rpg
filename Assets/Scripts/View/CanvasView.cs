using TurnBasedRPG.View.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedRPG.View
{
    public class CanvasView : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private UiActiveUnit activeUnit;
        [SerializeField] private UiUnitVitaContainer unitVitaContainer;
        [SerializeField] private Button nextTurnButton;

        public UiActiveUnit ActiveUnit => activeUnit;

        public UiUnitVitaContainer UnitVitaContainer => unitVitaContainer;

        public Canvas Canvas => canvas;

        public Button NextTurnButton => nextTurnButton;
    }
}