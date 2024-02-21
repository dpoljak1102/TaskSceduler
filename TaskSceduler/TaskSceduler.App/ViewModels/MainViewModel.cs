using TaskSceduler.App.Service.Common;

namespace TaskSceduler.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigationService.NavigateTo<InitViewModel>(); //HomeViewModel
        }
    }
}
