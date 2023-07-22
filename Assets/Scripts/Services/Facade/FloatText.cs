using UnityEngine;

namespace TurnBasedRPG.Services.Facade
{
    public static class FloatText
    {
        public static UiService UiService { private get; set; }

        public static void Show(Vector3 position, string text, Color color = default, float speed = 1f, float size = 36f)
            => UiService.ShowFloatingText(position, text, color, speed, size);
    }
}