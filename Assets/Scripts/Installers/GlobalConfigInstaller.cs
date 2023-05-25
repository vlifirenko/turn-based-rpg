using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Installers
{
    [CreateAssetMenu(fileName = "GlobalConfig", menuName = "Config/Global")]
    public class GlobalConfigInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
        }
    }
}