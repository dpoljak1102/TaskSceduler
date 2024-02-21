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

namespace TaskSceduler.App.Views
{
    /// <summary>
    /// Interaction logic for CreateView.xaml
    /// </summary>
    public partial class CreateView : UserControl
    {
        public CreateView()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void beginTimeBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (beginTimeBox.Text == "HH-mm-ss")
            {
                beginTimeBox.Text = "";
                beginTimeBox.Foreground = Brushes.Black;
            }
        }

        private void deadlineTimeBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (deadlineTimeBox.Text == "HH-mm-ss")
            {
                deadlineTimeBox.Text = "";
                deadlineTimeBox.Foreground = Brushes.Black;
            }
        }

        private void deadlineTimeBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
