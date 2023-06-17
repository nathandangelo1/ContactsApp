using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContactsApp.Views
{
    public partial class SettingsView : UserControl
    {
        public int bdaySliderValue { get; set; } = 15;
        public bool IsFirstName 
        {
            get
            {
                return (bool)rbFirstName.IsChecked;
            }
        }
        
        public SettingsView()
        {
            InitializeComponent();

            Settings.BirthdayRange = bdaySliderValue;
            Settings.SortByFirstName = IsFirstName;
        }
        private void sliderBday_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = ((int)e.NewValue);
            txtSliderValue.Text = value.ToString();
            bdaySliderValue = value;
            Settings.BirthdayRange = bdaySliderValue;
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            Settings.SortByFirstName = IsFirstName;
        }

        private void btnCleanUp_Click(object sender, RoutedEventArgs e)
        {
            ViewSetter.ClearView(View.Settings);
            ViewSetter.SetView(View.Delete);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            ViewSetter.SetView(View.Home);
        }
    }
}
