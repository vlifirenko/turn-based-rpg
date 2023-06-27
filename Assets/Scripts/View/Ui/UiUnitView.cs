using TMPro;
using TurnBasedRPG.View.Ui.Common;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedRPG.View.Ui
{
    public class UiUnitView : AUiContainerItem
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Slider vitaSlider;
        [SerializeField] private TMP_Text vitaText;
        [SerializeField] private Slider energySlider;
        [SerializeField] private TMP_Text energyText;
        [SerializeField] private TMP_Text strideText;
        [SerializeField] private TMP_Text attacksText;
        [SerializeField] private GameObject attackPanel;
        [SerializeField] private TMP_Text chanceText;
        [SerializeField] private GameObject selected;

        public Image Icon => icon;
        public TMP_Text NameText => nameText;
        public Slider VitaSlider => vitaSlider;
        public TMP_Text VitaText => vitaText;
        public Slider EnergySlider => energySlider;
        public TMP_Text EnergyText => energyText;
        public TMP_Text StrideText => strideText;
        public TMP_Text AttacksText => attacksText;
        public GameObject AttackPanel => attackPanel;
        public TMP_Text ChanceText => chanceText;

        public GameObject Selected => selected;
    }
}