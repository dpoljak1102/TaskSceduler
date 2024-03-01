using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using TaskSceduler.App.Core;
using TaskSceduler.App.Models;
using TaskSceduler.App.Service.Common;

namespace TaskSceduler.App.ViewModels
{
    public class HomeViewModel : ViewModelBase, IHandleParameters
    {
        #region Fileds

        private ObservableCollection<TaskModel> _taskCollections;
        public ObservableCollection<TaskModel> TaskCollections
        {
            get { return _taskCollections; }
            set { _taskCollections = value; OnPropertyChanged(); }
        }

        #endregion

        #region ICommands

        public ICommand NavigateCreateViewCommand { get; set; }

        public ICommand EnableViewCommand { get; set; }

        #endregion

        #region Bindings

        private int _maxConcurrentJobs = 1;
        public int MaxConcurrentJobs
        {
            get { return _maxConcurrentJobs; }
            set { _maxConcurrentJobs = value; OnPropertyChanged(); }
        }

        private bool _isEnabled = false;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; OnPropertyChanged(); }
        }

        #endregion


        public HomeViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            NavigateCreateViewCommand = new RelayCommand(obj => { NavigationService.NavigateTo<CreateViewModel>();});

            EnableViewCommand = new RelayCommand(obj => { IsEnabled = true; });

            TaskCollections = new ObservableCollection<TaskModel>
            {
                new TaskModel
                {
                    TrackerId = Guid.NewGuid(),
                    Subject = "Testing code A",
                    ProjectType = "A",
                    Status = "New",
                    Priority = "High",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7),
                    PercentageDone = 50,
                    State = TaskModel.TaskState.Finished
                },
                new TaskModel
                {
                    TrackerId = Guid.NewGuid(),
                    Subject = "Testing B",
                    ProjectType = "B",
                    Status = "New",
                    Priority = "High",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7),
                    PercentageDone = 50,
                },
                new TaskModel
                {
                    TrackerId = Guid.NewGuid(),
                    Subject = "Setup new task",
                    ProjectType = "C",
                    Status = "In Progress",
                    Priority = "Low",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(15),
                    PercentageDone = 75,
                    State = TaskModel.TaskState.Paused
                },
                new TaskModel
                {
                    TrackerId = Guid.NewGuid(),
                    Subject = "Testing code D",
                    ProjectType = "D",
                    Status = "New",
                    Priority = "Normal",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now,
                    PercentageDone = 100,
                    State = TaskModel.TaskState.Stopped
                },
            };

            // Only for GUI testing
            // TODO : need implement class libary for task sceduler
            var task1 = new TaskModel
            {
                Subject = "Currently working Z",
                ProjectType = "Z",
                Status = "In Progress",
                Priority = "High",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                PercentageDone = 0,
                State = TaskModel.TaskState.Running
            };

            // Only for GUI testing
            // TODO : need implement class libary for task sceduler
            TaskCollections.Add(task1);
            task1.StartUpdatingPercentage();
        }


        public void HandleParameters(object parameters)
        {
            if (parameters is TaskModel task && task != null)
            {
                TaskCollections.Add(parameters as TaskModel);
            }
        }
    }
}
