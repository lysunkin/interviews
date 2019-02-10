using System;
using System.Threading;

namespace TimerExample
{
    class Program
    {
        public static void Main()
        {
            Program ex = new Program();

            ex.StartCustomTimer(); // <- this one must exit after 10 seconds
            ex.StartCustomTimerAndStop(); // <- this one must exit after 3 seconds

            Console.WriteLine("Please wait for the timer message...");
            Console.ReadLine();
        }

        public void StartCustomTimer()
        {
            CustomTimer t = new CustomTimer(new TimerCallback(CustomTimerProc));
            t.Start(10);
        }

        public void StartCustomTimerAndStop()
        {
            CustomTimer t = new CustomTimer(new TimerCallback(CustomTimerProc));
            t.Start(10);
            // wait 2 seconds and stop it
            Thread.Sleep(2000);
            t.Stop();
        }

        private void CustomTimerProc(object state)
        {
            // The state object is the Timer object.
            CustomTimer t = (CustomTimer)state;
            Console.WriteLine("The custom timer with interval " + t.GetInterval() + " callback finished");
            t.Dispose();
        }
    }
}
