using System;
using UnityEngine;

namespace TurnBasedRPG.View.Ui.Common
{
    public abstract class AUiView : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        public bool IsVisible() => gameObject.activeSelf;
    }
}