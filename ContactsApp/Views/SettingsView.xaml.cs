using System.Windows;
using System.Windows.Controls;

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