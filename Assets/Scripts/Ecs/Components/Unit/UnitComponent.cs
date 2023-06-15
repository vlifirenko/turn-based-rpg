using Scellecs.Morpeh;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.View;
using TurnBasedRPG.View.Ui;
using TurnBasedRPG.View.Unit;

namespace TurnBasedRPG.Ecs.Components.Unit
{
    public struct UnitComponent : IComponent
    {
        public AUnit Unit;
        public UnitConfig config;
        public UnitView view;
        public CellView cellView;
        public UiUnitVita uiView;
    }
}