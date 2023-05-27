using System;
using TurnBasedRPG.Model.Enums;

namespace TurnBasedRPG.Model
{
    [Serializable]
    public class Damage
    {
        public int diceCount = 1;
        public EDice dice = EDice.D6;
        public int bonus;
    }
}