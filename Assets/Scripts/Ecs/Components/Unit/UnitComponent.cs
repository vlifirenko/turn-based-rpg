using Scellecs.Morpeh;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.View;
using TurnBasedRPG.View.Ui;
using TurnBasedRPG.View.Unit;

namespace TurnBasedRPG.Ecs.Components.Unit
{
    public struct UnitComponent : IComponent
    {
        public AUnit value;
        public CellView cellView;
    }
}