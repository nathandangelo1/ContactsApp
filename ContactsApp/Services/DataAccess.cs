using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

using System.Collections.ObjectModel;

namespace ContactsApp.Services
{
    public class DataAccess
    {
        public List<Contact> GetContacts()
        {
            // GETS CONNECTION STRING FOR CONTACT DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
               // return connection.Query<Contact>("SELECT * FROM dbo.tblContacts WHERE FirstName LIKE @FirstName", new { FirstName = contactSearch }).ToList();
                return connection.Query<Contact>("[dbo].[spGetContacts]").ToList();
            }
        }

        public void DeleteContact(Contact edit)
        {
            int newId;
            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                //foreach (PropertyInfo propertyInfo in edit.GetType().GetProperties())
                //{
                //    // do stuff here
                //}
                // Create a DynamicParameters object
                var parameters = new DynamicParameters();
                // Add input parameters
                parameters.Add("@id", edit.Id);

                // Add output parameter for new ID
                //parameters.Add("@returnId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                // Execute the command using Dapper
                var returns = connection.Execute("spDeleteContact", parameters, commandType: CommandType.StoredProcedure);
                // Get the output parameter value using Dapper
                //newId = parameters.Get<int>("returnId");

                if (returns == -1)
                {

                }
            }
        }

        public void UpdateContact(Contact edit)
        {
            int success;
            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                //foreach (PropertyInfo propertyInfo in edit.GetType().GetProperties())
                //{
                //    // do stuff here
                //}
                var sql = " exec [dbo].[spUpdateContact] " +
                    "@Id,@FirstName,@MiddleName,@NickName,@LastName,@Title,@Birthday," +
                    "@Email,@PhoneNumber,@Street,@City,@State,@ZipCode,@Country," +
                    "@Website,@Notes,@IsFavorite";

                var values = new { 
                    edit.Id, edit.FirstName, 
                    edit.MiddleName, edit.NickName, 
                    edit.LastName, edit.Title, 
                    edit.Birthday, edit.Email, 
                    edit.PhoneNumber, edit.Street, 
                    edit.City, edit.State, 
                    edit.ZipCode, edit.Country, 
                    edit.Website, edit.Notes, 
                    edit.IsFavorite 
                };

                var result = connection.Query(sql, values);

                if(result is not null) 
                { 
                    Contact.CurrentContact = edit;
                    int editId = edit.Id;
                    for (int i = 0; i < MainWindow.CL.Contacts.Count; i++)
                    {
                        Contact con = MainWindow.CL.Contacts[i];
                        int conId = con.Id;
                        if (conId == editId)
                        {
                            MainWindow.CL.Contacts[i] = edit;
                            break;
                        }
                    }
                    
                   // Contact.favorites[Contact.favorites.FindIndex(x => x.Id == edit.Id)] = edit;
                    //Refresh();
                }
                //Refresh();
            }
        }
        public void DeactivateContact(Contact edit)
        {
            int newId;
            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                //foreach (PropertyInfo propertyInfo in edit.GetType().GetProperties())
                //{
                //    // do stuff here
                //}
                // Create a DynamicParameters object
                var parameters = new DynamicParameters();
                // Add input parameters
                parameters.Add("@id", edit.Id);
                
                // Add output parameter for new ID
                //parameters.Add("@returnId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                // Execute the command using Dapper
                var returns = connection.Execute("spDeactivateContact", parameters, commandType: CommandType.StoredProcedure);
                // Get the output parameter value using Dapper
                //newId = parameters.Get<int>("returnId");

                if (returns == -1)
                {
                    
                }
            }
        }

        public void AddContact(Contact edit)
        {
            int newId;
            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                //foreach (PropertyInfo propertyInfo in edit.GetType().GetProperties())
                //{
                //    // do stuff here
                //}
                // Create a DynamicParameters object
                var parameters = new DynamicParameters();
                // Add input parameters
                parameters.Add("@first", edit.FirstName);
                parameters.Add("@middle", edit.MiddleName);
                parameters.Add("@nick", edit.NickName);
                parameters.Add("@last", edit.LastName);
                parameters.Add("@title", edit.Title);
                parameters.Add("@birthday", edit.Birthday);
                parameters.Add("@email", edit.Email);
                parameters.Add("@phone", edit.PhoneNumber);
                parameters.Add("@street", edit.Street);
                parameters.Add("@city", edit.City);
                parameters.Add("@state", edit.State);
                parameters.Add("@zip", edit.ZipCode);
                parameters.Add("@country", edit.Country);
                parameters.Add("@website", edit.Website);
                parameters.Add("@notes", edit.Notes);
                parameters.Add("@fav", edit.IsFavorite);
                parameters.Add("@pic", edit.Picture);
                // Add output parameter for new ID
                parameters.Add("@returnId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                // Execute the command using Dapper
                var returns = connection.Execute("spAddContact", parameters, commandType: CommandType.StoredProcedure);
                // Get the output parameter value using Dapper
                newId = parameters.Get<int>("returnId");
                         
                if (newId > 0)
                {
                    edit.Id = newId;
                    Contact.CurrentContact = edit;
                    MainWindow.CL.Contacts.Add(edit);
                    //MainWindow.CL.Contacts = new ObservableCollection<Contact>(MainWindow.CL.Contacts.OrderBy(x => x.FirstName));
                    //Contact.favorites = Contact.contacts.Where(x => x.IsFavorite == 1).ToList();
                    //Refresh();
                } 
            }
        }
        //private void Refresh()
        //{
        //    MainWindow window = (MainWindow)Application.Current.MainWindow;

        //    window.RefreshListView();
        //    ViewSetter.PopulateContactView();  <<<---- you need this
        //}
    }
}
