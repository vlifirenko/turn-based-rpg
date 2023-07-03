using UnityEngine;

namespace TurnBasedRPG.Services.Facade
{
    public static class FloatingText
    {
        public static UiService UiService { private get; set; }

        private static readonly Color _defaultColor = Color.white;

        public static void Show(Vector3 position, string text, Color color = default, float speed = 1f,
            float size = 1f)
            => UiService.ShowFloatingText(position, text, color, speed, size);
    }
}