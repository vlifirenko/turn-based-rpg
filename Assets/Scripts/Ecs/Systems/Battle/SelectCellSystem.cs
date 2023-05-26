using Scellecs.Morpeh;
using TurnBasedRPG.Installers;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Systems.Battle
{
    public class SelectCellSystem : ISystem
    {
        private readonly GlobalConfigInstaller.LayersConfig _layersConfig;
        private readonly RaycastHit[] _raycastHits = new RaycastHit[1];

        public World World { get; set; }

        public SelectCellSystem(GlobalConfigInstaller.LayersConfig layersConfig)
        {
            _layersConfig = layersConfig;
        }

        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastNonAlloc(ray, _raycastHits, Mathf.Infinity, _layersConfig.cell);
            Debug.Log(hits);
            if (hits == 0)
                return;
            
            for (var i = 0; i < hits; i++)
            {
                Debug.Log("Hit " + _raycastHits[i].collider.gameObject.name);
            }
        }

        public void Dispose()
        {
        }
    }
}