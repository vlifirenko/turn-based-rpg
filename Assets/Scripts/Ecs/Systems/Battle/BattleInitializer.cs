using Scellecs.Morpeh;
using TurnBasedRPG.Services;

namespace TurnBasedRPG.Ecs.Systems.Battle
{
    public class BattleInitializer : IInitializer
    {
        public World World { get; set; }
        private readonly BattleService _battleService;

        public BattleInitializer(BattleService battleService)
        {
            _battleService = battleService;
        }

        public void OnAwake()
        {
            _battleService.CreateMap();
        }

        public void Dispose()
        {
        }
    }
}