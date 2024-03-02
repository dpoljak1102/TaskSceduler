using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSceduler
{
    public class PriorityQueue
    {
        private readonly object lockObject = new object();
        private readonly Queue<Action> highPriorityTasks = new Queue<Action>();
        private readonly Queue<Action> normalPriorityTasks = new Queue<Action>();
        private readonly Queue<Action> lowPriorityTasks = new Queue<Action>();
        private int maxConcurrentTasks;

        public PriorityQueue(int maxConcurrentTasks)
        {
            this.maxConcurrentTasks = maxConcurrentTasks;
        }

        public void EnqueueTask(Action task, Priority priority)
        {
            lock (lockObject)
            {
                switch (priority)
                {
                    case Priority.High:
                        highPriorityTasks.Enqueue(task);
                        break;
                    case Priority.Normal:
                        normalPriorityTasks.Enqueue(task);
                        break;
                    case Priority.Low:
                        lowPriorityTasks.Enqueue(task);
                        break;
                }

                Monitor.PulseAll(lockObject);
            }
        }

        public void StartProcessing()
        {
            for (int i = 0; i < maxConcurrentTasks; i++)
            {
                Thread thread = new Thread(ProcessTasks);
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void ProcessTasks()
        {
            while (true)
            {
                Action task = null;

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

                if (task != null)
                {
                    task();
                }
            }
        }
    }

    public enum Priority
    {
        High,
        Normal,
        Low
    }
}
