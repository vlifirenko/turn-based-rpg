using System;
using TurnBasedRPG.Model.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TurnBasedRPG.Model.Config
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Config/Item")]
    public class ItemConfig : ScriptableObject
    {
        public string id;
        public string name;
        public WeaponDamage weaponDamage;
        public bool isTwoHanded;
        public EItemDistance distance;
        public int cost;
        public int charges;
        public int range;
        public EItemSlot slot;
        public string description;
        public ItemEffect[] effects;
        public Sprite icon;
    }

    [Serializable]
    public class ItemEffect
    {
        public EItemEffectType type;
        public EItemEffectStat stat;
        public float value;
        public int duration;
    }

    public enum EItemEffectType
    {
        None = 0,
        Recovery = 10,
        Buff = 20,
        Debuff = 30
    }

    public enum EItemEffectStat
    {
        None= 0,
        Vita = 10,
        Might = 20,
        Damage = 30,
        Defence = 40
    }
}