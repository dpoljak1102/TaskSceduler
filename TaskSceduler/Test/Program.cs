using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class PriorityQueue
{
    private readonly object lockObject = new object();
    private readonly Queue<TaskWithPriority> highPriorityTasks = new Queue<TaskWithPriority>();
    private readonly Queue<TaskWithPriority> normalPriorityTasks = new Queue<TaskWithPriority>();
    private readonly Queue<TaskWithPriority> lowPriorityTasks = new Queue<TaskWithPriority>();
    private readonly List<Thread> threads = new List<Thread>();
    private int threadCount;

    public PriorityQueue(int threadCount)
    {
        this.threadCount = threadCount;
        InitializeThreads();
    }

    private void InitializeThreads()
    {
        for (int i = 0; i < threadCount; i++)
        {
            Thread thread = new Thread(ExecuteTasks);
            thread.Start();
            threads.Add(thread);
        }
    }

    public void EnqueueTask(Action task, Priority priority)
    {
        TaskWithPriority taskWithPriority = new TaskWithPriority(task, priority);
        lock (lockObject)
        {
            switch (priority)
            {
                case Priority.High:
                    highPriorityTasks.Enqueue(taskWithPriority);
                    break;
                case Priority.Normal:
                    normalPriorityTasks.Enqueue(taskWithPriority);
                    break;
                case Priority.Low:
                    lowPriorityTasks.Enqueue(taskWithPriority);
                    break;
                default:
                    throw new ArgumentException("Invalid priority");
            }
            Monitor.PulseAll(lockObject);
        }
    }

    private void ExecuteTasks()
    {
        while (true)
        {
            TaskWithPriority task = null;

            lock (lockObject)
            {
                while (highPriorityTasks.Count == 0 && normalPriorityTasks.Count == 0 && lowPriorityTasks.Count == 0)
                {
                    Monitor.Wait(lockObject);
                }

                if (highPriorityTasks.Count > 0)
                {
                    task = highPriorityTasks.Dequeue();
                }
                else if (normalPriorityTasks.Count > 0)
                {
                    task = normalPriorityTasks.Dequeue();
                }
                else if (lowPriorityTasks.Count > 0)
                {
                    task = lowPriorityTasks.Dequeue();
                }
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            task.Action.Invoke();
            stopwatch.Stop();
            Console.WriteLine($"Task completed in {stopwatch.ElapsedMilliseconds} milliseconds.");
        }
    }

    public void Stop()
    {
        foreach (var thread in threads)
        {
            thread.Abort();
        }
    }
}

public class TaskWithPriority
{
    public Action Action { get; }
    public Priority Priority { get; }

    public TaskWithPriority(Action action, Priority priority)
    {
        Action = action;
        Priority = priority;
    }
}

public enum Priority
{
    High,
    Normal,
    Low
}

class Program
{
    static void Main(string[] args)
    {
        PriorityQueue priorityQueue = new PriorityQueue(2);

        priorityQueue.EnqueueTask(() => Task1(), Priority.High);
        priorityQueue.EnqueueTask(() => Task2(), Priority.Normal);
        priorityQueue.EnqueueTask(() => Task3(), Priority.Low);

        Console.ReadLine();

        priorityQueue.Stop();
    }

    static void Task1()
    {
        Console.WriteLine("Starting Task 1...");
        Thread.Sleep(5000); // Simulate task taking 3 seconds
        Console.WriteLine("Task 1 completed.");
    }

    static void Task2()
    {
        Console.WriteLine("Starting Task 2...");
        Thread.Sleep(5000); // Simulate task taking 2 seconds
        Console.WriteLine("Task 2 completed.");
    }

    static void Task3()
    {
        Console.WriteLine("Starting Task 3...");
        Thread.Sleep(5000); // Simulate task taking 1 second
        Console.WriteLine("Task 3 completed.");
    }
}
