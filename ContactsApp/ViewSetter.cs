using ContactsApp.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
        private static readonly SettingsView settingsView = new();
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
                    break;
                case View.Edit:
                    EditView.Reset();
                    break;
                case View.Add:
                    AddView = new();
                    break;
                case View.Home:
                    break;
            }
        }
        public static void SetView(View view)
        {
            switch (view)
            {
                case View.Contact:
                    ContentArea.Content = ContactView; 
                    break;
                case View.Edit:
                    ContentArea.Content = EditView; 
                    break;
                case View.Add:
                    ContentArea.Content = AddView; 
                    break;
                case View.Delete:
                    ContentArea.Content = DeleteView; 
                    break;
                case View.Settings:
                    ContentArea.Content = SettingsView; 
                    break;
                case View.Home:
                    ContentArea.Content = HomeView; 
                    break;
                default:
                    ContentArea.Content = ContactView; 
                    break;
            }
        }
        public static void PopulateContactView()
        {

            ContactView.PopulateView(ContactView);
        }
        public static void PopulateEditView()
        {
            EditView.PopulateView();
        }
    }
}
