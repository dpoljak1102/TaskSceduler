using TaskSceduler.App.Service.Common;

namespace TaskSceduler.App.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
