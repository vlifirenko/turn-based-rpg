using Scellecs.Morpeh;
using TurnBasedRPG.Model.Config;
using UnityEngine;

namespace TurnBasedRPG.View.Unit
{
    public class UnitView : AView
    {
        [SerializeField] private UnitConfig config;
        [SerializeField] private Animator animator;

        public Entity Entity { get; set; }

        public UnitConfig Config => config;

        public Animator Animator => animator;
    }
}