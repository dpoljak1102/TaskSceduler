using System;
using System.Threading;
using System.Threading.Tasks;
using TaskSceduler.App.Core;
using TaskSceduler.App.Service.Common;

namespace TaskSceduler.App.Models
{
    public class TaskModel:ObservableObject
    {

        private Guid _trackerId = Guid.NewGuid();
        public Guid TrackerId
        {
            get { return _trackerId; }
            set { _trackerId = value; OnPropertyChanged(); }
        }

        private string _subject = "";
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; OnPropertyChanged(); }
        }

        private string _projectType = "";
        public string ProjectType
        {
            get { return _projectType; }
            set { _projectType = value; OnPropertyChanged(); }
        }

        private string _status = "";
		public string Status
		{
			get { return _status; }
			set { _status = value; OnPropertyChanged(); }
		}

		private string _priority = "";
		public string Priority
        {
			get { return _priority; }
			set { _priority = value; OnPropertyChanged(); }
		}

		private DateTime _startDate = DateTime.Now;
		public DateTime StartDate
		{
			get { return _startDate;}
			set { _startDate = value; OnPropertyChanged();}
		}

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; OnPropertyChanged();}
        }

        private int _percentageDone;
        public int PercentageDone
        {
            get { return _percentageDone; }
            set
            {
                if (value < 0)
                    _percentageDone = 0;
                else if (value > 100)
                    _percentageDone = 100;
                else
                    _percentageDone = value;
                OnPropertyChanged();
            }
        }

        private int _executionTime;
        public int ExecutionTime
        {
            get { return _executionTime; }
            set { _executionTime = value; OnPropertyChanged(); }
        }

        public enum TaskState
        {
            NotStarted,
            Running,
            Paused,
            Stopped,
            Finished
        }

        private TaskState _state = TaskState.NotStarted;
        public TaskState State {
            get { return _state; }
            set {  _state = value; OnPropertyChanged(); }
        }

        private readonly object _lock = new();
        internal Action OnPaused { get; set; } = () => { };
        internal Action OnStopped { get; set; } = () => { };
        internal Action OnFinished { get; set; } = () => { };
        internal Action<TaskModel> OnResumeRequested { get; set; } = (TaskModel task) => { };

        private IUserJob _job;
        public IUserJob Job { get { return _job; } set { _job = value; } }


        //Dummy task for later
        public async void StartUpdatingPercentage()
        {
            while (PercentageDone < 100) 
            {
                UpdatePercentageDoneAsync();
                await Task.Delay(1000); 
            }
            State = TaskState.Finished;
        }

        private void UpdatePercentageDoneAsync()
        {
            Random random = new Random();
            var randomPercentage = random.Next(5, 20);

            PercentageDone += randomPercentage;
         
            App.Current.Dispatcher.Invoke(() =>
            {
                OnPropertyChanged(nameof(PercentageDone));
            });
        }

        internal void Start() //Resume ujedno
        {
            lock (_lock)
            {
                switch (_state)
                {
                    case TaskState.NotStarted:
                        StartUpdatingPercentage();
                        State = TaskState.Running;
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            OnPropertyChanged(nameof(State));
                        });
                        break; //TODO
                    case TaskState.Running:
                        break; //Ignore if already running
                    case TaskState.Paused:
                        OnResumeRequested(this);
                        StartUpdatingPercentage();
                        State = TaskState.Running;
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            OnPropertyChanged(nameof(State));
                        });
                        break; //TODO
                    case TaskState.Stopped:
                        throw new InvalidOperationException("Cant start a task once its stopped!");
                    case TaskState.Finished:
                        throw new InvalidOperationException("Cant start a task that is already finished!");
                }
            }
        }

        internal void Pause()
        {
            lock (_lock)
            {
                switch (_state)
                {
                    case TaskState.NotStarted:
                        throw new InvalidOperationException("Cant pause a task that is not started!");
                    case TaskState.Running:
                        OnPaused();
                        State = TaskState.Paused;
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            OnPropertyChanged(nameof(State));
                        });
                        break; //TODO
                    case TaskState.Paused:
                        break; //Ignore
                    case TaskState.Stopped:
                        throw new InvalidOperationException("Cant pause a task that is stopped!");
                    case TaskState.Finished:
                        throw new InvalidOperationException("Cant pause a task that is already finished!");
                }
            }
        }

        internal void Stop()
        {
            lock (_lock)
            {
                switch (_state)
                {
                    case TaskState.NotStarted:
                        throw new InvalidOperationException("Cant stop a task that is not started!");
                    case TaskState.Running:
                        OnStopped();
                        State = TaskState.Stopped;
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            OnPropertyChanged(nameof(State));
                        });
                        break; //TODO
                    case TaskState.Paused:
                        OnStopped();
                        this.State = TaskState.Stopped;
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            OnPropertyChanged(nameof(State));
                        });
                        break; //TODO
                    case TaskState.Stopped:
                        break; //Ignore
                    case TaskState.Finished:
                        throw new InvalidOperationException("Cant stop a task that is finished!");
                }
            }
        }
    }
}
