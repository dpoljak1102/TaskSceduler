using TaskSceduler.App.Core;
using TaskSceduler.App.Service.Common;

namespace TaskSceduler.App.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        private INavigationService? _navigationService;

        public INavigationService? NavigationService
        {
            get => _navigationService;
            set { _navigationService = value; OnPropertyChanged(); }
        }
    }
}
