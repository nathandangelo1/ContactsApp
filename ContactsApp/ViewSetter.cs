using ContactsApp.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ContactsApp
{
    public sealed class ViewManager
    {
        public static ContentControl ContentArea { get; set; }
        
        private static readonly ContactView contactView = new();
        public static ContactView ContactView
        {
            get 
            {
                return contactView;
            } 

        }
        
        private static EditView editView;
        public static EditView EditView
        {
            get 
            {
                if (editView is null)
                {
                    return new EditView();
                }
                return editView;
            } 
            set
            {
                editView = value;
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
                if (settingsView is null)
                {
                    return new SettingsView();
                }
                return settingsView;
            }

        }
        
        private static AddView addView;
        public static AddView AddView
        {
            get
            {
                if (addView is null)
                {
                    return new AddView();
                }
                return addView;
            }
            set
            {
                addView = value;
            }

        }

        public static void ClearView(View view)
        {
            switch (view)
            {
                case View.Edit:
                    EditView.Reset();
                    break;
                case View.Add:
                    AddView = new();
                    break;
            }
        }
        public static void SetView(View view)
        {
            switch (view)
            {
                case View.Contact:
                    ContentArea.Content = ContactView;
                    ContactView.PopulateView();
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
                default:
                    ContentArea.Content = ContactView; 
                    break;
            }
        }
    }
}
