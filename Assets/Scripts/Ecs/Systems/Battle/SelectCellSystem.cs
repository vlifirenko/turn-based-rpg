using Scellecs.Morpeh;
using TurnBasedRPG.Installers;

namespace TurnBasedRPG.Ecs.Systems.Battle
{
    public class SelectCellSystem : ISystem
    {
        private readonly GlobalConfigInstaller.LayersConfig _layersConfig;
        
        public World World { get; set; }

        public void OnAwake() {}

        public void OnUpdate(float deltaTime)
        {
        }

        public void Dispose() {}
    }
}