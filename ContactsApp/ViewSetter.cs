using ContactsApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ContactsApp
{
    public static class ViewSetter
    {
        static ContactView contactView = new();
        static EditView editView = new();
        // public static DeleteView deleteView = new();
        // public static SettingsView settingsView = new();

        public static ContentControl ContentArea {get;set;}
        //public static UserControl CurrentView 
        //{
        //    get
        //    {
        //        return CurrentView;
        //    }
        //    private set
        //    {
        //        CurrentView = value;
        //    }
        //}
        public static void ClearView(View view)
        {
            switch (view)
            {
                case View.contact:
                    Contact.CurrentContact = null;
                    contactView = new(); break;
                case View.edit:
                    editView = new(); break;
                //case Views.delete:
                //deleteView=new(); break;
                // case UserControls.settings:
                // settingsView=new(); break;

            }
        }
        public static void SetView(View view)
        {
            switch (view)
            {
                case View.contact:
                    //ContentArea.Content = contactView; break;
                    ContentArea.Content = contactView; break;
                case View.edit:
                    // ContentArea.Content = editView; break;
                    ContentArea.Content = editView; break;
                //case Views.delete:
                //CurrentView = deleteView; break;
                //break;
                // case UserControls.settings:
                // CurrentView = settingsView; break;
                //break;
                default:
                    ContentArea.Content = contactView; break;
            }
        }
        public static void PopulateContactView()
        {
            Contact contact = Contact.CurrentContact;

            contactView.txtfullName.Text = (contact.FullName is not null) ? contact.FullName.Trim() : "";
            contactView.txtStreet.Text = (contact.Street is not null) ? contact.Street : "";
            contactView.txtCity.Text = (contact.City is not null) ? contact.City : "";
            contactView.txtState.Text = (contact.State is not null) ? contact.State : "";
            contactView.txtZip.Text = (contact.ZipCode is not null) ? contact.ZipCode : "";
            contactView.txtEmail.Text = (contact.Email is not null) ? contact.Email : "";
            contactView.txtPhone.Text = (contact.PhoneNumber is not null) ? contact.PhoneNumber : "";
            contactView.txtWebsite.Text = (contact.Website is not null) ? contact.Website : "";
            contactView.txtNotes.Text = (contact.Notes is not null) ? contact.Notes : "";
        }
        public static void PopulateEditView()
        {
            Contact contact = Contact.CurrentContact;

            editView.txtbxFirst.Text = (contact.FirstName is not null) ? contact.FirstName.Trim() : "";
            editView.txtbxMiddle.Text = (contact.MiddleName is not null) ? contact.MiddleName : "";
            editView.txtbxNick.Text = (contact.NickName is not null) ? contact.NickName : "";
            editView.txtbxLast.Text = (contact.LastName is not null) ? contact.LastName : "";
            editView.txtbxTitle.Text = (contact.Title is not null) ? contact.Title : "";
            editView.txtbxBirthday.Text = (contact.Birthday is not null) ? contact.Birthday.ToString() : "";
            editView.txtbxEmail.Text = (contact.Email is not null) ? contact.Email : "";
            editView.txtbxPhone.Text = (contact.PhoneNumber is not null) ? contact.PhoneNumber : "";
            editView.txtbxStreet.Text = (contact.Street is not null) ? contact.Street : "";
            editView.txtbxCity.Text = (contact.City is not null) ? contact.City : "";
            editView.txtbxState.Text = (contact.State is not null) ? contact.State : "";
            editView.txtbxZip.Text= (contact.ZipCode is not null) ? contact.ZipCode : "";
            editView.txtbxCountry.Text= (contact.Country is not null) ? contact.Country : "";

            editView.txtbxWebsite.Text = (contact.Website is not null) ? contact.Website : "";
            editView.txtbxNotes.Text = (contact.Notes is not null) ? contact.Notes : "";
        }
    }
}
