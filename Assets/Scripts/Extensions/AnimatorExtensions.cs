using UnityEngine;

namespace TurnBasedRPG.Extensions
{
    public static class AnimatorExtensions
    {
        private static readonly int State = Animator.StringToHash("State");

        public static void SetState(this Animator animator, EAnimatorState state) => animator.SetInteger(State, (int) state);
    }

    public enum EAnimatorState
    {
        Idle = 0,
        IdleCombat = 1,
        Move = 2
    }
}