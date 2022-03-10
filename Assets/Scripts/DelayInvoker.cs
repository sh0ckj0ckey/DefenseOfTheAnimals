using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class DelayInvoker : MonoBehaviour
    {
        public static IEnumerator DelayToInvoke(Action action, float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            action();
        }
    }
}
