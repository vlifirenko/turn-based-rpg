using TurnBasedRPG.Model.Enums;
using UnityEngine;

namespace TurnBasedRPG.Model.Config
{
    [CreateAssetMenu(fileName = "NewUnit", menuName = "Config/Unit")]
    public class UnitConfig : ScriptableObject
    {
        public string id;
        public string name;
        public EUnitClass type;
        public int vita;
        public int defence;
        public int might;
        public int damageBonus;
        public int energy;
        public int stride;
    }
}