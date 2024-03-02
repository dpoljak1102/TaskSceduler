using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskSceduler.App.Core;
using TaskSceduler.App.Models;
using TaskSceduler.App.Service.Common;
using static TaskSceduler.App.Models.TaskModel;

namespace TaskSceduler.App.ViewModels
{
    public class CreateViewModel : ViewModelBase
    {

        #region ICommands

        //TODO :  User click 'Cancel', maybe better naming?
        public ICommand NavigateHomeViewCommand { get; set; }

        //User click 'Create' new task
        public ICommand CreateNewTaskCommand { get; set; }

        #endregion

        #region Bindings

        private ObservableCollection<string> _availablePriority;
        public ObservableCollection<string> AvailablePriority
        {
            get { return _availablePriority; }
            set { _availablePriority = value; OnPropertyChanged(); }
        }

        private string _subject;
        public string Subject
        {
            get { return _subject; }
            set { _subject = value;OnPropertyChanged();}
        }

        private string _projectType;
        public string ProjectType
        {
            get { return _projectType; }
            set { _projectType = value; OnPropertyChanged(); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }

        private string _priority;
        public string Priority
        {
            get { return _priority; }
            set { _priority = value; OnPropertyChanged(); }
        }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(); }
        }

        private DateTime _dueDate = DateTime.Now.AddDays(7);
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; OnPropertyChanged(); }
        }

        private string _percentageDone;
        public string PercentageDone
        {
            get { return _percentageDone; }
            set { _percentageDone = value; OnPropertyChanged(); }
        }

        private int _executionTime;
        public int ExecutionTime
        {
            get { return _executionTime; }
            set { _executionTime = value; OnPropertyChanged(); }
        }
        #endregion

        public CreateViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateHomeViewCommand = new RelayCommand(obj => { NavigationService.NavigateTo<HomeViewModel>(); });

            // TODO: better implementation
            AvailablePriority = new ObservableCollection<string>{"Low","Normal","High"};


            asyncMethods = new Func<TaskModel, Task>[]
            {
                AsyncMethodA,
                AsyncMethodB,
                AsyncMethodC,
                AsyncMethodD
            };

            CreateNewTaskCommand = new RelayCommand(obj => { NavigationService.NavigateTo<HomeViewModel>(CreateTask());});
        }


        private Random random = new Random();
        private Func<TaskModel, Task>[] asyncMethods;

        private TaskModel CreateTask()
        {
            int index = random.Next(0, asyncMethods.Length);

            Func<TaskModel, Task> selectedMethod = asyncMethods[index];

            TaskModel task = new TaskModel
            {
                TrackerId = Guid.NewGuid(),
                Name = GenerateRandomName(),
                TaskPriority = GenerateRandomPriority(),
                StartDate = StartDate,
                DueDate = DueDate,
                PercentageDone = 0,
            };

            task.TaskAction = async () => await selectedMethod(task);

            return task;
        }

        private async Task AsyncMethodA(TaskModel taskModel)
        {
            while (taskModel.PercentageDone != 100)
            {
                if (taskModel.CancellationTokenSource.IsCancellationRequested)
                    return;

                taskModel.PercentageDone = taskModel.PercentageDone + 15;
                await Task.Delay(2000);
            }
        }

        private async Task AsyncMethodB(TaskModel taskModel)
        {
            while (taskModel.PercentageDone != 100)
            {
                if (taskModel.CancellationTokenSource.IsCancellationRequested)
                    return;

                taskModel.PercentageDone = taskModel.PercentageDone + 15;
                await Task.Delay(1500);
            }
        }

        private async Task AsyncMethodC(TaskModel taskModel)
        {
            while (taskModel.PercentageDone != 100)
            {
                if (taskModel.CancellationTokenSource.IsCancellationRequested)
                    return;

                taskModel.PercentageDone = taskModel.PercentageDone + 15;
                await Task.Delay(500);
            }
        }

        private async Task AsyncMethodD(TaskModel taskModel)
        {
            while (taskModel.PercentageDone != 100)
            {
                if (taskModel.CancellationTokenSource.IsCancellationRequested)
                    return;

                taskModel.PercentageDone = taskModel.PercentageDone + 1;
                await Task.Delay(1000);
            }
        }

        private string GenerateRandomName()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            string randomString = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomString;
        }

        private Priority GenerateRandomPriority()
        {
            Random random = new Random();

            Array values = Enum.GetValues(typeof(Priority));

            int index = random.Next(values.Length);

            return (Priority)values.GetValue(index);
        }

    }
}
