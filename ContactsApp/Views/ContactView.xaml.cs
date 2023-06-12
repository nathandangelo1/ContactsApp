using ContactsApp.Services;
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

namespace ContactsApp.Views
{
    /// <summary>
    /// Interaction logic for ContactControl.xaml
    /// </summary>
    public partial class ContactView : UserControl
    {
        //public static Contact CurrentContact;
        public ContactView()
        {
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ViewSetter.SetView(View.Edit);
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
                RefreshMainList();
                ResetContactView();
                ViewSetter.SetView(View.Home);
            }
            else
            {
                // If the user clicked No, cancel the delete operation
               // CancelDelete();
            }
        }
        private void DeactivateContact(Contact contact)
        {
            if (contact is not null)
            {
                DataAccess da = new();
                da.DeactivateContact(contact);
                Contact.contacts.Remove(contact);
            }
        }
        private void RefreshMainList()
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            Contact.contacts = Contact.contacts.OrderBy(x => x.FirstName).ToList();
            Contact.favorites = Contact.favorites.OrderBy(x=>x.FirstName).ToList();
            window.RefreshListView();
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
                            //tb2.Foreground = Brushes.Black;
                            //tb2.FontWeight = FontWeights.Normal;
                        }
                    }
                }
            }
        }
    }
}

