using System.Windows.Input;
using TaskSceduler.App.Core;
using TaskSceduler.App.Service.Common;

namespace TaskSceduler.App.ViewModels
{
    public class InitViewModel : ViewModelBase
    {
        #region Fileds

        private string _maxConJobs;
        public string MaxConJobs { get; set; }

        #endregion

        #region ICommands
        public ICommand NavigateHomeViewCommand { get; set; }

        #endregion

        public InitViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateHomeViewCommand = new RelayCommand(obj => { NavigationService.NavigateTo<HomeViewModel>(); });
        }
    }
}