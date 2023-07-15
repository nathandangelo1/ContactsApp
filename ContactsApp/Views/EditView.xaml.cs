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
        }
        public void PopulateView()
        {
            Contact cc = Contact.CurrentContact;
            if (cc.IsFavorite == 1) checkbxFav.IsChecked = true;

            // If currentContact's property is not null,
            // set the ContactView's equivalent control to value,
            // else set it to null
            txtbxFirst.Text = (cc.FirstName is not null) ? cc.FirstName.Trim() : "";
            txtbxMiddle.Text = (cc.MiddleName is not null) ? cc.MiddleName : "";
            txtbxNick.Text = (cc.NickName is not null) ? cc.NickName : "";
            txtbxLast.Text = (cc.LastName is not null) ? cc.LastName : "";
            txtbxTitle.Text = (cc.Title is not null) ? cc.Title : "";
            datePickerBday.SelectedDate = (cc.Birthday is not null) ? cc.Birthday : null;
            txtbxEmail.Text = (cc.Email is not null) ? cc.Email : "";
            txtbxPhone.Text = (cc.PhoneNumber is not null) ? cc.PhoneNumber : "";
            txtbxStreet.Text = (cc.Street is not null) ? cc.Street : "";
            txtbxCity.Text = (cc.City is not null) ? cc.City : "";
            txtbxState.Text = (cc.State is not null) ? cc.State : "";
            txtbxZip.Text = (cc.ZipCode is not null) ? cc.ZipCode : "";
            txtbxCountry.Text = (cc.Country is not null) ? cc.Country : "";
            txtbxWebsite.Text = (cc.Website is not null) ? cc.Website : "";
            txtbxNotes.Text = (cc.Notes is not null) ? cc.Notes : "";

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
            ViewSetter.SetView(View.Contact);
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            DateTime? BdayDateTime = (datePickerBday.SelectedDate is not null) ? new DateTime(datePickerBday.SelectedDate.Value.Year, datePickerBday.SelectedDate.Value.Month, datePickerBday.SelectedDate.Value.Day) : null;

            Contact edit = new()
            {
                Id = Contact.CurrentContact.Id,
                FirstName = !string.IsNullOrWhiteSpace(txtbxFirst.Text) ? txtbxFirst.Text : null,
                MiddleName = !string.IsNullOrWhiteSpace(txtbxMiddle.Text) ? txtbxMiddle.Text : null,
                NickName = !string.IsNullOrWhiteSpace(txtbxNick.Text) ? txtbxNick.Text : null,
                LastName = !string.IsNullOrWhiteSpace(txtbxLast.Text) ? txtbxLast.Text : null,
                Title = !string.IsNullOrWhiteSpace(txtbxTitle.Text) ? txtbxTitle.Text : null,
                
                Birthday = (datePickerBday.SelectedDate != null) ? BdayDateTime : null,

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
                IsFavorite = (checkbxFav.IsChecked == true) ? (byte)1 : (byte)0
            };
            DataAccess conn = new();
            conn.UpdateContact(edit);

            // Return to ContactView
            ViewSetter.SetView(View.Contact);
            ViewSetter.ClearView(View.Edit);
            Contact.CurrentContact = edit;
            ViewSetter.PopulateContactView();
            //Refresh();
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
            if ((bool)checkbxFav.IsChecked) checkbxFav.IsChecked = false;
        }
        //private void Refresh()
        //{
        //    ViewSetter.PopulateContactView();
        //}
    }
}
