using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                // return connection.Query<Contact>("SELECT * FROM dbo.tblContacts WHERE FirstName LIKE @FirstName", new { FirstName = contactSearch }).ToList();
                connection.Query<Contact>("[dbo].[spUpdateContact]");

            }
        }

    }
}
