using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TaskSceduler.App.Service;
using TaskSceduler.App.Service.Common;
using TaskSceduler.App.ViewModels;

namespace TaskSceduler.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection service = new ServiceCollection();

            service.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            // TODO : AddTransient, AddScoped and AddSingleton Services Differences
            service.AddSingleton<MainViewModel>();
            service.AddSingleton<HomeViewModel>();
            service.AddTransient<CreateViewModel>();
            service.AddSingleton<InitViewModel>();

            service.AddSingleton<INavigationService, NavigationService>();
            service.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = service.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider?.GetRequiredService<MainWindow>();
            mainWindow?.Show();
            base.OnStartup(e);
        }
    }
}
