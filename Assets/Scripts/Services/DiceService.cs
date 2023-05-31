using TurnBasedRPG.Model.Enums;
using UnityEngine;

namespace TurnBasedRPG.Services
{
    public class DiceService
    {
        public int RollDice(EDice dice, int amount = 1)
        {
            var result = 0;
            var diceEdged = (int) dice;
            
            for (var i = 0; i < amount; i++)
                result += Random.Range(1, diceEdged + 1);

            return result;
        }
    }
}