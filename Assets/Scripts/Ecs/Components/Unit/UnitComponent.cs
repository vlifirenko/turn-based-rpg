﻿using Scellecs.Morpeh;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.View.Unit;

namespace TurnBasedRPG.Ecs.Components.Unit
{
    public struct UnitComponent : IComponent
    {
        public UnitConfig Config;
        public UnitView View;
    }
}