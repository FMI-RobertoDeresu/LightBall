using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class DelayUtils
    {
        public static void Wait(string label = "", int powerOfTwo = 30)
        {
            Debug.Log($"Wait {label} start".Replace("  ", " "));
            var i = 0L;
            while (i < 1L << powerOfTwo)
                i++;
            Debug.Log($"Wait {label} ({i}) end".Replace("  ", " "));
        }
    }
}