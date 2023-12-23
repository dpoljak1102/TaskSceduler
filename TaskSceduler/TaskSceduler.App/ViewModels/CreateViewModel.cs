using System.Windows.Input;
using TaskSceduler.App.Core;
using TaskSceduler.App.Service.Common;

namespace TaskSceduler.App.ViewModels
{
    public class CreateViewModel : ViewModelBase
    {

        #region ICommands
        public ICommand NavigateHomeViewCommand { get; set; }

        #endregion

        public CreateViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateHomeViewCommand = new RelayCommand(obj => { NavigationService.NavigateTo<HomeViewModel>(); });
        }
    }
}
