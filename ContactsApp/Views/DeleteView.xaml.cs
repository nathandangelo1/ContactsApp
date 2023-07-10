
using ContactsApp.Services;
using ContactsApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shell;

namespace ContactsApp.Views
{
    /// <summary>
    /// Interaction logic for DeleteView.xaml
    /// </summary>
    public partial class DeleteView : UserControl
    {
        //public ListCollectionView InactiveCollectionView;
        public Contact Selected;
        public CollectionViewSource InactiveViewSource;
        public DeleteView()
        {
            InitializeComponent();

            InactiveViewSource = new CollectionViewSource() { Source = MainWindow.CL.Contacts.Where(x => x.IsActive == 0) };
            InactiveViewSource.Filter += (s, e) =>
            {
                if (e.Item is Contact contact)
                {
                    if (contact.IsActive == 0)
                        e.Accepted = true;
                }
            };
            InactiveViewSource.IsLiveFilteringRequested = true;
            InactiveViewSource.SortDescriptions.Add(new SortDescription("NickName", ListSortDirection.Ascending));
            InactiveViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
            InactiveViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
            inactiveListView.ItemsSource = InactiveViewSource.View;

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = Selected;

            // Display a message box with Yes and No buttons and a question icon
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to permanently delete {contact.FirstName} {contact.LastName}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Check the result of the message box
            if (result == MessageBoxResult.Yes)
            {
                // If the user clicked Yes, perform the delete operation
                DataAccess da = new();
                da.DeleteContact(contact);

                MainWindow.CL.Contacts.Remove(contact);
                Selected = null;
                Clear();
                InactiveViewSource.View.Refresh();
                itemStackPanel.Visibility = Visibility.Hidden;

            }
        }
        private void Clear()
        {
            txtfullName.Text = "";
            imgContact.Source = null;
            txtPhone.Text = "";
            txtStreet.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtZip.Text = "";
            //itemStackPanel.Visibility = Visibility.Hidden;

        }

        private void btnPurgeAll_Click(object sender, RoutedEventArgs e)
        {
            List<Contact> contacts = new();

            // Display a message box with Yes and No buttons and a question icon
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to permanently delete ALL?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Check the result of the message box
            if (result == MessageBoxResult.Yes)
            {
                DataAccess da = new();
                // If the user clicked Yes, perform the delete operation
                foreach (var item in inactiveListView.Items)
                {
                    Contact contact = item as Contact;
                    contacts.Add(contact);
                }
                foreach (var contact in contacts)
                {
                    da.DeleteContact(contact);
                    MainWindow.CL.Contacts.Remove(contact);
                }
                Selected = null;
                InactiveViewSource.View.Refresh();

                itemStackPanel.Visibility = Visibility.Hidden;

            }
        }
        private void listView_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                itemStackPanel.Visibility = Visibility.Visible;
                var contact = item as Contact;
                Selected = contact;
                txtfullName.Text = (contact.FullName is not null) ? contact.FullName : null;
                imgContact.Source = (contact.Picture is not null) ? new BitmapImage(new Uri(Selected.Picture)) : null;
                txtPhone.Text = (contact.PhoneNumber is not null) ? contact.PhoneNumber : null;
                txtStreet.Text = (contact.Street is not null) ? contact.Street : null;
                txtCity.Text = (contact.City is not null) ? contact.City : null;
                txtStreet.Text = (contact.Street is not null) ? contact.Street : null;
                txtZip.Text = (contact.ZipCode is not null) ? contact.ZipCode : null;

            }
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = Selected;
            // If the user clicked Yes, perform the delete operation
            DataAccess da = new();
            contact.IsActive = 1;
            da.UpdateContact(contact);

            MainWindow.CL.Contacts[MainWindow.CL.Contacts.IndexOf(contact)].IsActive = 1;
            Selected = null;
            Clear();
            InactiveViewSource.View.Refresh();
            itemStackPanel.Visibility = Visibility.Hidden;
        }

        private void btnRestoreAll_Click(object sender, RoutedEventArgs e)
        {
            List<Contact> contacts = new();

            // Display a message box with Yes and No buttons and a question icon
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to restore ALL?", "Confirm Restore", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Check the result of the message box
            if (result == MessageBoxResult.Yes)
            {
                DataAccess da = new();
                // If the user clicked Yes, perform the delete operation
                foreach (var item in inactiveListView.Items)
                {
                    Contact contact = item as Contact;
                    contact.IsActive = 1;
                    contacts.Add(contact);
                }
                foreach (var contact in contacts)
                {
                    da.UpdateContact(contact);
                }
                Selected = null;
                InactiveViewSource.View.Refresh();
                itemStackPanel.Visibility = Visibility.Hidden;
            }
        }
    }
}
