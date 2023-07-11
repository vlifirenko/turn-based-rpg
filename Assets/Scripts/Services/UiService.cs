using System;
using DG.Tweening;
using Scellecs.Morpeh;
using TurnBasedRPG.Ecs.Components.Unit;
using TurnBasedRPG.Model.Item;
using TurnBasedRPG.Services.Facade;
using TurnBasedRPG.Signals;
using TurnBasedRPG.Utils;
using TurnBasedRPG.View.Canvas;
using TurnBasedRPG.View.Ui;
using UniRx;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG.Services
{
    public class UiService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly CanvasView _canvasView;
        private readonly BattleService _battleService;
        private readonly CharactersCanvasView _characterCanvasView;
        private readonly InventoryService _inventoryService;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public UiService(SignalBus signalBus, CanvasView canvasView, BattleService battleService,
            CharactersCanvasView characterCanvasView, InventoryService inventoryService)
        {
            _signalBus = signalBus;
            _canvasView = canvasView;
            _battleService = battleService;
            _characterCanvasView = characterCanvasView;
            _inventoryService = inventoryService;
        }

        public void Initialize()
        {
            _signalBus.GetStream<StrideChangedSignal>()
                .Subscribe(OnStrideChanged)
                .AddTo(_disposable);

            _signalBus.GetStream<UnitUpdatedSignal>()
                .Subscribe(OnUnitUpdated)
                .AddTo(_disposable);

            _signalBus.GetStream<AttacksLeftChangedSignal>()
                .Subscribe(OnAttacksLeftChanged)
                .AddTo(_disposable);

            _signalBus.GetStream<InventoryUpdatedSignal>()
                .Subscribe(OnInventoryUpdated)
                .AddTo(_disposable);

            _canvasView.NextTurnButton.OnClickAsObservable()
                .Subscribe(_ => _battleService.NextTurn())
                .AddTo(_disposable);

            // debug
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(x => _battleService.NextTurn())
                .AddTo(_disposable);
            //

            FloatText.UiService = this;
        }

        private void OnInventoryUpdated(InventoryUpdatedSignal signal)
        {
            // inventory
            var items = _inventoryService.GetAllInventoryItems();
            var inventoryView = _characterCanvasView.inventory;

            for (var i = 0; i < inventoryView.items.Length; i++)
            {
                var itemView = inventoryView.items[i];

                itemView.button.onClick.RemoveAllListeners();

                if (i < items.Length)
                {
                    itemView.icon.enabled = true;
                    itemView.icon.sprite = items[i].Icon;
                    itemView.title.text = items[i].Name;
                    var i1 = i;
                    itemView.button.onClick.AddListener(() => OnInventoryItemClick(items[i1]));
                }
                else
                {
                    itemView.icon.enabled = false;
                }
            }

            // equip
            var equipView = _characterCanvasView.slotContainer;
            var activeUnit = _battleService.ActiveUnit;

            if (activeUnit != null)
            {
                foreach (var slot in equipView.slots)
                {
                    var equipment = activeUnit.GetEquipmentInSlot(slot.slot);

                    if (equipment != null)
                    {
                        slot.icon.enabled = true;
                        slot.icon.sprite = equipment.Icon;
                        slot.button.onClick.AddListener(() => OnEquipmentItemClick(equipment));
                    }
                    else
                    {
                        slot.icon.enabled = false;
                        slot.button.onClick.RemoveAllListeners();
                    }
                }
            }
        }

        private void OnEquipmentItemClick(AItem item) => _inventoryService.EquipmentItemClick(item);

        private void OnInventoryItemClick(AItem item) => _inventoryService.InventoryItemClick(item);

        private void OnStrideChanged(StrideChangedSignal signal)
            => signal.unit.UiView.StrideText.text = $"Stride: {signal.value.PercentText}";

        private void OnUnitUpdated(UnitUpdatedSignal signal)
        {
            var unit = signal.unit;
            var vita = unit.Entity.GetComponent<VitaComponent>();
            var energy = unit.Entity.GetComponent<EnergyComponent>();
            var stride = unit.Entity.GetComponent<StrideComponent>();
            var attacksLeft = unit.Entity.GetComponent<AttacksLeftComponent>();
            var uiView = unit.UiView;

            uiView.Icon.sprite = unit.Icon;
            uiView.NameText.text = unit.Name;
            uiView.VitaSlider.value = vita.Value.Percent;
            uiView.VitaText.text = vita.Value.PercentText;
            uiView.EnergySlider.value = energy.Value.Percent;
            uiView.EnergyText.text = energy.Value.PercentText;
            uiView.StrideText.text = $"Stride: {stride.Value.PercentText}";
            uiView.AttacksText.text = $"Attacks: {attacksLeft.Value.PercentText}";
        }

        private void OnAttacksLeftChanged(AttacksLeftChangedSignal signal)
        {
            //todo _canvasView.UnitView.AttacksText.text = $"Attacks: {signal.value.PercentText}";
        }

        public void ShowFloatingText(Vector3 position, string text, Color color, float duration, float size)
        {
            var instance = _canvasView.FloatTextUiPool.GetItem() as UiFloatText;
            instance.Pool = _canvasView.FloatTextUiPool;

            var rectTransform = instance.GetComponent<RectTransform>();
            var anchoredPosition = _canvasView.Canvas.WorldToCanvasPosition(position, Camera.main);

            if (color == default)
                color = Color.white;

            rectTransform.anchoredPosition = anchoredPosition;
            instance.text.text = text;
            instance.text.color = color;
            instance.text.fontSize = size;

            var textRectTransform = instance.text.GetComponent<RectTransform>();
            textRectTransform.anchoredPosition = Vector2.zero;
            textRectTransform.DOAnchorPosY(60f, duration).onComplete = ()
                => instance.Pool.Recycle(instance);
            instance.text.DOFade(0f, duration);
        }

        public void Dispose() => _disposable?.Dispose();
    }
}