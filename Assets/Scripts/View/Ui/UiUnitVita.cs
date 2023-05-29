using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedRPG.View.Ui
{
    public class UiUnitVita : AUiView
    {
        [SerializeField] private Slider vitaSlider;
        [SerializeField] private TMP_Text vitaText;
        
        public Slider VitaSlider => vitaSlider;

        public TMP_Text VitaText => vitaText;
    }
}