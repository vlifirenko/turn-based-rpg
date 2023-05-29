using TurnBasedRPG.View.Ui;
using UnityEngine;

namespace TurnBasedRPG.View
{
    public class CanvasView : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private UiActiveUnit activeUnit;
        [SerializeField] private UiUnitVitaContainer unitVitaContainer;

        public UiActiveUnit ActiveUnit => activeUnit;

        public UiUnitVitaContainer UnitVitaContainer => unitVitaContainer;

        public Canvas Canvas => canvas;
    }
}