using Scellecs.Morpeh;
using TurnBasedRPG.Services;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Systems.Debug
{
    public class DebugSystem : ISystem
    {
        private readonly BattleService _battleService;
        
        public World World { get; set; }

        public DebugSystem(BattleService battleService)
        {
            _battleService = battleService;
        }

        public void OnUpdate(float deltaTime)
        {
            /*if (Input.GetKeyUp(KeyCode.F1))
                _battleService.NextUnit();*/
        }

        public void OnAwake()
        {
        }

        public void Dispose()
        {
        }
    }
}