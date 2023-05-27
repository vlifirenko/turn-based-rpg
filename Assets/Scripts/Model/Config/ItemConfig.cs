using TurnBasedRPG.Model.Enums;
using UnityEngine;

namespace TurnBasedRPG.Model.Config
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Config/Item")]
    public class ItemConfig : ScriptableObject
    {
        public string id;
        public string name;
        public Damage damage;
        public bool isTwoHanded;
        public EItemDistance distance;
        public int cost;
        public int charges;
        public EItemSlot slot;
        public string description;
    }
}