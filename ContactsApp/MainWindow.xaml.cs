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

    //contacts = new();
    //public static Contact CurrentContact { get; set; }
    //  default view of the favoritesListView.ItemsSource using the CollectionViewSource.GetDefaultView method
    //  and assigns it to the favoritesCollectionView variable. A view is a layer on top of a collection that
    //  allows you to sort, filter, group, or navigate the items in the collection. A collection can have multiple
    //  views associated with it, but only one default view.
        public ICollectionView contactsCollectionView;
        public ListCollectionView favoritesCollectionView;
        public static CL CL { get; set; }
        public bool HideUnhide { get; set; }
        //public static FL FL { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            ViewSetter.ContentArea = MainWindowContentArea;
            ViewSetter.SetView(View.Home);

            DataAccess db = new();

            CL = new();
            IEnumerable<Contact> results = db.GetContacts();
            CL.Contacts = new ObservableCollection<Contact>(results);

            contactsListView.ItemsSource = CL.Contacts ;
            var sortDescription1 = new SortDescription("NickName", ListSortDirection.Ascending);
            var sortDescription2 = new SortDescription("FirstName", ListSortDirection.Ascending);
            var sortDescription3 = new SortDescription("LastName", ListSortDirection.Ascending);
            contactsCollectionView = CollectionViewSource.GetDefaultView(contactsListView.ItemsSource);
            contactsCollectionView.SortDescriptions.Add(sortDescription1);
            contactsCollectionView.SortDescriptions.Add(sortDescription2);
            contactsCollectionView.SortDescriptions.Add(sortDescription3);
            contactsCollectionView.Filter = i => ((Contact)i).IsActive != 0;
            
            contactsCollectionView.Filter = UserFilter;


            favoritesCollectionView = new ListCollectionView(CL.Contacts);
            favoritesCollectionView.Filter = item => (item as Contact).IsFavorite == 1;
            favoritesListView.ItemsSource = favoritesCollectionView;
            
            //favoritesCollectionView.Filter = UserFilter;
            
            //this.DataContext = this;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
                return true;
            else
                return ((item as Contact).FullName.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase));
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(contactsListView.ItemsSource).Refresh();
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
        public void RefreshListView()
        {
            //var currentFavorites = new ObservableCollection<Contact>(FL.Favorites.Where(x => x.IsFavorite == 1);
            //FL.Favorites = new ObservableCollection<Contact>( CL.Contacts.Where(x => x.IsFavorite == 1));
            //CL.Contacts = new ObservableCollection<Contact>( CL.Contacts.Where(x => x.IsFavorite != 1));
            // contactsListView.ItemsSource = CL.Contacts;
            // favoritesListView.ItemsSource = FL.Favorites;

            //contactsListView.Items.Refresh();
            // contactsCollectionView.Refresh();

            //favoritesListView.Items.Refresh();
            favoritesListView.ItemsSource = CL.Contacts.Where(x => x.Id == 1);
        }
    }
}
