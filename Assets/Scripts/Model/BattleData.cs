using System.Collections.Generic;
using Scellecs.Morpeh;
using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Model
{
    public class BattleData
    {
        public readonly List<AUnit> UnitOrder = new List<AUnit>();

        public int? CurrentUnitIndex { get; set; }

        public AUnit GetCurrentUnit() => CurrentUnitIndex != null ? UnitOrder[CurrentUnitIndex.Value] : null;
    }
}