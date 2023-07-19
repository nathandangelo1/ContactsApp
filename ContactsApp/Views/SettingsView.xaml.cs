using ContactsApp.Services;
using System.Windows;
using System.Windows.Controls;
using System.Linq;


namespace ContactsApp.Views
{
    public partial class SettingsView : UserControl
    {
        public bool SortByFirstTemp { get; set; }
        public bool SortByLastTemp { get; set; }
        public SettingsView()
        {
            InitializeComponent();

            //Get/Set initial Settings
            DataAccess db = new();
            var results2 = db.GetSettings();
            var sortby = results2.Where(x => x.SettingId == 1).ElementAt(0);
            var bdayRange = results2.Where(x => x.SettingId == 2).ElementAt(0);
            Settings.SortByFirstName = sortby.Value == "true" ? true : false;
            Settings.BirthdayRange = int.Parse(bdayRange.Value);
            
            //Update UI
            rbFirstName.IsChecked = Settings.SortByFirstName;
            rbLastName.IsChecked = !Settings.SortByFirstName;
            sliderBday.Value = Settings.BirthdayRange;
        }
        private void sliderBday_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)e.NewValue;
            txtSliderValue.Text = value.ToString();
            //Settings.BirthdayRange = value;
        }

        private void btnCleanUp_Click(object sender, RoutedEventArgs e)
        {
            ViewManager.SetView(View.Delete);
        }

        private void btnSaveExit_Click(object sender, RoutedEventArgs e)
        {
            Settings.SortByFirstName = (bool)rbFirstName.IsChecked;
            Settings.BirthdayRange = (int)sliderBday.Value;

            var db = new DataAccess();
            db.UpdateSettings();

            if (Contact.CurrentContact is null)
            {
                Contact.CurrentContact = MainWindow.GetRandomContact();
                ViewManager.SetView(View.Contact);
            }
            else ViewManager.SetView(View.Contact);
        }

        private void rbLastName_Checked(object sender, RoutedEventArgs e)
        {
            SortByLastTemp = (bool)rbLastName.IsChecked;
        }

        private void rbFirstName_Checked(object sender, RoutedEventArgs e)
        {
            SortByFirstTemp = (bool)rbFirstName.IsChecked;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Contact.CurrentContact is null)
            {
                Contact.CurrentContact = MainWindow.GetRandomContact();
                ViewManager.SetView(View.Contact);
            }
            else ViewManager.SetView(View.Contact);
        }
    }
}