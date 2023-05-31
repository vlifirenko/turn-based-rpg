using System;
using TurnBasedRPG.Model.Enums;

namespace TurnBasedRPG.Model
{
    [Serializable]
    public class WeaponDamage
    {
        public int diceCount = 1;
        public EDice dice = EDice.D6;
        public int bonus;
    }
}