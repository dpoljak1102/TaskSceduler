using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using TaskSceduler.App.Core;
using TaskSceduler.App.Models;
using TaskSceduler.App.Service.Common;
using static TaskSceduler.App.Models.TaskModel;

namespace TaskSceduler.App.ViewModels
{
    public class HomeViewModel : ViewModelBase, IHandleParameters
    {
        private ObservableCollection<TaskModel> _taskCollections;
        public ObservableCollection<TaskModel> TaskCollections
        {
            get { return _taskCollections; }
            set { _taskCollections = value; OnPropertyChanged(); }
        }

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get { return _selectedTask; }
            set { _selectedTask = value; OnPropertyChanged(); }
        }


        private SemaphoreSlim _semaphoreSlim;
        public SemaphoreSlim SemaphoreSlim
        {
            get { return _semaphoreSlim; }
            set { _semaphoreSlim = value; OnPropertyChanged(); }
        }

        private int _maxConcurrentJobs;
        public int MaxConcurrentJobs
        {
            get { return _maxConcurrentJobs; }
            set { _maxConcurrentJobs = value; OnPropertyChanged(); }
        }

        public ICommand NavigateCreateViewCommand { get; set; }
        public ICommand StartTaskCommand { get; set; }
        public ICommand StopTaskCommand { get; set; }
        public ICommand DeleteTaskCommand { get; set; }
        public ICommand StartAllTasksCommand { get; set; }

        public HomeViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateCreateViewCommand = new RelayCommand(obj => { NavigationService.NavigateTo<CreateViewModel>(); });
            TaskCollections = new ObservableCollection<TaskModel> { };
            _semaphoreSlim = new SemaphoreSlim(MaxConcurrentJobs);

            StartTaskCommand = new RelayCommand(
                async obj =>
                {
                    if (obj is TaskModel task)
                    {
                        await SemaphoreSlim.WaitAsync();
                        try
                        {
                            await task.StartTaskActionAsync();
                        }
                        finally
                        {
                            SemaphoreSlim.Release();
                        }
                    }
                }
            );

            StartAllTasksCommand = new RelayCommand(
               async obj =>
               {
                   await StartAllTasksAsync();
               }
           );

            StopTaskCommand = new RelayCommand(obj =>
            {
                if (obj is TaskModel task)
                {
                    task?.StopTaskAction();
                }

            });

            DeleteTaskCommand = new RelayCommand(obj =>
            {
                if (obj is TaskModel task)
                {
                    task?.StopTaskAction();

                    if (TaskCollections != null)
                        TaskCollections.Remove(task);
                }
            });
        }

        public void HandleParameters(object parameters)
        {
            if (parameters is TaskModel task && task != null)
            {
                TaskCollections.Add(parameters as TaskModel);
            }
        }


        private async Task StartAllTasksAsync()
        {
            await App.Current.Dispatcher.InvokeAsync(async () =>
            {
                SemaphoreSlim = new SemaphoreSlim(MaxConcurrentJobs);

                int totalTasks = TaskCollections.Count;
                int completedTasks = 0;

                while (completedTasks < totalTasks)
                {
                    var tasksToExecute = new List<TaskModel>();

                    // Uzmi sve taskove visokog prioriteta
                    var highPriorityTasks = TaskCollections
                        .Where(task => task.State == TaskState.NotStarted && task.TaskPriority == Priority.High)
                        .Take(MaxConcurrentJobs);

                    tasksToExecute.AddRange(highPriorityTasks);

                    // Ako nema dovoljno taskova visokog prioriteta, uzmi taskove normalnog prioriteta
                    if (tasksToExecute.Count < MaxConcurrentJobs)
                    {
                        var normalPriorityTasks = TaskCollections
                            .Where(task => task.State == TaskState.NotStarted && task.TaskPriority == Priority.Normal)
                            .Take(MaxConcurrentJobs - tasksToExecute.Count);

                        tasksToExecute.AddRange(normalPriorityTasks);
                    }

                    // Ako nema dovoljno ni taskova normalnog prioriteta, uzmi taskove niskog prioriteta
                    if (tasksToExecute.Count < MaxConcurrentJobs)
                    {
                        var lowPriorityTasks = TaskCollections
                            .Where(task => task.State == TaskState.NotStarted && task.TaskPriority == Priority.Low)
                            .Take(MaxConcurrentJobs - tasksToExecute.Count);

                        tasksToExecute.AddRange(lowPriorityTasks);
                    }

                    if (tasksToExecute.Count == 0)
                        break;

                    await SemaphoreSlim.WaitAsync();

                    try
                    {
                        var taskExecutions = tasksToExecute.Select(async task =>
                        {
                            await task.StartTaskActionAsync();
                            Interlocked.Increment(ref completedTasks);
                        });

                        await Task.WhenAll(taskExecutions);
                    }
                    finally
                    {
                        SemaphoreSlim.Release();
                    }
                }
            });
        }






    }
}
