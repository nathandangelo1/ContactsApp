using ContactsApp.Services;
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
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
        //private CollectionViewSource _collectionViewSource;
        //public CollectionViewSource CollectionViewSource
        //{
        //    get { return _collectionViewSource; }
        //    set
        //    {
        //        if (_collectionViewSource != value)
        //        {
        //            _collectionViewSource = value;
        //            OnPropertyChanged(nameof(CollectionViewSource));
        //        }
        //    }
        //}

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
                    if (String.IsNullOrEmpty(txtSearch.Text) && contact.IsActive == 1 && contact.IsFavorite == 0)
                        e.Accepted = true;
                    else
                        e.Accepted = contact.FullName.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase)
                        && contact.IsActive == 1 && contact.IsFavorite == 0;
                }
            };

            //if (!Settings.SortByFirstName) // If NOT sort by First Name, then sort by lastname
            //{
            //    contactsViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
            //    contactsViewSource.SortDescriptions.Add(new SortDescription("NickName", ListSortDirection.Ascending));
            //    contactsViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
            //}
            //else // else, sort by firstname
            //{
            //    contactsViewSource.SortDescriptions.Add(new SortDescription("NickName", ListSortDirection.Ascending));
            //    contactsViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
            //    contactsViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
            //}
            contactsListView.ItemsSource = contactsViewSource.View;


            favoritesViewSource = new CollectionViewSource() { Source = CL.Contacts };
            favoritesViewSource.Filter += (s, e) =>
            {
                if (e.Item is Contact contact)
                {
                    if (String.IsNullOrEmpty(txtSearch.Text) && contact.IsActive == 1 && contact.IsFavorite == 1)
                        e.Accepted = true;
                    else
                        e.Accepted = contact.FullName.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase)
                        && contact.IsActive == 1 && contact.IsFavorite == 1;
                }
            };
            //favoritesViewSource.SortDescriptions.Add(new SortDescription("NickName", ListSortDirection.Ascending));
            //favoritesViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
            //favoritesViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
            favoritesListView.ItemsSource = favoritesViewSource.View;
            Sort();
        }

        public void Sort()
        {
            if (Settings.SortByFirstName)
            {
                contactsViewSource.SortDescriptions.Clear();
                contactsViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
                contactsViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
                favoritesViewSource.SortDescriptions.Clear();
                favoritesViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
                favoritesViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
            }
            else
            {
                contactsViewSource.SortDescriptions.Clear();
                contactsViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
                contactsViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
                favoritesViewSource.SortDescriptions.Clear();
                favoritesViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
                favoritesViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
            }
            contactsViewSource.View.Refresh();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            contactsViewSource.View.Refresh();
            //CollectionViewSource.GetDefaultView(favoritesListView.ItemsSource).Refresh();
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
        //public void RefreshListView()
        //{
        //    //var currentFavorites = new ObservableCollection<Contact>(FL.Favorites.Where(x => x.IsFavorite == 1);
        //    //FL.Favorites = new ObservableCollection<Contact>( CL.Contacts.Where(x => x.IsFavorite == 1));
        //    //CL.Contacts = new ObservableCollection<Contact>( CL.Contacts.Where(x => x.IsFavorite != 1));
        //    // contactsListView.ItemsSource = CL.Contacts;
        //    // favoritesListView.ItemsSource = FL.Favorites;

        //    //contactsListView.Items.Refresh();
        //    // contactsCollectionView.Refresh();

        //    //favoritesListView.Items.Refresh();
        //    favoritesListView.ItemsSource = CL.Contacts.Where(x => x.Id == 1);
        //}



            private void MinimizeButton_Click(object sender, RoutedEventArgs e)
            {
                this.WindowState = WindowState.Minimized;
            }

            private void MaximizeButton_Click(object sender, RoutedEventArgs e)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                    //MaximizeButton.Content = "[ ]";
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    //MaximizeButton.Content = "[ ]";
                }
            }

            private void CloseButton_Click(object sender, RoutedEventArgs e)
            {
                this.Close();
            }
        
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        //public static void UpdateSettings()
        //{
        //    if (Settings.SortByFirstName) // If NOT sort by First Name, then sort by lastname
        //    {
        //        contactsViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
        //        contactsViewSource.SortDescriptions.Add(new SortDescription("NickName", ListSortDirection.Ascending));
        //        contactsViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
        //    }
        //    else // else, sort by firstname
        //    {
        //        contactsViewSource.SortDescriptions.Add(new SortDescription("NickName", ListSortDirection.Ascending));
        //        contactsViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
        //        contactsViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
        //    }
        //    contactsViewSource.View.Refresh();
        //}
    }

}

