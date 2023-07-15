using ContactsApp.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ContactsApp.Views
{
    /// <summary>
    /// Interaction logic for ContactControl.xaml
    /// </summary>
    public partial class ContactView : UserControl
    {
        public ContactView()
        {
            InitializeComponent();
        }
        public static void PopulateView(ContactView ContactView)
        {
            Contact cc = Contact.CurrentContact;
            

            // If currentContact's property is not null, set the ContactView's equivalent control to value, else set it to null
            ContactView.txtfullName.Text = (cc.FullName is not null) ? cc.FullName.Trim() : "";
            ContactView.txtStreet.Text = (cc.Street is not null) ? cc.Street : "";
            ContactView.txtCity.Text = (cc.City is not null) ? cc.City : "";
            ContactView.txtState.Text = (cc.State is not null) ? cc.State : "";
            ContactView.txtZip.Text = (cc.ZipCode is not null) ? cc.ZipCode : "";
            ContactView.txtEmail.Text = (cc.Email is not null) ? cc.Email : "";
            ContactView.txtPhone.Text = (cc.PhoneNumber is not null) ? cc.PhoneNumber : "";
            ContactView.txtWebsite.Text = (cc.Website is not null) ? cc.Website : "";
            ContactView.txtNotes.Text = (cc.Notes is not null) ? cc.Notes : "";
            try
            {
                ContactView.imgContact.Source = new BitmapImage(new Uri(cc.Picture, UriKind.Absolute));
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Photo error");

                ContactView.imgContact.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/noImage.png", UriKind.RelativeOrAbsolute));
            }
            if (ContactView.imgContact.Source.ToString().Contains("noImage.png"))
            {
                ContactView.imgContact.Height = 75;
                ContactView.imgContact.Width = 75;
            }
            else
            {
                ContactView.imgContact.Height = 200;
                ContactView.imgContact.Width = 200;
            }

            if (WithinRange(cc.Birthday))
            {
                ContactView.txtfullName.Text = ContactView.txtfullName.Text + $"\U0001F382";
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
            ViewSetter.PopulateEditView();
            ViewSetter.SetView(View.Edit);
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
                DeactivateContact(contact);
                Contact.CurrentContact = null;
                //RefreshMainList();
                ResetContactView();
                ViewSetter.SetView(View.Home);
                OnListChanged(nameof(ContactView));
            }
        }

        private void DeactivateContact(Contact contact)
        {
            if (contact is not null)
            {
                DataAccess da = new();
                da.DeactivateContact(contact);
                MainWindow.CL.Contacts[MainWindow.CL.Contacts.IndexOf(contact)].IsActive = 0;
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

