using System;
using TurnBasedRPG.Model.Config;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Installers
{
    [CreateAssetMenu(fileName = "GlobalConfig", menuName = "Config/Global")]
    public class GlobalConfigInstaller : ScriptableObjectInstaller
    {
        public UnitsConfig unitsConfig;
        public MapConfig mapConfig;
        public LayersConfig layersConfig;

        [Serializable]
        public class UnitsConfig
        {
            public UnitConfig[] startUnits;
        }

        [Serializable]
        public class MapConfig
        {
            public int width = 10;
            public int height = 10;
            public Vector2 cellSize = Vector2.one;
            public Vector2 cellSpacing = new Vector2(.1f, .1f);
        }
        
        [Serializable]
        public class LayersConfig
        {
            public LayerMask cell;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(unitsConfig);
            Container.BindInstance(mapConfig);
            Container.BindInstance(layersConfig);
        }
    }
}