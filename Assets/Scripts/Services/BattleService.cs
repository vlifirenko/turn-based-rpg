using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model;
using TurnBasedRPG.Model.Map;
using TurnBasedRPG.Model.Unit;
using TurnBasedRPG.Signals;
using TurnBasedRPG.View;
using UniRx;
using Zenject;

namespace TurnBasedRPG.Services
{
    public class BattleService : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly SignalBus _signalBus;

        public AUnit ActiveUnit { get; private set; }

        private BattleData _battleData;
        private readonly List<AUnit> _allUnits = new List<AUnit>();

        public BattleData BattleData => _battleData;

        public BattleService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _battleData = new BattleData();

            _signalBus.GetStream<NextTurnSignal>()
                .Subscribe(_ => NextTurn())
                .AddTo(_disposable);
        }

        public void AddUnit(AUnit unit) => _allUnits.Add(unit);

        public void SelectUnit(AUnit unit)
        {
        }

        public void MoveTo(Cell targetCell) => ActiveUnit.MoveTo(targetCell);

        public void Attack(Cell targetCell)
        {
            var target = targetCell.Content as AUnit;
            if (target.IsPlayer)
                return;

            ActiveUnit.Attack(target);
        }

        public void NextTurn()
        {
            if (ActiveUnit != null)
            {
                ref var stride = ref ActiveUnit.Entity.GetComponent<StrideComponent>();
                stride.Value.SetMax();
                ref var attackLeft = ref ActiveUnit.Entity.GetComponent<AttacksLeftComponent>();
                attackLeft.Value.SetMax();

                var currentUnitIndex = _allUnits.IndexOf(ActiveUnit);
                currentUnitIndex += 1;
                currentUnitIndex %= _allUnits.Count;

                SetActiveUnit(_allUnits[currentUnitIndex]);
            }
            else
            {
                _allUnits.Sort((unit, aUnit) => unit.Initiative.CompareTo(aUnit.Initiative));
                SetActiveUnit(_allUnits[0]);
            }

            ActiveUnit.StartTurn();
            _signalBus.Fire(new SetActiveUnitSignal(ActiveUnit));
        }

        private void SetActiveUnit(AUnit activeUnit)
        {
            ActiveUnit?.Deselect();
            ActiveUnit = activeUnit;
            ActiveUnit.Select();
            
            _signalBus.Fire(new InventoryUpdatedSignal());
        }

        public void Dispose() => _disposable?.Dispose();
    }
}