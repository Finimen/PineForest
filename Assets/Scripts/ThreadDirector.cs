using System;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Scripts
{
    public static class ThreadDirector
    {
        private static Stack<Thread> ThreadsPool = new Stack<Thread>(4);

        public static Thread GetThread(Action action)
        {
            return new Thread(() => action());
        }
    }
}