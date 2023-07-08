using Scellecs.Morpeh;
using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.View.Common;
using UnityEngine;

namespace TurnBasedRPG.View.Unit
{
    public class UnitView : AView
    {
        [SerializeField] private UnitConfig config;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject selected;

        public AUnit Unit { get; set; }

        public UnitConfig Config => config;

        public Animator Animator => animator;

        public GameObject Selected => selected;
    }
}