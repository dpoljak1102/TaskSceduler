using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TaskSceduler.App.Core;

namespace TaskSceduler.App.Models
{
    public class TaskModel : ObservableObject
    {
        public enum TaskState
        {
            NotStarted,
            Running,
            Paused,
            Stopped,
            Finished
        }

        public enum Priority
        {
            High,
            Normal,
            Low
        }

        private Guid _trackerId = Guid.NewGuid();
        public Guid TrackerId
        {
            get { return _trackerId; }
            set { _trackerId = value; OnPropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private Priority _taskPriority = Priority.Low;
        public Priority TaskPriority
        {
            get { return _taskPriority; }
            set { _taskPriority = value; OnPropertyChanged(); }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(); }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; OnPropertyChanged(); }
        }

        private int _percentageDone;
        public int PercentageDone
        {
            get { return _percentageDone; }
            set
            {
                if (value < 0)
                    _percentageDone = 0;
                else if (value >= 100)
                {
                    _percentageDone = 100;
                    State = TaskState.Finished;
                }
                else
                    _percentageDone = value;
                OnPropertyChanged();
            }
        }

        private TaskState _state = TaskState.NotStarted;
        public TaskState State
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged(); }
        }

        private int _executionTime;
        public int ExecutionTime
        {
            get { return _executionTime; }
            set { _executionTime = value; OnPropertyChanged(); }
        }

        private Func<Task> _taskAction;
        public Func<Task> TaskAction
        {
            get { return _taskAction; }
            set { _taskAction = value; OnPropertyChanged(); }
        }

        private CancellationTokenSource _cancellationTokenSource;
        public CancellationTokenSource CancellationTokenSource
        {
            get { return _cancellationTokenSource; }
            set { _cancellationTokenSource = value; OnPropertyChanged(); }
        }

        public async Task StartTaskActionAsync()
        {
            if (State == TaskState.Stopped || StartDate > DateTime.Now || DueDate < DateTime.Now)
                return;

            if (DueDate.Date == DateTime.Today)
            {
                TimeSpan adjustedExecutionTime = DueDate.TimeOfDay - DateTime.Now.TimeOfDay;
                if (adjustedExecutionTime.TotalMilliseconds < ExecutionTime)
                {
                    ExecutionTime = (int)adjustedExecutionTime.TotalMilliseconds;
                }
            }

            if (CancellationTokenSource == null || CancellationTokenSource.IsCancellationRequested)
            {
                CancellationTokenSource = new CancellationTokenSource(ExecutionTime);
            }

            State = TaskState.Running;

            var cancellationTokenSource = CancellationTokenSource = new CancellationTokenSource(ExecutionTime);

            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                await TaskAction?.Invoke();
            }
            finally
            {
                stopwatch.Stop();
                cancellationTokenSource.Dispose();
                if (stopwatch.ElapsedMilliseconds > ExecutionTime)
                    State = TaskState.Stopped;
                else if (stopwatch.ElapsedMilliseconds < ExecutionTime)
                    ExecutionTime -= ((int)stopwatch.ElapsedMilliseconds);
            }
        }

        public void StopTaskAction()
        {
            if (State == TaskState.Running)
            {
                this.CancellationTokenSource?.Cancel();
                State = TaskState.Stopped;
            }
            else if (State == TaskState.Paused)
                State = TaskState.Stopped;
        }

        public void PauseTaskAction()
        {
            if (State == TaskState.Running)
            {
                this.CancellationTokenSource?.Cancel();
                State = TaskState.Paused;
            }
        }
    }
}
