using UnityEngine;

namespace TurnBasedRPG.View.Ui.Common
{
    public abstract class AUiContainer<T> : AUiView where T : AUiContainerItem
    {
        [SerializeField] private T prefab;
        [SerializeField] private Transform container;

        public T Prefab => prefab;

        public T CreateItem() => Instantiate(prefab, container);
    }
}