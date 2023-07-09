using TMPro;
using TurnBasedRPG.Model.Enums;
using TurnBasedRPG.View.Ui.Common;
using UnityEngine.UI;

namespace TurnBasedRPG.View.Ui.Characters
{
    public class UiSlot : AUiView
    {
        public EItemSlot slot;
        public TMP_Text slotName;
        public Image icon;
        public Button button;
    }
}