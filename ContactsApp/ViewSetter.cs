using ContactsApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        private static DeleteView deleteView = new();
        public static DeleteView DeleteView
        {
            get
            {
                return deleteView;
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
                    //AddView = new(); break;
                case View.Delete:
                    //DeleteView = new(); break;
                case View.Settings:
                    //SettingsView = new(); break;
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
        public static void PopulateContactView()
        {
            Contact cc = Contact.CurrentContact;
            SetView(View.Contact);
           // if (contact.IsFavorite == 1) { contactView.checkbxFav.IsChecked = true; }

            ContactView.txtfullName.Text = (cc.FullName is not null) ? cc.FullName.Trim() : "";
            ContactView.txtStreet.Text = (cc.Street is not null) ? cc.Street : "";
            ContactView.txtCity.Text = (cc.City is not null) ? cc.City : "";
            ContactView.txtState.Text = (cc.State is not null) ? cc.State : "";
            ContactView.txtZip.Text = (cc.ZipCode is not null) ? cc.ZipCode : "";
            ContactView.txtEmail.Text = (cc.Email is not null) ? cc.Email : "";
            ContactView.txtPhone.Text = (cc.PhoneNumber is not null) ? cc.PhoneNumber : "";
            ContactView.txtWebsite.Text = (cc.Website is not null) ? cc.Website : "";
            ContactView.txtNotes.Text = (cc.Notes is not null) ? cc.Notes : "";

        }
        public static void PopulateEditView()
        {
            Contact cc = Contact.CurrentContact;
            if (cc.IsFavorite == 1)  EditView.checkbxFav.IsChecked = true;

            EditView.txtbxFirst.Text = (cc.FirstName is not null) ? cc.FirstName.Trim() : "";
            EditView.txtbxMiddle.Text = (cc.MiddleName is not null) ? cc.MiddleName : "";
            EditView.txtbxNick.Text = (cc.NickName is not null) ? cc.NickName : "";
            EditView.txtbxLast.Text = (cc.LastName is not null) ? cc.LastName : "";
            EditView.txtbxTitle.Text = (cc.Title is not null) ? cc.Title : "";
            EditView.txtbxBirthday.Text = (cc.Birthday is not null) ? cc.Birthday.ToString() : "";
            EditView.txtbxEmail.Text = (cc.Email is not null) ? cc.Email : "";
            EditView.txtbxPhone.Text = (cc.PhoneNumber is not null) ? cc.PhoneNumber : "";
            EditView.txtbxStreet.Text = (cc.Street is not null) ? cc.Street : "";
            EditView.txtbxCity.Text = (cc.City is not null) ? cc.City : "";
            EditView.txtbxState.Text = (cc.State is not null) ? cc.State : "";
            EditView.txtbxZip.Text= (cc.ZipCode is not null) ? cc.ZipCode : "";
            EditView.txtbxCountry.Text= (cc.Country is not null) ? cc.Country : "";

            EditView.txtbxWebsite.Text = (cc.Website is not null) ? cc.Website : "";
            EditView.txtbxNotes.Text = (cc.Notes is not null) ? cc.Notes : "";
        }
    }
}
