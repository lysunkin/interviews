using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        var scheduler = new LimitedConcurrencyLevelTaskScheduler(2);
        var factory = new TaskFactory(scheduler);

        for (int i = 0; i < 10; i++)
        {
            factory.StartNew(() =>
            {
                Console.WriteLine($"Task {Task.CurrentId} running on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
            });
        }

        Console.ReadLine();
    }
}

class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
{
    private readonly int _maxDegreeOfParallelism;
    private readonly LinkedList<Task> _tasks = new LinkedList<Task>();

    public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
    {
        _maxDegreeOfParallelism = maxDegreeOfParallelism;
    }

    protected override IEnumerable<Task> GetScheduledTasks()
    {
        return _tasks;
    }

    protected override void QueueTask(Task task)
    {
        _tasks.AddLast(task);
        if (ThreadPool.QueueUserWorkItem(ExecuteTasks))
        {
            // Task execution started
        }
    }

    private void ExecuteTasks(object _)
    {
        while (true)
        {
            Task task;
            lock (_tasks)
            {
                if (_tasks.Count == 0)
                {
                    break;
                }
                task = _tasks.First.Value;
                _tasks.RemoveFirst();
            }
            TryExecuteTask(task);
        }
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        return false;
    }
}
