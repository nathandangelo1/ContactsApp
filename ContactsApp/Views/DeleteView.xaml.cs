
using ContactsApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ContactsApp.Views
{

    public partial class DeleteView : UserControl
    {
        public Contact Selected;
        public CollectionViewSource InactiveViewSource;
        public static event EventHandler OnListChange;

        public static void OnListChanged(string propertyName)
        {
            OnListChange?.Invoke(typeof(Settings), new PropertyChangedEventArgs(propertyName));
        }
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
            InactiveViewSource.SortDescriptions.Add(new SortDescription("GroupingName", ListSortDirection.Ascending));
            inactiveListView.ItemsSource = InactiveViewSource.View;

        }
        public void PopulateView()
        {
            Contact cc = Selected;

            if (cc is not null)
            {
                // If currentContact's property is not null, set the ContactView's equivalent control to value, else set it to null
                txtfullName.Text = (cc.FullName is not null) ? cc.FullName.Trim() : "";
                txtPhone.Text = (cc.PhoneNumber is not null) ? cc.PhoneNumber : "";
                txtStreet.Text = (cc.Street is not null) ? cc.Street : "";
                txtCity.Text = (cc.City is not null) ? cc.City : "";
                txtState.Text = (cc.State is not null) ? cc.State : "";
                txtZip.Text = (cc.ZipCode is not null) ? cc.ZipCode : "";
                try
                {
                    imgContact.Source = new BitmapImage(new Uri(cc.Picture, UriKind.Absolute));
                }
                catch
                {
                    MessageBoxResult result = MessageBox.Show("Photo error");

                    imgContact.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/noImage.png", UriKind.RelativeOrAbsolute));
                }
                // if stock image, make smaller
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
            }
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
                OnListChanged(nameof(DeleteView));
            }
        }
        // Sets all fields to null/empty
        private void Clear()
        {
            txtfullName.Text = "";
            imgContact.Source = null;
            txtPhone.Text = "";
            txtStreet.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtZip.Text = "";
        }
        // Permanently deletes all inactive contacts
        private void btnPurgeAll_Click(object sender, RoutedEventArgs e)
        {
            List<Contact> contacts = new();

            // Display a message box with Yes and No buttons and a question icon
            MessageBoxResult result = MessageBox.Show($"Purge ALL deleted? Are you sure?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

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
                OnListChanged(nameof(DeleteView));
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
                //imgContact.Source = (contact.Picture is not null) ? new BitmapImage(new Uri(Selected.Picture)) : null;
                txtPhone.Text = (contact.PhoneNumber is not null) ? contact.PhoneNumber : null;
                txtStreet.Text = (contact.Street is not null) ? contact.Street : null;
                txtCity.Text = (contact.City is not null) ? contact.City : null;
                txtStreet.Text = (contact.Street is not null) ? contact.Street : null;
                txtZip.Text = (contact.ZipCode is not null) ? contact.ZipCode : null;
                try
                {
                    imgContact.Source = new BitmapImage(new Uri(contact.Picture, UriKind.Absolute));
                }
                catch
                {
                    MessageBoxResult result = MessageBox.Show("Photo error");

                    imgContact.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/noImage.png", UriKind.RelativeOrAbsolute));
                }
                // if stock image, make smaller
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

            }
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = Selected;
            // If the user clicked Yes, perform the delete operation
            DataAccess da = new();
            // Set local copy to active
            contact.IsActive = 1;
            // Update db
            da.UpdateContact(contact);
            //Update List
            MainWindow.CL.Contacts[MainWindow.CL.Contacts.IndexOf(contact)].IsActive = 1;
            Selected = null;
            Clear();
            InactiveViewSource.View.Refresh();
            itemStackPanel.Visibility = Visibility.Hidden;
            OnListChanged(nameof(DeleteView));
        }

        private void btnRestoreAll_Click(object sender, RoutedEventArgs e)
        {
            // Used to tally up all Contacts in inactiveListView
            List<Contact> contacts = new();

            // Display a message box with Yes and No buttons and a question icon
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to restore ALL?", "Confirm Restore", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Check the result of the message box
            // If the user clicked Yes, perform the delete operation
            if (result == MessageBoxResult.Yes)
            {
                DataAccess da = new();

                // Collect all inactive Contacts
                foreach (var item in inactiveListView.Items)
                {
                    Contact contact = item as Contact;
                    contact.IsActive = 1;
                    contacts.Add(contact);
                }
                // Set all inactive in DB to active
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
