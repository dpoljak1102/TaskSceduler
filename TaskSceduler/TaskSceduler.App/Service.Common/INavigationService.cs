using TaskSceduler.App.ViewModels;

namespace TaskSceduler.App.Service.Common
{
    /// <summary>
    /// IService for navigating between different ViewModels in the application.
    /// </summary>
    public interface INavigationService
    {
        ViewModelBase CurrentView { get; }
        void NavigateTo<T>() where T : ViewModelBase;
        void NavigateTo<T>(object parameters) where T : ViewModelBase;
    }
}
