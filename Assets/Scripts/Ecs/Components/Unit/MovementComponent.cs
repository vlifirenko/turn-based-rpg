using System;
using Scellecs.Morpeh;
using TurnBasedRPG.View;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Components.Unit
{
    public struct MovementComponent : IComponent
    {
        public Vector2Int destination;
        public CellView targetCell;
        public CellView[] path;
        public Action onMovementComplete;
    }
}