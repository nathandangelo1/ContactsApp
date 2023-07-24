using ContactsApp.Services;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ContactsApp.Views
{
    public partial class EditView : UserControl
    {
        private string? Picture { get; set; }

        public EditView()
        {
            InitializeComponent();

            Contact cc = Contact.CurrentContact;
            if (cc.IsFavorite == 1) checkbxFav.IsChecked = true;

            // If currentContact's property is not null,
            // set the ContactView's equivalent control to value,
            // else set it to null
            txtbxFirst.Text = (cc.FirstName is null) ? "" : cc.FirstName.Trim()  ;
            txtbxMiddle.Text = (cc.MiddleName is null) ? "" : cc.MiddleName;
            txtbxNick.Text = (cc.NickName is null) ? "" : cc.NickName;
            txtbxLast.Text = (cc.LastName is null) ? "" : cc.LastName;
            txtbxTitle.Text = (cc.Title is null) ? "" : cc.Title;
            datePickerBday.SelectedDate = (cc.Birthday is null) ? null : cc.Birthday;
            txtbxEmail.Text = (cc.Email is null) ?  "" : cc.Email;
            txtbxPhone.Text = (cc.PhoneNumber is null) ? "" : cc.PhoneNumber;
            txtbxStreet.Text = (cc.Street is null) ? "" : cc.Street;
            txtbxCity.Text = (cc.City is null) ? "" : cc.City;
            txtbxState.Text = (cc.State is null) ? "" : cc.State;
            txtbxZip.Text = (cc.ZipCode is null) ? "" : cc.ZipCode;
            txtbxCountry.Text = (cc.Country is null) ? "" : cc.Country;
            txtbxWebsite.Text = (cc.Website is null) ? "" : cc.Website;
            txtbxNotes.Text = (cc.Notes is null) ? "" : cc.Notes;

            try
            {
                imgContact.Source = new BitmapImage(new Uri(cc.Picture, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                imgContact.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/noImage.png", UriKind.RelativeOrAbsolute));
            }

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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            ViewManager.SetView(View.Contact);
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            DateTime? BdayDateTime = 
                (datePickerBday.SelectedDate is null) ? 
                null :
                new DateTime(datePickerBday.SelectedDate.Value.Year,
                    datePickerBday.SelectedDate.Value.Month, 
                    datePickerBday.SelectedDate.Value.Day);

            if (Picture is null && Contact.CurrentContact is not null)
            {
                Picture = Contact.CurrentContact.Picture;
            }

            Contact edit = new()
            {
                Id = Contact.CurrentContact.Id,
                FirstName = string.IsNullOrWhiteSpace(txtbxFirst.Text) ? null : txtbxFirst.Text,
                MiddleName = string.IsNullOrWhiteSpace(txtbxMiddle.Text) ? null : txtbxMiddle.Text,
                NickName = string.IsNullOrWhiteSpace(txtbxNick.Text) ? null : txtbxNick.Text,
                LastName = string.IsNullOrWhiteSpace(txtbxLast.Text) ? null : txtbxLast.Text,
                Title = string.IsNullOrWhiteSpace(txtbxTitle.Text) ? null : txtbxTitle.Text,
                Birthday = (datePickerBday.SelectedDate != null) ? BdayDateTime : null,
                Email = string.IsNullOrWhiteSpace(txtbxEmail.Text) ? null : txtbxEmail.Text,
                PhoneNumber = string.IsNullOrWhiteSpace(txtbxPhone.Text) ? null : txtbxPhone.Text,
                Street = string.IsNullOrWhiteSpace(txtbxNick.Text) ? null : txtbxNick.Text,
                City = string.IsNullOrWhiteSpace(txtbxCity.Text) ? null : txtbxCity.Text,
                State = string.IsNullOrWhiteSpace(txtbxState.Text) ? null : txtbxState.Text,
                ZipCode = string.IsNullOrWhiteSpace(txtbxZip.Text) ? null : txtbxZip.Text,
                Country = string.IsNullOrWhiteSpace(txtbxCountry.Text) ? null : txtbxCountry.Text,
                Website = string.IsNullOrWhiteSpace(txtbxWebsite.Text) ? null : txtbxWebsite.Text,
                Notes = string.IsNullOrWhiteSpace(txtbxNotes.Text) ? null : txtbxNotes.Text,
                Picture = string.IsNullOrWhiteSpace(Picture) ? null : Picture,
                IsActive = 1,
                IsFavorite = (checkbxFav.IsChecked == true) ? (byte)1 : (byte)0
            };
            // Update Database
            DataAccess conn = new();
            conn.UpdateContact(edit);
            
            // Update CurrentContact
            Contact.CurrentContact = edit;

            // Return to ContactView
            ViewManager.SetView(View.Contact);
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            // Create an open file dialog
            OpenFileDialog ofd = new OpenFileDialog();
            // Set the filter to show only image files
            ofd.Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp";
            // Show the dialog and check if the user selected a file
            if (ofd.ShowDialog() == true)
            {
                // Create a bitmap image from the file path
                BitmapImage bi = new BitmapImage(new Uri(ofd.FileName, UriKind.RelativeOrAbsolute));
                // Set the image source to the bitmap image
                imgContact.Source = bi;
                Picture = ofd.FileName;
                imgContact.Width = 200;
                imgContact.Height = 200;
            }
        }
        public void Reset()
        {
            foreach (WrapPanel wp in editPanel.Children)
            {
                foreach (Control con in wp.Children)
                {
                    if (con.GetType() == typeof(TextBox))
                    {
                        TextBox tx = (con as TextBox);
                        tx.Clear();
                    }
                }
            }
            checkbxFav.IsChecked = false;
        }
    }
}
