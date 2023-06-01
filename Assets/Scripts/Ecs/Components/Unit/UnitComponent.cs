using Scellecs.Morpeh;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.View;
using TurnBasedRPG.View.Ui;
using TurnBasedRPG.View.Unit;

namespace TurnBasedRPG.Ecs.Components.Unit
{
    public struct UnitComponent : IComponent
    {
        public UnitConfig config;
        public UnitView view;
        public CellView cellView;
        public UiUnitVita uiView;
    }
}