﻿using TurnBasedRPG.View.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedRPG.View
{
    public class CanvasView : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private UiUnitContainer playerUnits;
        [SerializeField] private UiUnitContainer enemyUnits;
        [SerializeField] private Button nextTurnButton;

        public Canvas Canvas => canvas;
        public Button NextTurnButton => nextTurnButton;

        public UiUnitContainer PlayerUnits => playerUnits;

        public UiUnitContainer EnemyUnits => enemyUnits;
    }
}