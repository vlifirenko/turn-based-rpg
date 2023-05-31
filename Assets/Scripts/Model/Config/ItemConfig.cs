using TurnBasedRPG.Model.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace TurnBasedRPG.Model.Config
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Config/Item")]
    public class ItemConfig : ScriptableObject
    {
        public string id;
        public string name;
        [FormerlySerializedAs("damage")] public WeaponDamage weaponDamage;
        public bool isTwoHanded;
        public EItemDistance distance;
        public int cost;
        public int charges;
        public EItemSlot slot;
        public string description;
    }
}