using System;
using Framework.Extensions;
using Framework.Tools.Singleton;

namespace Framework.Utils
{
    public class Scheduler : MonoSingleton<Scheduler>
    {
        public static void Wait(float seconds, Action callback)
        {
            Instance.WaitForSeconds(seconds, callback);
        }
    }
}