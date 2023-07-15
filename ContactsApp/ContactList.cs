using System.Collections.ObjectModel;

namespace ContactsApp
{
    public class ContactsList : ObservableCollection<Contact>
    {
        private ObservableCollection<Contact> _contacts;

        public ObservableCollection<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                _contacts = value;
            }
        }
    }
}
