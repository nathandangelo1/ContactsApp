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
        //  U+1F382
        //contacts = new();
        //public static Contact CurrentContact { get; set; }
        //  default view of the favoritesListView.ItemsSource using the CollectionViewSource.GetDefaultView method
        //  and assigns it to the favoritesCollectionView variable. A view is a layer on top of a collection that
        //  allows you to sort, filter, group, or navigate the items in the collection. A collection can have multiple
        //  views associated with it, but only one default view.
        //public ICollectionView contactsCollectionView;
        //public ICollectionView favoritesCollectionView;
        public static ContactsList CL { get; set; }
        public CollectionViewSource contactsViewSource;
        public CollectionViewSource favoritesViewSource;
        public static class Icons
        {
            public const string IconName = "\u25A1";
        }
        public MainWindow()
        {
            InitializeComponent();

            ViewSetter.ContentArea = MainWindowContentArea;
            ViewSetter.SetView(View.Home);

            DataAccess db = new();

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
            contactsViewSource.SortDescriptions.Add(new SortDescription("NickName", ListSortDirection.Ascending));
            contactsViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
            contactsViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
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
            favoritesViewSource.SortDescriptions.Add(new SortDescription("NickName", ListSortDirection.Ascending));
            favoritesViewSource.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
            favoritesViewSource.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
            favoritesListView.ItemsSource = favoritesViewSource.View;
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
    }

}

