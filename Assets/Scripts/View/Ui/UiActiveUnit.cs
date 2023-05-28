﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedRPG.View.Ui
{
    public class UiActiveUnit : AUiView
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Slider vitaSlider;
        [SerializeField] private TMP_Text vitaText;
        [SerializeField] private Slider energySlider;
        [SerializeField] private TMP_Text energyText;

        public Image Icon => icon;

        public TMP_Text NameText => nameText;

        public Slider VitaSlider => vitaSlider;

        public TMP_Text VitaText => vitaText;

        public Slider EnergySlider => energySlider;

        public TMP_Text EnergyText => energyText;
    }
}