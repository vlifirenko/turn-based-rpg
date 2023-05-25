using TurnBasedRPG.Model.Config;
using UnityEngine;

namespace TurnBasedRPG.View.Unit
{
    public class UnitView : AView
    {
        [SerializeField] private UnitConfig config;

        public UnitConfig Config => config;
    }
}