using ContactsApp.Services;
using ContactsApp.Views;
using System;
using System.Collections.Generic;
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
        public System.ComponentModel.ICollectionView contactsCollectionView;
        public System.ComponentModel.ICollectionView favoritesCollectionView;
        public MainWindow()
        {
            InitializeComponent();
            //TestPage testPage = new();

            ViewSetter.ContentArea = MainWindowContentArea;
            ViewSetter.SetView(View.Home);

            DataAccess db = new();

            Contact.contacts = db.GetContacts();
            
            Contact.favorites = Contact.contacts.Where(x => x.IsFavorite == 1).ToList();

            favoritesListView.ItemsSource = Contact.favorites;
            var sortDescription = new SortDescription("FirstName", ListSortDirection.Ascending);
            favoritesCollectionView = CollectionViewSource.GetDefaultView(favoritesListView.ItemsSource);
            favoritesCollectionView.SortDescriptions.Add(sortDescription);
            favoritesCollectionView.Filter = i => ((Contact)i).IsFavorite == 1;


            contactsListView.ItemsSource = Contact.contacts;
            //var sortDescription = new SortDescription("FirstName", ListSortDirection.Ascending);
            contactsCollectionView = CollectionViewSource.GetDefaultView(contactsListView.ItemsSource);
            contactsCollectionView.SortDescriptions.Add(sortDescription);
            contactsCollectionView.Filter = i => ((Contact)i).IsActive != 0;
            contactsCollectionView.Filter = i => ((Contact)i).IsFavorite != 1;
            

            //this.DataContext = this;
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
            contactsListView.Items.Refresh();
            contactsCollectionView.Refresh();

            Contact.favorites = Contact.contacts.Where(x => x.IsFavorite == 1).ToList();

            favoritesListView.Items.Refresh();
            favoritesCollectionView.Refresh();
        }
    }
}
