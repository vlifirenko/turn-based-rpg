using Scellecs.Morpeh;
using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Ecs.Components.Unit
{
    public struct EnemyTurnComponent : IComponent
    {
        public EEnemyTurnStage stage;
        public AUnit target;
    }

    public enum EEnemyTurnStage
    {
        None = 0,
        LookingForTarget = 10,
        MoveToTarget = 20,
        AttackTarget = 30
    }
}