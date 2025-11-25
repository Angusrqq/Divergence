using System;
using System.Threading.Tasks;
using UnityEngine;

public static class Utilities
{
    public static async Task AsyncAnimation<T>(float speed, T start, T end, Action<T, T, float> LerpCallback = null)
    {
        float elapsedTime = 0f;
        while (elapsedTime <= 1f)
        {
            elapsedTime += Time.deltaTime * speed;
            LerpCallback.Invoke(start, end, elapsedTime);
            await Awaitable.NextFrameAsync();
        }
    }
}
