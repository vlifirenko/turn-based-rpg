using UnityEngine;
using UnityEngine.AI;

namespace TurnBasedRPG.View.Unit
{
    public class DiabloJuniorView : UnitView
    {
        [SerializeField] private NavMeshAgent navMeshAgent;

        public NavMeshAgent NavMeshAgent => navMeshAgent;
    }
}