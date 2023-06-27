using System.Linq;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Services;
using UnityEngine;

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
            SetInitiative();
        }

        private void SetInitiative()
        {
            var filter = World.Filter.With<UnitComponent>();
            foreach (var entity in filter)
            {
                var unit = entity.GetComponent<UnitComponent>().unit;
                var randomInitiative = Random.Range(0, filter.Count() + 1);

                unit.Initiative = randomInitiative;
            }
        }

        public void Dispose()
        {
        }
    }
}