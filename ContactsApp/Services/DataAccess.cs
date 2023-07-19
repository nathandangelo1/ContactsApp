using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using static ContactsApp.MainWindow;
using static Dapper.SqlMapper;

namespace ContactsApp.Services
{
    public class DataAccess
    {
        public List<Contact> GetContacts()
        {
            // GETS CONNECTION STRING FOR CONTACT DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                return connection.Query<Contact>("[dbo].[spGetContacts]").ToList();
            }
        }

        public void DeleteContact(Contact edit)
        {
            int newId;
            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                // Create a DynamicParameters object
                var parameters = new DynamicParameters();

                // Add input parameters
                parameters.Add("@id", edit.Id);

                // Execute the command using Dapper
                var returns = connection.Execute("spDeleteContact", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateContact(Contact edit)
        {
            int success;

            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                var sql = " exec [dbo].[spUpdateContact] " +
                    "@Id,@FirstName,@MiddleName,@NickName,@LastName,@Title,@Birthday," +
                    "@Email,@PhoneNumber,@Street,@City,@State,@ZipCode,@Country," +
                    "@Website,@Notes,@IsFavorite,@Picture";

                var values = new
                {
                    edit.Id,
                    edit.FirstName,
                    edit.MiddleName,
                    edit.NickName,
                    edit.LastName,
                    edit.Title,
                    edit.Birthday,
                    edit.Email,
                    edit.PhoneNumber,
                    edit.Street,
                    edit.City,
                    edit.State,
                    edit.ZipCode,
                    edit.Country,
                    edit.Website,
                    edit.Notes,
                    edit.IsFavorite,
                    edit.Picture
                };

                var result = connection.Query(sql, values);

                if (result is not null)
                {
                    // Update currently selected contact
                    Contact.CurrentContact = edit;

                    // Update ContactsList element
                    CL.Contacts[CL.Contacts.IndexOf(CL.Contacts.Single(x => x.Id == edit.Id))] = edit;
                }
            }
        }
        public void DeactivateContact(Contact edit)
        {
            int newId;

            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                // Create a DynamicParameters object
                var parameters = new DynamicParameters();

                // Add input parameters
                parameters.Add("@id", edit.Id);

                // Execute the command using Dapper
                var returns = connection.Execute("spDeactivateContact", parameters, commandType: CommandType.StoredProcedure);

            }
        }

        public void AddContact(Contact edit)
        {
            int newId;
            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                // Create a DynamicParameters object
                var parameters = new DynamicParameters();

                #region // region: Input Parameters
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
                #endregion

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
                    CL.Contacts.Add(edit);
                }
            }
        }
        public void UpdateSettings()
        {
            int success;

            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                var sql = " EXEC [dbo].[UpdateSettings] @SettingName,@Val;";
                var sql2 = " EXEC [dbo].[UpdateSettings] @SettingName,@Val";

                var values = new
                {
                    SettingName = "SortByFirstName",
                    Val = Settings.SortByFirstName.ToString()
                };
                var values2 = new
                {
                    SettingName = "BdayRange",
                    Val = Settings.BirthdayRange.ToString()
                };

                var result = connection.Query(sql, values);
                var result2 = connection.Query(sql2, values2);

                if (result == null || result2 == null)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public class SettingsModel
        {
            public int SettingId { get; set; }
            public string SettingName { get; set; }
            public string Value { get; set; }
        }

        public List<SettingsModel> GetSettings()
        {
            int success;

            // GETS CONNECTION STRING FOR MOVIES DB FROM HELPER CLASS
            using (var connection = new SqlConnection(Helper.CnnVal("contacts")))
            {
                var sql = "SELECT * FROM dbo.Settings";

                //SettingId SettingName     Value   LastModified
                //1         SortByFirstName ***     2023 - 07 - 17 00:07:59.123
                //2         BdayRange       ***     2023 - 07 - 17 00:07:59.707

                return connection.Query<SettingsModel>(sql).ToList();
               
            }
        }
    }
}
