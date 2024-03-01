using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskSceduler.App.Models;

namespace TaskSceduler.App.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void ClickStart(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                TaskModel tm = button.DataContext as TaskModel;
                if (tm != null)
                {
                    tm.Start();
                }
            }
        }

        private void ClickPause(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                TaskModel tm = button.DataContext as TaskModel;
                if (tm != null)
                {
                    tm.Pause();
                }
            }
        }

        private void ClickStop(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                TaskModel tm = button.DataContext as TaskModel;
                if (tm != null)
                {
                    tm.Stop();
                }
            }
        }

        private void ClickDelete(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                TaskModel tm = button.DataContext as TaskModel;
                if (tm != null)
                {
                    
                }
            }
        }

        private void Initialize(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                button.IsEnabled = false;
                MaxConcurrentJobsNumber.IsReadOnly = true;
            }

        }
    }
}
