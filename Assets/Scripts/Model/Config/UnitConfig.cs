using TurnBasedRPG.Model.Enums;
using TurnBasedRPG.View.Unit;
using UnityEngine;
using UnityEngine.Serialization;

namespace TurnBasedRPG.Model.Config
{
    [CreateAssetMenu(fileName = "NewUnit", menuName = "Config/Unit")]
    public class UnitConfig : ScriptableObject
    {
        public UnitView prefab;
        public string id;
        public string name;
        public Sprite icon;
        [FormerlySerializedAs("type")] public EUnitClass unitClass;
        public int vita;
        public int defence;
        public int might;
        public int damageBonus;
        public int energy;
        public int stride;
        public int attacks = 1;
        public ItemConfig[] items;
    }
}