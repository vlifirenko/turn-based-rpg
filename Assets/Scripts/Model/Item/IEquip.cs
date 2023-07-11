using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Model.Item
{
    public interface IEquip
    {
        AItem Equip(AUnit owner);
        void Unequip();
    }
}