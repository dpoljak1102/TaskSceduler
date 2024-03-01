using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskSceduler.App.Core;
using TaskSceduler.App.Models;
using TaskSceduler.App.Service.Common;

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

            // Here we sent some data, we don't care what it is inside!
            // Logic of the HomeViewModel is to process that data
            CreateNewTaskCommand = new RelayCommand(obj => { NavigationService.NavigateTo<HomeViewModel>(
                new TaskModel
                {
                    // TODO : match GUI and this bindings for all fields

                    // This get from View/Gui data and bind here 
                    TrackerId = Guid.NewGuid(),
                    Subject = Subject,
                    ProjectType = "B",
                    Status = "New",
                    Priority = Priority,
                    StartDate = StartDate,
                    DueDate = DueDate,
                    PercentageDone = 50,
                    ExecutionTime = ExecutionTime
                });
            });
        }
    }
}
