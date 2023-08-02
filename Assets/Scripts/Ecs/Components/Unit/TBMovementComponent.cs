﻿using System;
using Scellecs.Morpeh;
using TurnBasedRPG.Model.Map;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Components.Unit
{
    public struct TBMovementComponent : IComponent
    {
        public Vector2Int destination;
        //public Cell targetCell;
        public Cell[] path;
        public int pathIndex;
        public float range;
        public Action onMovementComplete;
    }
}