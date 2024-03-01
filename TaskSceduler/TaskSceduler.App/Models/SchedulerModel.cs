using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSceduler.App.Core;

namespace TaskSceduler.App.Models
{
    internal class SchedulerModel : ObservableObject
    {
        private readonly PriorityQueue<TaskModel, int> taskQueue = new();

        private int _maxConcurrentJobs;
        public int MaxConcurrentJobs {  
            get { return _maxConcurrentJobs; } 
            set { _maxConcurrentJobs = value; OnPropertyChanged(); } }

        private int _currentlyRunningJobCount = 0;
        private readonly object _lock = new();

        public void Schedule(TaskModel task) //Mozda neki bolji nacin
        {
            task.OnFinished = HandleJobFinished;
            task.OnPaused = HandleJobPaused;
            task.OnStopped = HandleJobStopped;
            task.OnResumeRequested = HandleJobResumeRequest;
            RunTask(task);
        }


        public void RunTask(TaskModel task)
        {
            lock (_lock)
            {
                if (_currentlyRunningJobCount < _maxConcurrentJobs)
                {
                    _currentlyRunningJobCount++;
                    task.Start();
                }
                else
                {
                    switch (task.Priority)
                    {
                        case "Low":
                            taskQueue.Enqueue(task, 2);
                            break;
                        case "Medium":
                            taskQueue.Enqueue(task, 1);
                            break;
                        case "High":
                            taskQueue.Enqueue(task, 0);
                            break;
                    }
                }
            }
        }

        private void FreeJobSlot()
        {
            lock (_lock)
            {
                _currentlyRunningJobCount--;
                if (taskQueue.TryDequeue(out TaskModel task, out int priority))
                {
                    RunTask(task);
                }
            }
        }

        private void HandleJobFinished()
            => FreeJobSlot();

        private void HandleJobPaused()
            => FreeJobSlot();

        private void HandleJobStopped()
            => FreeJobSlot();

        private void HandleJobResumeRequest(TaskModel task)
            => RunTask(task);

    }
}
