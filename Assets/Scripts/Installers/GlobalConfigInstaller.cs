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
        
        [Serializable]
        public class UnitsConfig
        {
            public UnitConfig[] startUnits;
        }
        
        public override void InstallBindings()
        {
            Container.BindInstance(unitsConfig);
        }
    }
}