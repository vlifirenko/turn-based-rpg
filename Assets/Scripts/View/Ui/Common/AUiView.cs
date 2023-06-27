using System;
using UnityEngine;

namespace TurnBasedRPG.View.Ui.Common
{
    public abstract class AUiView : MonoBehaviour
    {
        public GameObject GameObject { get;private set; }

        private void Awake()
        {
            GameObject = gameObject;
        }

        public void Show() => GameObject.SetActive(true);

        public void Hide() => GameObject.SetActive(false);

        public bool IsVisible() => GameObject.activeSelf;
    }
}