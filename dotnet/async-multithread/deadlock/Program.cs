using System;
using System.Threading;

class Program
{
    static object lock1 = new object();
    static object lock2 = new object();

    static void Main(string[] args)
    {
        var t1 = new Thread(() => AcquireLocks(lock1, lock2));
        var t2 = new Thread(() => AcquireLocks(lock2, lock1));
        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();
    }

    static void AcquireLocks(object firstLock, object secondLock)
    {
        lock (firstLock)
        {
            Thread.Sleep(100); // Simulate work
            lock (secondLock)
            {
                Console.WriteLine("Locks acquired.");
            }
        }
    }
}
