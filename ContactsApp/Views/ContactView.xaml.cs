using ContactsApp.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ContactsApp.Views
{
    public partial class ContactView : UserControl
    {
        public ContactView()
        {
            InitializeComponent();
        }

        public void PopulateView()
        {
            Contact cc = Contact.CurrentContact;

            // If currentContact's property is not null, set the ContactView's equivalent control to value, else set it to null
            txtfullName.Text = (cc.FullName is not null) ? cc.FullName.Trim() : "";
            txtStreet.Text = (cc.Street is not null) ? cc.Street : "";
            txtCity.Text = (cc.City is not null) ? cc.City : "";
            txtState.Text = (cc.State is not null) ? cc.State : "";
            txtZip.Text = (cc.ZipCode is not null) ? cc.ZipCode : "";
            txtEmail.Text = (cc.Email is not null) ? cc.Email : "";
            txtPhone.Text = (cc.PhoneNumber is not null) ? cc.PhoneNumber : "";
            txtWebsite.Text = (cc.Website is not null) ? cc.Website : "";
            txtNotes.Text = (cc.Notes is not null) ? cc.Notes : "";
            try
            {
                imgContact.Source = new BitmapImage(new Uri(cc.Picture, UriKind.Absolute));
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Photo error");

                imgContact.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/noImage.png", UriKind.RelativeOrAbsolute));
            }
            if (imgContact.Source.ToString().Contains("noImage.png"))
            {
                imgContact.Height = 75;
                imgContact.Width = 75;
            }
            else
            {
                imgContact.Height = 200;
                imgContact.Width = 200;
            }

            if (WithinRange(cc.Birthday))
            {
                txtfullName.Text = txtfullName.Text + $"\U0001F382";
            }
        }
        private static bool WithinRange(DateTime? bday)
        {
            if (bday is null) return false;

            DateTime birthday = (DateTime)bday;

            // Get the current date
            DateTime currentDate = DateTime.Today;

            // Define the range boundaries
            int rangeInDays = Settings.BirthdayRange;
            DateTime rangeStart = currentDate.AddDays(-rangeInDays);
            DateTime rangeEnd = currentDate.AddDays(rangeInDays);

            // Check if the birthday's day of year is within the range
            return birthday.DayOfYear >= rangeStart.DayOfYear && birthday.DayOfYear <= rangeEnd.DayOfYear;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ViewManager.SetView(View.Edit);
        }

        public static event EventHandler OnListChange;

        public static void OnListChanged(string propertyName)
        {
            OnListChange?.Invoke(typeof(Settings), new PropertyChangedEventArgs(propertyName));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var contact = Contact.CurrentContact;

            // Display a message box with Yes and No buttons and a question icon
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {contact.FirstName} {contact.LastName}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Check the result of the message box
            if (result == MessageBoxResult.Yes)
            {
                // If the user clicked Yes, perform the delete operation
                //DeactivateContact(contact);
                if (contact is not null)
                {
                    DataAccess da = new();
                    da.DeactivateContact(contact);
                    MainWindow.CL.Contacts[MainWindow.CL.Contacts.IndexOf(contact)].IsActive = 0;
                }
                Contact.CurrentContact = null;
                ResetContactView();
                Contact.CurrentContact = MainWindow.GetRandomContact();
                ViewManager.SetView(View.Contact);
                OnListChanged(nameof(ContactView));
            }
        }


        public void ResetContactView()
        {
            foreach (var child in contactPanel.Children)
            {
                // Check if the child is a textblock
                if (child is TextBlock tb)
                {
                    // Clear or reset the textblock properties
                    tb.Text = "";
                }
                // Check if the child is another stackpanel
                else if (child is WrapPanel wp)
                {
                    // Loop through the grandchild elements
                    foreach (var grandchild in wp.Children)
                    {
                        // Check if the grandchild is a textblock
                        if (grandchild is TextBlock tb2)
                        {
                            // Clear or reset the textblock properties
                            tb2.Text = "";
                        }
                    }
                }
            }
        }
    }
}

