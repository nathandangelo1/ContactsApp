using ContactsApp.Services;
using ContactsApp.Views;
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



namespace ContactsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public enum View
    {
        contact,
        edit,
        settings,
        delete
    }
    public partial class MainWindow : Window
    {
        List<Contact> contacts = new();
        //public static Contact CurrentContact { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //TestPage testPage = new();

            ViewSetter.ContentArea = MainWindowContentArea;
            ViewSetter.SetView(View.contact);

            DataAccess db = new();

            contacts = db.GetContacts();

            contactsListView.ItemsSource = contacts;
            //contactsListView.DisplayMemberPath = "FullName";
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
    }
}
