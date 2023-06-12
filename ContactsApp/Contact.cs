using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsApp
{
    public class Contact : INotifyPropertyChanged
    {
        public static Contact CurrentContact;
        public static List<Contact> favorites;
        public static List<Contact> contacts;

        //public int Index { get; set; }

        private int id;
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string? firstName;
        public string? FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        private string? middleName;
        public string? MiddleName
        {
            get
            {
                return middleName;
            }
            set
            {
                if (middleName != value)
                {
                    middleName = value;
                    OnPropertyChanged(nameof(MiddleName));
                }
            }
        }

        private string? lastName;
        public string? LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        private string? nickname;
        public string? NickName
        {
            get
            {
                return nickname;
            }
            set
            {
                if (nickname != value)
                {
                    nickname = value;
                    OnPropertyChanged(nameof(NickName));
                }
            }
        }

        private string? title;
        public string? Title
        {
            get
            {
                return title;
            }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private string? birthday;
        public string? Birthday
        {
            get
            {
                return birthday;
            }
            set
            {
                if (birthday != value)
                {
                    birthday = value;
                    OnPropertyChanged(nameof(Birthday));
                }
            }
        }

        private string? email;
        public string? Email
        {
            get
            {
                return email;
            }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        private string? phoneNumber;
        public string? PhoneNumber 
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                if (phoneNumber != value)
                {
                    phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        private string? street;
        public string? Street 
        {
            get
            {
                return street;
            }
            set
            {
                if (street != value)
                {
                    street = value;
                    OnPropertyChanged(nameof(Street));
                }
            }
        }

        private string? city;
        public string? City 
        {
            get
            {
                return city;
            }
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        private string? state;
        public string? State 
        {
            get
            {
                return state;
            }
            set
            {
                if (state != value)
                {
                    state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        private string? zipCode;
        public string? ZipCode 
        {
            get
            {
                return zipCode;
            }
            set
            {
                if (zipCode != value)
                {
                    zipCode = value;
                    OnPropertyChanged(nameof(ZipCode));
                }
            }
        }

        private string? country;
        public string? Country 
        {
            get
            {
                return country;
            }
            set
            {
                if (country != value)
                {
                    country = value;
                    OnPropertyChanged(nameof(Country));
                }
            }
        }

        private string? website;
        public string? Website 
        {
            get
            {
                return website;
            }
            set
            {
                if (website != value)
                {
                    website = value;
                    OnPropertyChanged(nameof(Website));
                }
            }
        }
        
        private string? notes;
        public string? Notes {
            get
            {
                return notes;
            }
            set
            {
                if (notes != value)
                {
                    notes = value;
                    OnPropertyChanged(nameof(Notes));
                }
            }
        }

        private string? picture;

        public string? Picture 
        {
            get
            {
                return picture;
            }
            set
            {
                if (picture != value)
                {
                    picture = value;
                    OnPropertyChanged(nameof(Picture));
                }
            }

        }
        
        private int isFavorite;
        public int IsFavorite 
        {
            get
            {
                return IsFavorite;
            }
            set
            {
                if (isFavorite != value)
                {
                    isFavorite = value;
                    OnPropertyChanged(nameof(IsFavorite));
                }
            }
        }
        private int isActive;
        public int IsActive 
        {
            get
            {
                return isActive;
            }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public string FullName
        {
            get
            {
                if (IsFavorite==1)
                {
                    return (NickName is not null) ? $"\U0001f9e1 {NickName} {LastName}" : $"\U0001f9e1 {FirstName} {LastName}";
                }
                else
                {
                    return (NickName is not null) ? $"{NickName} {LastName}" : $"{FirstName} {LastName}";

                }
            }
        }
        public bool Equals(Contact? obj)
        {
            // Check for null values and compare types
            if (obj == null || GetType() != obj.GetType())
                return false;
            // Check Values
            else if (obj.Id.Equals(Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

