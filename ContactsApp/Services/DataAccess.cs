using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ContactsApp.Services
{
    public class DataAccess
    {
        public List<Contact> GetContacts()
        {
            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
               // return connection.Query<Contact>("SELECT * FROM dbo.tblContacts WHERE FirstName LIKE @FirstName", new { FirstName = contactSearch }).ToList();
                return connection.Query<Contact>("[dbo].[spGetContacts]").ToList();

            }
        }
        public void UpdateContact(Contact Contact)
        {
            int success;
            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                var sql = " exec [dbo].[spUpdateContact] " +
                    "@Id,@FirstName,@MiddleName,@NickName,@LastName,@Title,@Birthday," +
                    "@Email,@PhoneNumber,@Street,@City,@State,@ZipCode,@Country," +
                    "@Website,@Notes,@IsFavorite ";
                var values = new { 
                    Id = Contact.Id, FirstName = Contact.FirstName, 
                    MiddleName = Contact.MiddleName, NickName = Contact.NickName, 
                    LastName = Contact.LastName, Title = Contact.Title, 
                    Birthday = Contact.Birthday.ToString(), Email = Contact.Email, 
                    PhoneNumber = Contact.PhoneNumber, Street = Contact.Street, 
                    City = Contact.City, State = Contact.State, 
                    ZipCode = Contact.ZipCode, Country = Contact.Country, 
                    Website = Contact.Website, Notes = Contact.Notes, 
                    IsFavorite = Contact.IsFavorite };
                var result = connection.Query(sql, values);
                Console.WriteLine(result.ToString());

            }
        }

    }
}
