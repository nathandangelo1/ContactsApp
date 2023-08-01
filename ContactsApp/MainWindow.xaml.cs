using ContactsApp.Services;
using ContactsApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using static ContactsApp.ViewManager;
using static ContactsApp.Settings;
using System.Linq;
using System.IO;

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
            ContentArea = MainWindowContentArea;

            // Instatiate dataAccess class
            DataAccess db = new();

            // Populate list of contacts based on query results
            CL = new();
            IEnumerable<Contact> results = db.GetContacts();
            CL.Contacts = new ObservableCollection<Contact>(results);

            // Create CollectionViewSource to model the collection view of main Contacts List
            contactsViewSource = new CollectionViewSource() { Source = CL.Contacts };
            contactsViewSource.Filter += (s, e) =>
            {
                if (e.Item is Contact contact)
                {
                    if (string.IsNullOrEmpty(txtSearch.Text) && contact.IsActive == 1)
                        e.Accepted = true;
                    else
                        e.Accepted = contact.FullName.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase)
                        && contact.IsActive == 1;
                }
            };
            contactsViewSource.GroupDescriptions.Add(new PropertyGroupDescription("GroupingName", new FirstLetterConverter()));
            //Set listview to corresponding view
            contactsListView.ItemsSource = contactsViewSource.View;

            // Create ViewSource to model the collection view of Favorites List
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
            //Set listview to corresponding view
            favoritesListView.ItemsSource = favoritesViewSource.View;

            // Subscribe to important events
            OnSettingsChange += Settings_OnSettingsChange;
            Views.ContactView.OnListChange += RefreshContactsList;
            Views.DeleteView.OnListChange += RefreshContactsList;

            // Event is fired below to sort and group contactsListView without a seperate function
            Settings_OnSettingsChange(new object(), new EventArgs());

            //Get random contact for initial display
            Contact.CurrentContact = GetRandomContact();
            
            // Set and populate the current view
            SetView(View.Contact);
            ViewManager.ContactView.PopulateView();
        }
        public static Contact GetRandomContact()
        {
            Random rnd = new Random();
            var list = CL.Contacts.Where(x => x.IsActive == 1);
            int randIndex = rnd.Next(list.Count());
            return list.ElementAt(randIndex);
        }
        private void RefreshContactsList(object? sender, EventArgs e)
        {
            contactsViewSource.View.Refresh();
        }

        private void Settings_OnSettingsChange(object? sender, EventArgs e)
        {
            contactsViewSource.SortDescriptions.Clear();
            favoritesViewSource.SortDescriptions.Clear();
            contactsViewSource.GroupDescriptions.Clear();
            
            contactsViewSource.GroupDescriptions.Add(new PropertyGroupDescription("GroupingName", new FirstLetterConverter()));

            contactsViewSource.SortDescriptions.Add(new SortDescription(((bool)SortByFirstName ? "GroupingName" : "LastName"  ), ListSortDirection.Ascending));
            //contactsViewSource.SortDescriptions.Add(new SortDescription(((bool)SortByFirstName ? "LastName"  : "GroupingName" ), ListSortDirection.Ascending));
            favoritesViewSource.SortDescriptions.Add(new SortDescription(((bool)SortByFirstName ? "GroupingName" : "LastName" ), ListSortDirection.Ascending));
            //favoritesViewSource.SortDescriptions.Add(new SortDescription(((bool)SortByFirstName ? "LastName"  : "GroupingName"), ListSortDirection.Ascending));

            contactsViewSource.View.Refresh();
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            contactsViewSource.View.Refresh();
            favoritesViewSource.View.Refresh();
        }

        private void listView_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                //var contact = item as Contact;
                Contact.CurrentContact = item as Contact;
                ViewManager.ContactView.PopulateView();
                SetView(View.Contact);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            SetView(View.Add);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SetView(View.Settings);
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

