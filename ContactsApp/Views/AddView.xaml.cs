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
using Dapper;
using Microsoft.Data.SqlClient;
using System.Reflection;
using ContactsApp.Services;

namespace ContactsApp.Views
{
    /// <summary>
    /// Interaction logic for AddView.xaml
    /// </summary>
    public partial class AddView : UserControl
    {
        public AddView()
        {
            InitializeComponent();
            //EditPanel = Contact.CurrentContact;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ViewSetter.ClearView(View.edit);
            ViewSetter.SetView(View.contact);
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            //if(!DateOnly.TryParse(txtbxBirthday.Text, out DateOnly birthday)){
            //    WarningException we = new ("Date Error");
            //};
            Contact edit = new()
            {
                Id = Contact.CurrentContact.Id,
                FirstName = !string.IsNullOrWhiteSpace(txtbxFirst.Text) ? txtbxFirst.Text : null,
                MiddleName = !string.IsNullOrWhiteSpace(txtbxMiddle.Text) ? txtbxMiddle.Text : null,
                NickName = !string.IsNullOrWhiteSpace(txtbxNick.Text) ? txtbxNick.Text : null,
                LastName = !string.IsNullOrWhiteSpace(txtbxLast.Text) ? txtbxLast.Text : null,
                Title = !string.IsNullOrWhiteSpace(txtbxTitle.Text) ? txtbxTitle.Text : null,
                Birthday = !string.IsNullOrWhiteSpace(txtbxBirthday.Text) ? txtbxBirthday.Text : null,
                Email = !string.IsNullOrWhiteSpace(txtbxEmail.Text) ? txtbxEmail.Text : null,
                PhoneNumber = !string.IsNullOrWhiteSpace(txtbxPhone.Text) ? txtbxPhone.Text : null,
                Street = !string.IsNullOrWhiteSpace(txtbxNick.Text) ? txtbxStreet.Text : null,
                City = !string.IsNullOrWhiteSpace(txtbxCity.Text) ? txtbxCity.Text : null,
                State = !string.IsNullOrWhiteSpace(txtbxState.Text) ? txtbxState.Text : null,
                ZipCode = !string.IsNullOrWhiteSpace(txtbxZip.Text) ? txtbxZip.Text : null,
                Country = !string.IsNullOrWhiteSpace(txtbxCountry.Text) ? txtbxCountry.Text : null,
                Website = !string.IsNullOrWhiteSpace(txtbxWebsite.Text) ? txtbxWebsite.Text : null,
                Notes = !string.IsNullOrWhiteSpace(txtbxNotes.Text) ? txtbxNotes.Text : null,
                IsActive = 1,
                IsFavorite = ((bool)checkbxFav.IsChecked) ? 1 : 0
            };


            DataAccess conn = new();
            conn.UpdateContact(edit);
            ViewSetter.SetView(View.contact);
            ViewSetter.ClearView(View.edit);
            //Contact.CurrentContact = edit;
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
