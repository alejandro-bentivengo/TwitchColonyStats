using UnityEngine;

namespace Colonystats.Utilities
{
    public static class TCText
    {
        public static string BigText(string str)
        {
            return $"<size=32>{str}</size>";
        }

        public static string ColoredText(string str, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{str}</color>";
        }
    }
}
