using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.Services;

namespace TurnBasedRPG.Ecs.Systems.Battle
{
    public class AiTurnSystem : ISystem
    {
        private readonly BattleService _battleService;
        public World World { get; set; }

        private Filter _filter;

        public AiTurnSystem(BattleService battleService)
        {
            _battleService = battleService;
        }

        public void OnAwake() => _filter = World.Filter.With<AiTurnComponent>();

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                var unit = entity.GetComponent<UnitComponent>().Unit as AiUnit;
                unit.MakeTurn();
            }
        }

        public void Dispose()
        {
        }
    }
}