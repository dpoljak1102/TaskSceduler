using System;
using TaskSceduler.App.Core;
using TaskSceduler.App.Service.Common;
using TaskSceduler.App.ViewModels;

namespace TaskSceduler.App.Service
{
    public class NavigationService : ObservableObject, INavigationService
    {
        private ViewModelBase? _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView!;
            set { _currentView = value; OnPropertyChanged(); }
        }

        // The factory function used to create instances of ViewModelBase objects
        // This is injected via the constructor
        private readonly Func<Type, ViewModelBase> _viewModelFactory;

        public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        /// <summary>
        /// Navigates to the specified ViewModel of type T.
        /// </summary>
        /// <typeparam name="T">Type of the ViewModel to navigate to.</typeparam>
        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var viewModel = _viewModelFactory(typeof(TViewModel));

            if (viewModel is IHandleParameters handleParameters)
            {
                handleParameters.HandleParameters(null);
            }

            CurrentView = viewModel;
        }

        /// <summary>
        /// Navigates to the specified ViewModel of type T with the given parameters.
        /// </summary>
        /// <typeparam name="T">Type of the ViewModel to navigate to.</typeparam>
        /// <param name="parameters">Parameters to pass to the ViewModel.</param>
        public void NavigateTo<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            var viewModel = _viewModelFactory(typeof(TViewModel));

            if (viewModel is IHandleParameters handleParameters)
            {
                handleParameters.HandleParameters(parameter);
            }

            CurrentView = viewModel;
        }
    }
}
