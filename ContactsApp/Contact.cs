using System;

namespace ContactsApp
{
    public class Contact
    {
        public static Contact CurrentContact;

        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? NickName { get; set; }

        public string? Title { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? Country { get; set; }

        public string? Website { get; set; }

        public string? Notes { get; set; }

        private string? picture;

        public string? Picture
        {
            get
            {
                if (picture is null) return "pack://application:,,,/Resources/noImage.png";
                else return picture;
            }
            set
            {
                if (picture != value)
                {
                    picture = value;
                }
            }

        }

        public int IsFavorite { get; set; }
        public int IsActive { get; set; }

        // Displays name based on whether Contact is favorite(adds Heart emoji) and whether Contact has a nickname
        public string FullName
        {
            get
            {
                if (IsFavorite == 1)
                {
                    return (NickName is not null) ? $"{NickName} {LastName} \U0001f9e1 " : $"{FirstName} {LastName} \U0001f9e1";
                }
                else
                {
                    return (NickName is not null) ? $"{NickName} {LastName}" : $"{FirstName} {LastName}";
                }
            }
        }
        public string? GroupingName
        {
            get
            {
                if ((bool)Settings.SortByFirstName)
                {
                    return (NickName is not null) ? $"{NickName}" : (FirstName is not null) ? $"{FirstName}" : (LastName is not null) ? $"{LastName}" : null;
                }
                
                else
                {
                    return (LastName is not null) ? $"{LastName}" : null;
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

