using TurnBasedRPG.View.Common;
using TurnBasedRPG.View.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedRPG.View.Canvas
{
    public class CanvasView : ACanvasView
    {
        [SerializeField] private UnityEngine.Canvas canvas;
        [SerializeField] private UiUnitContainer playerUnits;
        [SerializeField] private UiUnitContainer enemyUnits;
        [SerializeField] private Button nextTurnButton;
        [SerializeField] private UiPool floatTextUiPool;

        public UnityEngine.Canvas Canvas => canvas;
        public Button NextTurnButton => nextTurnButton;
        public UiUnitContainer PlayerUnits => playerUnits;
        public UiUnitContainer EnemyUnits => enemyUnits;
        public UiPool FloatTextUiPool => floatTextUiPool;
    }
}