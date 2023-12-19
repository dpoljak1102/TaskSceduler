using System;
using System.Collections.ObjectModel;
using TaskSceduler.App.Models;
using TaskSceduler.App.Service.Common;

namespace TaskSceduler.App.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Fileds

        private ObservableCollection<TaskModel> _taskCollections;
        public ObservableCollection<TaskModel> TaskCollections
        {
            get { return _taskCollections; }
            set { _taskCollections = value; OnPropertyChanged(); }
        }
        
        #endregion

        public HomeViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            TaskCollections = new ObservableCollection<TaskModel> 
            {
                new TaskModel
                {
                    TrackerId = 1,
                    ProjectType = "Feature",
                    Status = "In Progress",
                    Priority = "High",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7),
                    PercentageDone = 50
                },
                new TaskModel
                {
                    TrackerId = 2,
                    ProjectType = "Feature",
                    Status = "In Progress",
                    Priority = "Low",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(15),
                    PercentageDone = 0
                },
            };
        }

       
    }
}
