using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public void UpdateContact(Contact edit)
        {
            int success;
            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                foreach (PropertyInfo propertyInfo in edit.GetType().GetProperties())
                {
                    // do stuff here
                }
                var sql = " exec [dbo].[spUpdateContact] " +
                    "@Id,@FirstName,@MiddleName,@NickName,@LastName,@Title,@Birthday," +
                    "@Email,@PhoneNumber,@Street,@City,@State,@ZipCode,@Country," +
                    "@Website,@Notes,@IsFavorite ";

                var values = new { 
                    Id = edit.Id, FirstName = edit.FirstName, 
                    MiddleName = edit.MiddleName, NickName = edit.NickName, 
                    LastName = edit.LastName, Title = edit.Title, 
                    Birthday = edit.Birthday, Email = edit.Email, 
                    PhoneNumber = edit.PhoneNumber, Street = edit.Street, 
                    City = edit.City, State = edit.State, 
                    ZipCode = edit.ZipCode, Country = edit.Country, 
                    Website = edit.Website, Notes = edit.Notes, 
                    IsFavorite = edit.IsFavorite 
                };

                var result = connection.Query(sql, values);

                if(result is not null) 
                { 
                    Contact.CurrentContact = edit;
                    Contact.contacts[Contact.contacts.FindIndex(x => x.Id == edit.Id)] = edit;
                }
                //MessageBox.Show(result.ToString());
            }
        }
        public void AddContact(Contact edit)
        {
            int success;
            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                foreach (PropertyInfo propertyInfo in edit.GetType().GetProperties())
                {
                    // do stuff here
                }
                var sql = " exec [dbo].[spAddContact] " +
                    "@Id,@FirstName,@MiddleName,@NickName,@LastName,@Title,@Birthday," + 
                    "@Email,@PhoneNumber,@Street,@City,@State,@ZipCode,@Country," +
                    "@Website,@Notes,@IsFavorite ";

                var values = new
                {
                    Id = edit.Id,
                    FirstName = edit.FirstName,
                    MiddleName = edit.MiddleName,
                    NickName = edit.NickName,
                    LastName = edit.LastName,
                    Title = edit.Title,
                    Birthday = edit.Birthday,
                    Email = edit.Email,
                    PhoneNumber = edit.PhoneNumber,
                    Street = edit.Street,
                    City = edit.City,
                    State = edit.State,
                    ZipCode = edit.ZipCode,
                    Country = edit.Country,
                    Website = edit.Website,
                    Notes = edit.Notes,
                    IsFavorite = edit.IsFavorite
                };

                var result = connection.Query(sql, values);

                if (result is not null)
                {
                    Contact.CurrentContact = edit;
                    Contact.contacts[Contact.contacts.FindIndex(x => x.Id == edit.Id)] = edit;
                }
                //MessageBox.Show(result.ToString());
            }
        }
    }
}
