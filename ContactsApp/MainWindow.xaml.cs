using ContactsApp.Services;
using ContactsApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;


namespace ContactsApp
{
    public enum View
    {
        Contact,
        Edit,
        Settings,
        Delete,
        Add,
        Home
    }
    public partial class MainWindow : Window
    {
        public static ContactsList CL { get; set; }
        private CollectionViewSource contactsViewSource;
        private CollectionViewSource favoritesViewSource;

        public MainWindow()
        {
            InitializeComponent();

            // Get main content area 
            ViewSetter.ContentArea = MainWindowContentArea;

            // Set the current view
            ViewSetter.SetView(View.Home);

            // Instatiate dataAccess class
            DataAccess db = new();

            // Populate list of contacts based on query results
            CL = new();
            IEnumerable<Contact> results = db.GetContacts();
            CL.Contacts = new ObservableCollection<Contact>(results);

            //var contacts = new ObservableCollection<Contact>();
            contactsViewSource = new CollectionViewSource() { Source = CL.Contacts };
            contactsViewSource.Filter += (s, e) =>
            {
                if (e.Item is Contact contact)
                {
                    if (string.IsNullOrEmpty(txtSearch.Text) && contact.IsActive == 1 && contact.IsFavorite == 0)
                        e.Accepted = true;
                    else
                        e.Accepted = contact.FullName.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase)
                        && contact.IsActive == 1 && contact.IsFavorite == 0;
                }
            };

            contactsListView.ItemsSource = contactsViewSource.View;

            favoritesViewSource = new CollectionViewSource() { Source = CL.Contacts };
            favoritesViewSource.Filter += (s, e) =>
            {
                if (e.Item is Contact contact)
                {
                    if (string.IsNullOrEmpty(txtSearch.Text) && contact.IsActive == 1 && contact.IsFavorite == 1)
                        e.Accepted = true;
                    else
                        e.Accepted = contact.FullName.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase)
                        && contact.IsActive == 1 && contact.IsFavorite == 1;
                }
            };
            favoritesListView.ItemsSource = favoritesViewSource.View;

            Settings.OnSettingsChange += Settings_OnSettingsChange;
            ContactView.OnListChange += RefreshContactsList;
            DeleteView.OnListChange += RefreshContactsList;

            // Event is fired below to sort and group contactsListView without a seperate function(reduce code duplication)
            Settings_OnSettingsChange(new object(), new EventArgs());
        }
        private void RefreshContactsList(object? sender, EventArgs e)
        {
            contactsViewSource.View.Refresh();
        }

        private void Settings_OnSettingsChange(object? sender, EventArgs e)
        {
            if (Settings.SortByFirstName)
            {
                contactsViewSource.SortDescriptions.Clear();
                contactsViewSource.GroupDescriptions.Clear();
                contactsViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
                contactsViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
                contactsViewSource.GroupDescriptions.Add(new PropertyGroupDescription("GroupingName", new FirstLetterConverter()));
                favoritesViewSource.SortDescriptions.Clear();
                favoritesViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
                favoritesViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
            }
            else
            {
                contactsViewSource.SortDescriptions.Clear();
                contactsViewSource.GroupDescriptions.Clear();
                contactsViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
                contactsViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
                contactsViewSource.GroupDescriptions.Add(new PropertyGroupDescription("GroupingName", new FirstLetterConverter()));
                favoritesViewSource.SortDescriptions.Clear();
                favoritesViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
                favoritesViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
            }
            contactsViewSource.View.Refresh();
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            contactsViewSource.View.Refresh();
        }

        private void listView_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                var contact = item as Contact;
                Contact.CurrentContact = contact;
                ViewSetter.PopulateContactView();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewSetter.SetView(View.Add);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            ViewSetter.SetView(View.Settings);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}

