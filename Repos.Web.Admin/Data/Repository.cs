using Repos.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mssql.dbman;
using System.Diagnostics;
using System.Data;
using SimpleAES;
using System.Data.SqlClient;
using Repos.Web.Admin.ViewModels;

namespace Repos.Web.Admin.Data
{
    public class Repository : IRepository
    {
        private MSSQLServer _db;
        private readonly Config _config;
        
        public Repository(Config config)
        {
            _config = config;
            _db = new MSSQLServer();
            _db.Server = _config.Database.Server;
            _db.Database = _config.Database.Database;
            _db.User = _config.Database.User;
            _db.Password = _config.Database.Password;
        }
        /// <summary>
        /// Checks if user credentials are valid
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AuthenticateUser(ref User user)
        {
            DataTable dtUser = new DataTable();
            dtUser = _db.GetData($"SELECT Usuario, Contrasena FROM Usuario WHERE Usuario = '{user.Username}' AND Estatus = 1", autoConnect: true);

            if (dtUser.Rows.Count > 0)
                if (AES256.Decrypt(dtUser.Rows[0]["Contrasena"].ToString(), _config.Database.EncryptionKey) == user.Password)
                {
                    GetUserDataByUsername(ref user);
                }

            return ((user.Id == default) ? false : true);
        }
        /// <summary>
        /// Fills out all user fields on user object
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>Returns user with all of it's fields from the repository</returns>
        public User GetUserDataByUsername(ref User user)
        {
            var query = $"SELECT * FROM Usuario WHERE Usuario = '{user.Username}'";
            DataTable dtUser = _db.GetData(query: query, autoConnect: true);

            if (dtUser.Rows.Count > 0)
            {
                user.Id = (Guid)dtUser.Rows[0]["ID"];
                user.CreateDate = (DateTime)dtUser.Rows[0]["FechaRegistro"];
                user.Username = (string)dtUser.Rows[0]["Usuario"];
                user.Name = (string)dtUser.Rows[0]["Nombre"];
                user.LastName = (string)dtUser.Rows[0]["Apellido"];
                user.Password = (string)dtUser.Rows[0]["Contrasena"];
                user.Status = (bool)dtUser.Rows[0]["Estatus"];
            }
            return user;
        }
        /// <summary>
        /// Gets a datatable with all the companies on the repository
        /// </summary>
        /// <param name="justActive">Specifies to get only active companies (Status = true | Estado = 1)</param>
        /// <returns>List of companies</returns>
        public List<Company> GetCompanies(bool justActive = false)
        {
            DataTable dtCompanies = new DataTable();
            
            List<Company> companies = new List<Company>();
            var query = "SELECT * FROM Empresa";
            if (justActive)
                query += " WHERE Estatus = 1";

            dtCompanies = _db.GetData(query, autoConnect: true);
            foreach (DataRow row in dtCompanies.Rows)
            {
                Company company = new Company();
                company.Id = (Guid)row["ID"];
                company.CreateDate = (DateTime)row["FechaRegistro"];
                company.Code = (string)row["Codigo"];
                company.Name = (string)row["Nombre"];
                company.Status = (bool)row["Estatus"];

                companies.Add(company);
            }
            return companies;
        }

        /// <summary>
        /// Creates a new company on the repository
        /// </summary>
        /// <param name="company">Company object</param>
        /// <returns>Successful or unsuccessful transaction</returns>
        public bool CreateCompany(Company company)
        {
            bool success = false;
            var query = $"INSERT INTO Empresa (Codigo, Nombre) VALUES('{company.Code}', '{company.Name}')";

            if (_db.SetConnection())
                if (_db.WriteData(query))
                    success = true;

            return success;
        }
        /// <summary>
        /// Gets a company by it's Id from the repository
        /// </summary>
        /// <param name="Id">Id (Guid) from the company</param>
        /// <returns>Company object</returns>
        public Company GetCompanyById(Guid Id)
        {
            Company company = new Company();
            var query = $"SELECT * FROM Empresa WHERE Id = '{Id}'";
            DataTable dtCompany = _db.GetData(query: query, autoConnect: true);
            
            if (dtCompany.Rows.Count > 0)
            {
                company.CreateDate = (DateTime)dtCompany.Rows[0]["FechaRegistro"];
                company.Code = (string)dtCompany.Rows[0]["Codigo"];
                company.Name = (string)dtCompany.Rows[0]["Nombre"];
                company.Status = (bool)dtCompany.Rows[0]["Estatus"];
            }
            return company;
        }

        /// <summary>
        /// Edits a company's attributes from the repository
        /// </summary>
        /// <param name="company">Company object</param>
        /// <returns>Successful or unsuccessful transaction</returns>
        public bool EditCompany(Company company)
        {
            bool success = false;
            var query = $"UPDATE Empresa SET Codigo = '{company.Code}', Nombre = '{company.Name}', Estatus = '{company.Status}' WHERE Id = '{company.Id}'";

            if (_db.SetConnection())
                if (_db.WriteData(query))
                    success = true;

            return success;
        }

        /// <summary>
        /// Deletes a company from the repository
        /// </summary>
        /// <param name="company">Company object</param>
        /// <returns>Successful or unsuccessful transaction</returns>
        public bool DeleteCompany(Company company)
        {
            bool success = false;
            var query = $"DELETE FROM Empresa WHERE Id = '{company.Id}'";

            if (_db.SetConnection())
                if (_db.WriteData(query))
                    success = true;

            return success;
        }
        /// <summary>
        /// Gets a collection of stores from the repository
        /// </summary>
        /// <param name="justActive">Defines if just active stores</param>
        /// <returns>Collection of stores</returns>
        public StoreViewModel GetStores(bool justActive = false)
        {
            StoreViewModel storeViewModel = new StoreViewModel();
            var query = "SELECT * FROM Sucursal";

            if (justActive)
                query += $" WHERE Estatus = 1";

            DataTable dtStores = _db.GetData(query, autoConnect: true);
            foreach (DataRow row in dtStores.Rows)
            {
                storeViewModel.Stores.Add(new Store()
                {
                    Id = (Guid)row["ID"],
                    CreateDate = (DateTime)row["FechaRegistro"],
                    Code = (string)row["Codigo"],
                    Name = (string)row["Nombre"],
                    Address = AddStringFieldValue(row, "Direccion"),
                    Status = (bool)row["Estatus"],
                    Company = new Company
                    {
                        Id = (Guid)row["EmpresaID"]
                    }
                });
            }
            return storeViewModel;
        }

        private string AddStringFieldValue(DataRow row, string fieldName)
        {
            if (!DBNull.Value.Equals(row[fieldName]))
                return (string)row[fieldName];
            else
                return string.Empty;
        }
    }
}
