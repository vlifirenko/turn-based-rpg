using System;
using TurnBasedRPG.View.Ui.Common;
using UnityEngine;

namespace TurnBasedRPG.View.Common
{
    public class UiPool : AView
    {
        [SerializeField] private AUiView prefab;
        [SerializeField] private Transform container;
        [SerializeField] private int size = 10;
        
        private AUiView[] _pool;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _pool = new AUiView[size];
            
            for (var i = 0; i < size; i++)
            {
                if (container != null)
                    _pool[i] = Instantiate(prefab, container);
                else
                    _pool[i] = Instantiate(prefab, transform);

                _pool[i].Hide();
            }
        }

        public AUiView GetItem()
        {
            foreach (var item in _pool)
            {
                if (!item.IsVisible())
                {
                    item.Show();
                    return item;
                }
            }

            throw new Exception("Pool is full");
        }

        public void Recycle(AUiView item) => item.Hide();
    }
}