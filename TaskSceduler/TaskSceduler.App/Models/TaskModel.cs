using System;
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

        private DateTime _startDate = DateTime.Now;
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
            if (CancellationTokenSource == null || CancellationTokenSource.IsCancellationRequested)
            {
                CancellationTokenSource = new CancellationTokenSource();
            }

            State = TaskState.Running;

            var cancellationTokenSource = CancellationTokenSource = new CancellationTokenSource();

            try
            {
                await TaskAction?.Invoke();
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }
        }

        public void StopTaskAction()
        {
            if (State == TaskState.Running)
            {
                this.CancellationTokenSource?.Cancel();
                State = TaskState.Stopped;
            }
        }
    }
}
