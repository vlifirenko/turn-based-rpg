using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Installers;
using TurnBasedRPG.Services;
using TurnBasedRPG.View.Unit;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class UnitInitializer : IInitializer
    {
        public World World { get; set; }

        private readonly GlobalConfigInstaller.UnitsConfig _unitsConfig;
        private readonly UnitService _unitService;

        public UnitInitializer(
            GlobalConfigInstaller.UnitsConfig unitsConfig,
            UnitService unitService)
        {
            _unitsConfig = unitsConfig;
            _unitService = unitService;
        }

        public void OnAwake()
        {
            foreach (var item in _unitsConfig.startUnits)
                _unitService.CreateUnit(item.config, item.position);
        }

        public void Dispose()
        {
        }
    }
}