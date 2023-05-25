using System;
using TurnBasedRPG.View.Unit;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Installers
{
    [CreateAssetMenu(fileName = "Prefabs", menuName = "Config/Prefabs")]
    public class PrefabInstaller : ScriptableObjectInstaller
    {
        public Units units;
        
        public override void InstallBindings()
        {
            Container.BindInstance(units);
        }
        
        [Serializable]
        public class Units
        {
            public UnitView debug;
        }
    }
}