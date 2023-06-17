using System;
using System.Windows;
using System.Windows.Controls;
using ContactsApp.Services;
using System.ComponentModel;
using Microsoft.Win32;
using System.Media;
using System.Windows.Media.Imaging;

namespace ContactsApp.Views
{
    /// <summary>
    /// Interaction logic for AddView.xaml
    /// </summary>
    public partial class AddView : UserControl
    {
        static string? Picture { get; set; }
        public AddView()
        {
            InitializeComponent();
            //EditPanel = Contact.CurrentContact;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ViewSetter.ClearView(View.Add);

            if (Contact.CurrentContact is not null)
            {
                ViewSetter.SetView(View.Contact);
            }
            else
            {
                ViewSetter.SetView(View.Home);
            }
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            //if (!DateOnly.TryParse(txtbxBirthday.Text, out DateOnly birthday))
            //{
            //    WarningException we = new("Birthday Error");
            //};
            Contact edit = new()
            {
                //Id = Contact.CurrentContact.Id,
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
                Picture = !string.IsNullOrWhiteSpace(Picture) ? Picture : null,
                IsActive = 1,
                IsFavorite = (checkbxFav.IsChecked==true) ? (byte)1 : (byte)0
            };
            

            DataAccess conn = new();
            conn.AddContact(edit);
            ViewSetter.SetView(View.Contact);
            ViewSetter.ClearView(View.Edit);
            //Contact.CurrentContact = edit;
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".jpg";
            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openFileDialog.FilterIndex = 1;

            //SHOW FILE DIALOG
            bool? result = openFileDialog.ShowDialog();

            //PROCESS DIALOG RESULTS / DETERMINE IF FILE WAS OPENED
            if (result == true)
            {
                //STORE FILE PATH
                string selectedFile = openFileDialog.FileName;
                Picture = selectedFile;
                imgContact.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
            
        }
    }
}
