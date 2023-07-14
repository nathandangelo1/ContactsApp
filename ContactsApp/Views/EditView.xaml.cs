using ContactsApp.Services;
using Microsoft.Win32;
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

namespace ContactsApp.Views
{
    /// <summary>
    /// Interaction logic for EditView.xaml
    /// </summary>
    public partial class EditView : UserControl
    {
        private string? Picture { get; set; }
        public EditView()
        {
            InitializeComponent();
            //EditPanel = Contact.CurrentContact;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //ViewSetter.ClearView(View.Edit);
            Reset();
            ViewSetter.SetView(View.Contact);
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            //if (!Date.TryParse(txtbxBirthday.Text, out DateOnly birthday))
            //{
            //    WarningException we = new("Birthday Error");
            //};
            //DateTime? selectedDate = datePickerBday.SelectedDate.Value.Date;
            //if(selectedDate is not null)
            //{
            DateTime? BdayDateTime = (datePickerBday.SelectedDate is not null) ? new DateTime(datePickerBday.SelectedDate.Value.Year, datePickerBday.SelectedDate.Value.Month, datePickerBday.SelectedDate.Value.Day) : null;

            //}

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
            ViewSetter.SetView(View.Contact);
            ViewSetter.ClearView(View.Edit);
            Contact.CurrentContact = edit;
            Refresh();
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
        private void Refresh()
        {
            //MainWindow window = (MainWindow)Application.Current.MainWindow;

            //window.RefreshListView();
            ViewSetter.PopulateContactView();
        }
    }
}
