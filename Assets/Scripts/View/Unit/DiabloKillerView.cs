using UnityEngine;

namespace TurnBasedRPG.View.Unit
{
    public class DiabloKillerView : UnitView
    {
        [SerializeField] private CharacterController characterController;

        public CharacterController CharacterController => characterController;
    }
}