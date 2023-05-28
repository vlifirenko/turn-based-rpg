using UnityEngine;

namespace TurnBasedRPG.View.Ui
{
    public class UiUnitVitaContainer : AUiView
    {
        [SerializeField] private UiUnitVita prefab;

        public UiUnitVita Prefab => prefab;
    }
}