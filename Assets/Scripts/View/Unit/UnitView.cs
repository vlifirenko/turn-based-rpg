using TurnBasedRPG.Model.Config;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.View.Common;
using TurnBasedRPG.View.Item;
using UnityEngine;

namespace TurnBasedRPG.View.Unit
{
    public class UnitView : AView, IEquippableUnit
    {
        [SerializeField] private UnitConfig config;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject selected;

        public AUnit Unit { get; set; }

        public UnitConfig Config => config;

        public Animator Animator => animator;

        public GameObject Selected => selected;
        
        public void Equip(AEquipmentItemView item)
        {
        }

        public void Unequip(AEquipmentItemView item)
        {
        }
    }

    public interface IEquippableUnit
    {
        void Equip(AEquipmentItemView item);
        void Unequip(AEquipmentItemView item);
    }
}