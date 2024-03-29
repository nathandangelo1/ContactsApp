﻿using System;
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
            //ViewManager.ClearView(View.Add);
            ViewManager.AddView = new();

            if (Contact.CurrentContact is not null)
            {
                ViewManager.SetView(View.Contact);
            }
            else
            {
                ViewManager.SetView(View.Home);
            }
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            // If 
            DateTime? BdayDateTime = (datePickerBday.SelectedDate is not null) ? 
                new DateTime
                (
                    datePickerBday.SelectedDate.Value.Year, 
                    datePickerBday.SelectedDate.Value.Month, 
                    datePickerBday.SelectedDate.Value.Day
                ) : null;

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
            ViewManager.SetView(View.Contact);
            ViewManager.ClearView(View.Edit);
            Contact.CurrentContact = edit;
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
                imgContact.Height = 200;
                imgContact.Width = 200;
            }

        }
    }
}
