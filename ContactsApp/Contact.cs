using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsApp
{
    public class Contact
    {
        public static Contact CurrentContact;

        public int Index { get; set; }
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? NickName { get; set; }
        public string? Title { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public string? Website { get; set; }
        public string? Notes { get; set; }
        public string? Picture { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsActive { get; set; }


        public string FullName
        {
            get
            {
                if (IsFavorite)
                {
                    return (NickName is not null) ? $"\U0001f9e1 {NickName} {LastName}" : $"{FirstName} {LastName}";
                }
                else
                {
                    return (NickName is not null) ? $"   {NickName} {LastName}" : $"{FirstName} {LastName}";

                }
            }
        }
    }
}

