
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
        public ListCollectionView InactiveCollectionView;
        public Contact Selected;
        public DeleteView()
        {
            InitializeComponent();
            
            InactiveCollectionView = new ListCollectionView(MainWindow.CL.Contacts);

            var sortDescription1 = new SortDescription("NickName", ListSortDirection.Ascending);
            var sortDescription2 = new SortDescription("FirstName", ListSortDirection.Ascending);
            var sortDescription3 = new SortDescription("LastName", ListSortDirection.Ascending);
            //InactiveCollectionView = CollectionViewSource.GetDefaultView(inactiveListView.ItemsSource);
            InactiveCollectionView.SortDescriptions.Add(sortDescription1);
            InactiveCollectionView.SortDescriptions.Add(sortDescription2);
            InactiveCollectionView.SortDescriptions.Add(sortDescription3);


            InactiveCollectionView.Filter = item => (item as Contact).IsActive == 0;
            inactiveListView.ItemsSource = InactiveCollectionView;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var contact = Selected;

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
                itemStackPanel.Visibility = Visibility.Hidden;

            }
            else
            {
                // If the user clicked No, cancel the delete operation
                // CancelDelete();
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
            itemStackPanel.Visibility = Visibility.Hidden;

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
                itemStackPanel.Visibility = Visibility.Hidden;

            }
        }
        private void listView_Click(object sender, RoutedEventArgs e)
        {
            itemStackPanel.Visibility = Visibility.Visible;
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
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
            da.UpdateContact(contact);

            MainWindow.CL.Contacts[MainWindow.CL.Contacts.IndexOf(contact)].IsActive = 1;
            Selected = null;
            Clear();
            itemStackPanel.Visibility = Visibility.Hidden;
        }
    }
}
