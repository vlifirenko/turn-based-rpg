using UnityEngine;

namespace TurnBasedRPG.Model.Config
{
    [CreateAssetMenu(fileName = "NewUnit", menuName = "Config/Unit")]
    public class UnitConfig : ScriptableObject
    {
        public Unit unit;
    }
}