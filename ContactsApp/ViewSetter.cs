using ContactsApp.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ContactsApp
{
    public sealed class ViewSetter
    {
        private static readonly ContactView contactView = new();
        public static ContactView ContactView
        {
            get
            {
                return contactView;
            }
        }
        private static readonly EditView editView = new();
        public static EditView EditView
        {
            get
            {
                return editView;
            }
        }
        private static DeleteView deleteView;
        public static DeleteView DeleteView
        {
            get
            {
                if (deleteView is null)
                {
                    return new DeleteView();
                }
                return deleteView;
            }
            set
            {
                deleteView = value;
            }
        }
        private static SettingsView settingsView = new();
        public static SettingsView SettingsView
        {
            get
            {
                return settingsView;
            }
        }
        private static AddView addView = new();
        public static AddView AddView
        {
            get
            {
                return addView;
            }
            set
            {
                addView = value;
            }
        }
        private static HomeView homeView = new();
        public static HomeView HomeView
        {
            get
            {
                return homeView;
            }
        }
        public static ContentControl ContentArea { get; set; }

        public static void ClearView(View view)
        {
            switch (view)
            {
                case View.Contact:
                    Contact.CurrentContact = null;
                    //ContactView = new(); break;
                    break;
                case View.Edit:
                    EditView.Reset();
                    break;
                case View.Add:
                    AddView = new();
                    break;
                case View.Home:
                    //HomeView = new(); break;
                    break;
            }
        }
        public static void SetView(View view)
        {
            switch (view)
            {
                case View.Contact:
                    ContentArea.Content = ContactView; break;

                case View.Edit:
                    PopulateEditView();
                    ContentArea.Content = EditView; break;

                case View.Add:
                    ContentArea.Content = AddView; break;
                case View.Delete:
                    ContentArea.Content = DeleteView; break;
                case View.Settings:
                    ContentArea.Content = SettingsView; break;
                case View.Home:
                    ContentArea.Content = HomeView; break;
                default:
                    ContentArea.Content = ContactView; break;
            }
        }
        public static void PopulateDeleteView()
        {
            Contact cc = DeleteView.Selected;

            if (cc is not null)
            {
                // If currentContact's property is not null, set the ContactView's equivalent control to value, else set it to null
                DeleteView.txtfullName.Text = (cc.FullName is not null) ? cc.FullName.Trim() : "";
                DeleteView.txtPhone.Text = (cc.PhoneNumber is not null) ? cc.PhoneNumber : "";
                DeleteView.txtStreet.Text = (cc.Street is not null) ? cc.Street : "";
                DeleteView.txtCity.Text = (cc.City is not null) ? cc.City : "";
                DeleteView.txtState.Text = (cc.State is not null) ? cc.State : "";
                DeleteView.txtZip.Text = (cc.ZipCode is not null) ? cc.ZipCode : "";
                try
                {
                    DeleteView.imgContact.Source = new BitmapImage(new Uri(cc.Picture, UriKind.Absolute));
                    //DeleteView.imgContact.Source = (cc.Picture is not null) ? new BitmapImage(new Uri(cc.Picture)) : null;
                }
                catch
                {
                    MessageBoxResult result = MessageBox.Show("Photo error");

                    DeleteView.imgContact.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/noImage.png", UriKind.RelativeOrAbsolute));
                }
                // if stock image, make smaller
                if (DeleteView.imgContact.Source.ToString().Contains("noImage.png"))
                {
                    DeleteView.imgContact.Height = 75;
                    DeleteView.imgContact.Width = 75;
                }
            }
        }
        public static void PopulateContactView()
        {
            Contact cc = Contact.CurrentContact;
            SetView(View.Contact);

            // If currentContact's property is not null, set the ContactView's equivalent control to value, else set it to null
            ContactView.txtfullName.Text = (cc.FullName is not null) ? cc.FullName.Trim() : "";
            ContactView.txtStreet.Text = (cc.Street is not null) ? cc.Street : "";
            ContactView.txtCity.Text = (cc.City is not null) ? cc.City : "";
            ContactView.txtState.Text = (cc.State is not null) ? cc.State : "";
            ContactView.txtZip.Text = (cc.ZipCode is not null) ? cc.ZipCode : "";
            ContactView.txtEmail.Text = (cc.Email is not null) ? cc.Email : "";
            ContactView.txtPhone.Text = (cc.PhoneNumber is not null) ? cc.PhoneNumber : "";
            ContactView.txtWebsite.Text = (cc.Website is not null) ? cc.Website : "";
            ContactView.txtNotes.Text = (cc.Notes is not null) ? cc.Notes : "";
            try
            {
                ContactView.imgContact.Source = new BitmapImage(new Uri(cc.Picture, UriKind.Absolute));
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Photo error");

                ContactView.imgContact.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/noImage.png", UriKind.RelativeOrAbsolute));
            }
            if (ContactView.imgContact.Source.ToString().Contains("noImage.png"))
            {
                ContactView.imgContact.Height = 75;
                ContactView.imgContact.Width = 75;
            }
            else
            {
                ContactView.imgContact.Height = 200;
                ContactView.imgContact.Width = 200;
            }

            if (WithinRange(cc.Birthday))
            {
                ContactView.txtfullName.Text = $"\U0001F382 " + ContactView.txtfullName.Text;
            }
        }
        private static bool WithinRange(DateTime? bday)
        {
            if (bday is null) return false;

            DateTime birthday = (DateTime)bday;

            // Get the current date
            DateTime currentDate = DateTime.Today;

            // Define the range boundaries
            int rangeInDays = Settings.BirthdayRange;
            DateTime rangeStart = currentDate.AddDays(-rangeInDays);
            DateTime rangeEnd = currentDate.AddDays(rangeInDays);

            // Check if the birthday's day of year is within the range
            return birthday.DayOfYear >= rangeStart.DayOfYear && birthday.DayOfYear <= rangeEnd.DayOfYear;
        }
        public static void PopulateEditView()
        {
            Contact cc = Contact.CurrentContact;
            if (cc.IsFavorite == 1) EditView.checkbxFav.IsChecked = true;

            // If currentContact's property is not null,
            // set the ContactView's equivalent control to value,
            // else set it to null
            EditView.txtbxFirst.Text = (cc.FirstName is not null) ? cc.FirstName.Trim() : "";
            EditView.txtbxMiddle.Text = (cc.MiddleName is not null) ? cc.MiddleName : "";
            EditView.txtbxNick.Text = (cc.NickName is not null) ? cc.NickName : "";
            EditView.txtbxLast.Text = (cc.LastName is not null) ? cc.LastName : "";
            EditView.txtbxTitle.Text = (cc.Title is not null) ? cc.Title : "";
            EditView.datePickerBday.SelectedDate = (cc.Birthday is not null) ? cc.Birthday : null;
            EditView.txtbxEmail.Text = (cc.Email is not null) ? cc.Email : "";
            EditView.txtbxPhone.Text = (cc.PhoneNumber is not null) ? cc.PhoneNumber : "";
            EditView.txtbxStreet.Text = (cc.Street is not null) ? cc.Street : "";
            EditView.txtbxCity.Text = (cc.City is not null) ? cc.City : "";
            EditView.txtbxState.Text = (cc.State is not null) ? cc.State : "";
            EditView.txtbxZip.Text = (cc.ZipCode is not null) ? cc.ZipCode : "";
            EditView.txtbxCountry.Text = (cc.Country is not null) ? cc.Country : "";
            EditView.txtbxWebsite.Text = (cc.Website is not null) ? cc.Website : "";
            EditView.txtbxNotes.Text = (cc.Notes is not null) ? cc.Notes : "";

            //EditView.imgContact.Source = (cc.Picture is not null) ? new BitmapImage(new Uri("Resources\\noImage.png", UriKind.RelativeOrAbsolute)) : null;
            try
            {
                EditView.imgContact.Source = new BitmapImage(new Uri(cc.Picture, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                //MessageBoxResult result = MessageBox.Show("Photo error");

                EditView.imgContact.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/noImage.png", UriKind.RelativeOrAbsolute));
            }
            if (EditView.imgContact.Source.ToString().Contains("noImage.png"))
            {
                EditView.imgContact.Height = 75;
                EditView.imgContact.Width = 75;
            }
        }

    }
}
