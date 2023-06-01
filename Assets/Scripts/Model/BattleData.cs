using System.Collections.Generic;
using Scellecs.Morpeh;

namespace TurnBasedRPG.Model
{
    public class BattleData
    {
        public List<Entity> UnitOrder = new List<Entity>();

        public int? CurrentUnitIndex { get; set; }

        public Entity GetCurrentUnit() => CurrentUnitIndex != null ? UnitOrder[CurrentUnitIndex.Value] : null;
    }
}