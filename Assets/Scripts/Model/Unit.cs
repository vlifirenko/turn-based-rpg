using System;
using TurnBasedRPG.Model.Enums;

namespace TurnBasedRPG.Model
{
    [Serializable]
    public class Unit
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