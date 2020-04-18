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

        public bool AuthenticateUser(ref User user)
        {
            DataTable dtUser = new DataTable();
            dtUser = _db.GetData($"SELECT Usuario, Contrasena FROM Usuario WHERE Usuario = '{user.Username}'", autoConnect: true);

            if (dtUser.Rows.Count > 0)
                if (AES256.Decrypt(dtUser.Rows[0]["Contrasena"].ToString(), _config.Database.EncryptionKey) == user.Password)
                {
                    GetUserDataByUsername(ref user);
                }

            return ((user.Id == default) ? false : true);
        }

        public User GetUserDataByUsername(ref User user)
        {
            DataTable dtUser = new DataTable();
            var query = $"SELECT * FROM Usuario WHERE Usuario = '{user.Username}'";

            dtUser = _db.GetData(query: query, autoConnect: true);

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
    }
}
