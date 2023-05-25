using Zenject;

namespace TurnBasedRPG.Installers
{
    public class EcsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EcsService>().AsSingle();
        }
    }
}