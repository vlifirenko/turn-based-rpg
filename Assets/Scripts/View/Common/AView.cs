using UnityEngine;

namespace TurnBasedRPG.View.Common
{
    public class AView : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        public bool IsVisible() => gameObject.activeSelf;
    }
}