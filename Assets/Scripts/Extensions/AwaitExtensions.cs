using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class AwaitExtensions
    {
        public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
        {
            return Task.Delay(timeSpan).GetAwaiter();
        }

        public static TaskAwaiter GetAwaiter(this WaitUntil waitUntil)
        {
            return Task.Run(() =>
            {
                while (waitUntil.keepWaiting)
                    continue;
            }).GetAwaiter();
        }
    }
}