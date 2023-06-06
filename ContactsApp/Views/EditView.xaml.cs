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
        public EditView()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ViewSetter.ClearView(View.edit);
            ViewSetter.SetView(View.contact);
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if(!DateOnly.TryParse(txtbxBirthday.Text, out DateOnly birthday)){
                WarningException we = new ();
            };
            Contact edit = new()
            {
                FirstName = txtbxFirst.Text,
                MiddleName = txtbxMiddle.Text,
                NickName = txtbxNick.Text,
                LastName = txtbxLast.Text,
                Title = txtbxTitle.Text,
                Birthday = birthday,
                Email= txtbxEmail.Text,
                PhoneNumber = txtbxPhone.Text,
                Street = txtbxStreet.Text,
                City = txtbxCity.Text,
                State = txtbxState.Text,
                Zip = txtbxZip.Text,
                Country = txtbxCountry.Text,
                Website = txtbxWebsite.Text,
                Notes = txtbxNotes.Text

            };
        }
    }
}
