using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class CoroutineUtils
    {
        public static IEnumerator WaitUntil(Func<bool> until, Action after)
        {
            yield return new WaitUntil(until);
            after();
        }
    }
}