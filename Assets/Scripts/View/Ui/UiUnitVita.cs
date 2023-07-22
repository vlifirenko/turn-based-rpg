using TMPro;
using TurnBasedRPG.View.Ui.Common;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedRPG.View.Ui
{
    public class UiUnitVita : AUiContainerItem
    {
        [SerializeField] private Slider vitaSlider;
        [SerializeField] private TMP_Text vitaText;
        
        public Slider VitaSlider => vitaSlider;

        public TMP_Text VitaText => vitaText;
    }
}