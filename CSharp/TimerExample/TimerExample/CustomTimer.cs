using System;
using System.Diagnostics;
using System.Threading;

namespace TimerExample
{
    public class CustomTimer : IDisposable
    {
        private TimerCallback _callbackDelegate;
        private Thread _mainThread;

        // 0 for false, 1 for true
        private int _stopRequest;

        private const int WAIT = 10;
        private const int SECOND = 1000;

        private int _interval;

        public void Dispose()
        {
            Stop();
        }

        public void Stop()
        {
            Interlocked.Exchange(ref _stopRequest, 1);
        }

        public int GetInterval()
        {
            return _interval;
        }

        public CustomTimer(TimerCallback callBack)
        {
            _callbackDelegate = callBack;
        }

        public void Start(int intervalSeconds)
        {
            Interlocked.Exchange(ref _stopRequest, 0);

            _interval = intervalSeconds;

            _mainThread = new Thread(TimerLoop);
            _mainThread.IsBackground = true;
            _mainThread.Start(intervalSeconds * SECOND);
        }

        private void TimerLoop(object args)
        {
            int interval = (int)args;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                if (1 == Interlocked.Exchange(ref _stopRequest, 0))
                {
                    StopWaiting();
                    return;
                }

                Thread.Sleep(WAIT);

                long t = sw.ElapsedMilliseconds;

                if (t > interval)
                {
                    StopWaiting();
                    return;
                }
            }
            while (true);
        }

        private void StopWaiting()
        {
            _mainThread = null;
            Interlocked.Exchange(ref _stopRequest, 0);
            _callbackDelegate.Invoke(this);
        }
    }
}
