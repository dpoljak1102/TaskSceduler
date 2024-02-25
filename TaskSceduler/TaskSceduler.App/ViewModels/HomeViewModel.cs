using System;
using System.Collections.ObjectModel;
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

        #endregion


        public HomeViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            NavigateCreateViewCommand = new RelayCommand(obj => { NavigationService.NavigateTo<CreateViewModel>();});


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
                    PercentageDone = 50
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
                    PercentageDone = 50
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
                    PercentageDone = 75
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
                    PercentageDone = 100
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
                PercentageDone = 0
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
