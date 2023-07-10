using TurnBasedRPG.Model.Unit;

namespace TurnBasedRPG.Model.Item
{
    public interface IEquip
    {
        void Equip(AUnit owner);
        void Unequip();
    }
}