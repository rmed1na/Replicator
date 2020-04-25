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
                company.Id = (Guid)dtCompany.Rows[0]["ID"];
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
                    Name = AddPossibleNullString(row, "Nombre"),
                    Address = AddPossibleNullString(row, "Direccion"),
                    Status = (bool)row["Estatus"],
                    Company = new Company
                    {
                        Id = (Guid)row["EmpresaID"]
                    }
                });
            }
            return storeViewModel;
        }
        
        /// <summary>
        /// Handles DbNull values from repository
        /// </summary>
        /// <param name="row">DataRow that's being handled</param>
        /// <param name="fieldName">Name of the column or field to check</param>
        /// <returns>Actual string or empty one</returns>
        private string AddPossibleNullString(DataRow row, string fieldName)
        {
            if (!DBNull.Value.Equals(row[fieldName]))
                return (string)row[fieldName];
            else
                return string.Empty;
        }
        /// <summary>
        /// Gets a store by it's Id from the repository
        /// </summary>
        /// <param name="Id">Store Id</param>
        /// <returns>Store object</returns>
        public Store GetStoreById(Guid Id)
        {
            DataTable dtStore = _db.GetData($"SELECT * FROM Sucursal WHERE ID = '{Id}'", autoConnect: true);
            Store store = new Store();
            foreach (DataRow row in dtStore.Rows)
            {
                store.Id = (Guid)row["ID"];
                store.Code = (string)row["Codigo"];
                store.Name = AddPossibleNullString(row, "Nombre");
                store.CreateDate = (DateTime)row["FechaRegistro"];
                store.Address = AddPossibleNullString(row, "Direccion");
                store.Status = (bool)row["Estatus"];
                store.Company = new Company()
                {
                    Id = (Guid)row["EmpresaID"]
                };
            }
            return (store);
        }
        /// <summary>
        /// Edits a store attributes from an store object
        /// </summary>
        /// <param name="store">Store object</param>
        /// <returns>Successful or unsuccessful transaction</returns>
        public bool EditStore(Store store)
        {
            bool success = false;
            var query = $"UPDATE Sucursal SET Codigo = '{store.Code}', Nombre = '{store.Name}', Direccion = '{store.Address}', Estatus = '{store.Status}'";

            if (_db.SetConnection())
                if (_db.WriteData(query))
                    success = true;

            return success;
        }
        /// <summary>
        /// Creates a new store
        /// </summary>
        /// <param name="store">Store object</param>
        /// <returns>Successful or unsuccessful transaction</returns>
        public bool CreateStore(Store store)
        {
            bool success = false;
            var query = $"INSERT INTO Sucursal (EmpresaID, Codigo, Nombre, Direccion) VALUES((SELECT TOP 1 ID FROM Empresa WHERE Codigo = '{store.Company.Code}')," +
                $"'{store.Code}', '{store.Name}', NULLIF('{store.Address}', ''))";

            if (_db.SetConnection())
                if (_db.WriteData(query))
                    success = true;

            return success;
        }

        public bool DeleteStore(Store store)
        {
            bool success = false;
            var query = $"DELETE FROM Sucursal WHERE ID = '{store.Id}'";

            if (_db.SetConnection() && (_db.WriteData(query)))
                success = true;

            return success;
        }

        public WarehouseViewModel GetWarehouses(bool justActive = false)
        {
            WarehouseViewModel warehouseViewModel = new WarehouseViewModel();
            var query = $"SELECT * FROM Almacen";

            if (justActive)
                query += $" WHERE Estatus = 1";

            DataTable dtWarehouses = _db.GetData(query, autoConnect: true);
            foreach (DataRow row in dtWarehouses.Rows)
            {
                warehouseViewModel.Warehouses.Add(new Warehouse()
                {
                    Id = (Guid)row["ID"],
                    CreateDate = (DateTime)row["FechaRegistro"],
                    Code = (string)row["Codigo"],
                    Name = (string)row["Nombre"],
                    Status = (bool)row["Estatus"],
                    Store = new Store()
                    {
                        Id = (Guid)row["SucursalID"]
                    }
                });
            }
            return warehouseViewModel;
        }

        public bool CreateWarehouse(Warehouse warehouse)
        {
            var query = $"INSERT INTO Almacen (SucursalID, Codigo, Nombre, Direccion) " +
                $"VALUES((SELECT TOP 1 ID FROM Sucursal WHERE Codigo = '{warehouse.Store.Code}'), '{warehouse.Code}', '{warehouse.Name}', NULLIF('{warehouse.Address}', ''))";

            if (_db.SetConnection() && _db.WriteData(query))
                return true;
            else
                return false;
        }

        public Warehouse GetWarehouseById(Guid Id)
        {
            var query = $"SELECT * FROM Almacen WHERE ID = '{Id}'";
            DataTable dtWarehouse = _db.GetData(query, autoConnect: true);
            Warehouse warehouse = new Warehouse();

            foreach (DataRow row in dtWarehouse.Rows)
            {
                warehouse.Id = (Guid)row["ID"];
                warehouse.Code = (string)row["Codigo"];
                warehouse.CreateDate = (DateTime)row["FechaRegistro"];
                warehouse.Name = (string)row["Nombre"];
                warehouse.Status = (bool)row["Estatus"];
                warehouse.Store = GetStoreById((Guid)row["SucursalID"]);
                warehouse.Address = AddPossibleNullString(row, "Direccion");
            }
            return warehouse;
        }

        public bool EditWarehouse(Warehouse warehouse)
        {
            var query = $"UPDATE Almacen SET SucursalID = (SELECT ID FROM Sucursal WHERE Codigo = '{warehouse.Store.Code}')," +
                $" Codigo = '{warehouse.Code}', Nombre = '{warehouse.Name}', Estatus = '{warehouse.Status}', Direccion = NULLIF('{warehouse.Address}', '')";

            if (_db.SetConnection() && _db.WriteData(query))
                return true;
            else
                return false;
        }

        public bool DeleteWarehouse(Warehouse warehouse)
        {
            var query = $"DELETE FROM Almacen WHERE ID = '{warehouse.Id}'";

            if (_db.SetConnection() && _db.WriteData(query))
                return true;
            else
                return false;
        }

        public bool CreatePos(Pos pos)
        {
            var query = $"INSERT INTO Caja (AlmacenID, Codigo, Nombre) VALUES((SELECT TOP 1 ID FROM Almacen WHERE Codigo = '{pos.Warehouse.Code}')," +
                $" '{pos.Code}', '{pos.Name}')";

            if (_db.SetConnection() && _db.WriteData(query))
                return true;
            else
                return false;
        }

        public PosViewModel GetPos(bool justActive = false)
        {
            PosViewModel posViewModel = new PosViewModel();
            var query = $"SELECT * FROM Caja";
            if (justActive)
                query += $" WHERE Estatus = 1";

            DataTable dtPos = _db.GetData(query, autoConnect: true);
            foreach (DataRow row in dtPos.Rows)
            {
                posViewModel.Pos.Add(new Pos()
                {
                    Id = (Guid)row["ID"],
                    CreateDate = (DateTime)row["FechaRegistro"],
                    Code = (string)row["Codigo"],
                    Name = (string)row["Nombre"],
                    Status = (bool)row["Estatus"],
                    Ip = AddPossibleNullString(row, "Ip"),
                    Hostname = AddPossibleNullString(row, "Hostname"),
                    Warehouse = new Warehouse()
                    {
                        Id = (Guid)row["AlmacenID"]
                    }
                });
            }
            return posViewModel;
        }

        public Pos GetPosById(Guid Id)
        {
            var query = $"SELECT * FROM Caja WHERE ID = '{Id}'";
            DataTable dtPos = _db.GetData(query, autoConnect: true);
            Pos pos = new Pos();

            foreach (DataRow row in dtPos.Rows)
            {
                pos.Id = Id;
                pos.CreateDate = (DateTime)row["FechaRegistro"];
                pos.Code = (string)row["Codigo"];
                pos.Name = (string)row["Nombre"];
                pos.Hostname = AddPossibleNullString(row, "Hostname");
                pos.Ip = AddPossibleNullString(row, "Ip");
                pos.Status = (bool)row["Estatus"];
                pos.Warehouse = GetWarehouseById((Guid)row["AlmacenID"]);
            }
            return pos;
        }

        public bool EditPos(Pos pos)
        {
            var query = $"UPDATE Caja SET Codigo = '{pos.Code}', " +
                $" Nombre = '{pos.Name}', " +
                $" Estatus = '{pos.Status}'," +
                $" AlmacenID = (SELECT TOP 1 ID FROM Almacen WHERE Codigo = NULLIF('{pos.Warehouse.Code}', ''))";

            if (_db.SetConnection() && _db.WriteData(query))
                return true;
            else
                return false;
        }

        public bool DeletePos(Pos pos)
        {
            var query = $"DELETE FROM Caja WHERE ID = '{pos.Id}'";

            if (_db.SetConnection() && _db.WriteData(query))
                return true;
            else
                return false;
        }

        public ItemViewModel GetItems(bool justActive = false)
        {
            ItemViewModel itemViewModel = new ItemViewModel();
            var query = $"SELECT * FROM Articulo";

            if (justActive)
                query = $" WHERE Estatus = 1";

            query += $" ORDER BY Referencia ASC"; 

            DataTable dtItems = _db.GetData(query, autoConnect: true);
            foreach (DataRow row in dtItems.Rows)
                itemViewModel.Items.Add(new Item()
                {
                    Id = (Guid)row["ID"],
                    CreateDate = (DateTime)row["FechaRegistro"],
                    Code = (string)row["Referencia"],
                    Description = AddPossibleNullString(row, "Descripcion"),
                    Price = (decimal)row["Precio"],
                    TaxPercentaje = (int)row["PorcentajeImpuesto"],
                    Status = (bool)row["Estatus"]
                });

            return itemViewModel;
        }
        
        public bool CreateItem(Item item)
        {
            var query = $"INSERT INTO Articulo (Referencia, Descripcion, Precio, PorcentajeImpuesto, Estatus)" +
                $" VALUES('{item.Code}', NULLIF('{item.Description}',''), {item.Price}, {item.TaxPercentaje}, '{item.Status}')";

            if (_db.SetConnection() && _db.WriteData(query))
                return true;
            else
                return false;
        }

        public Item GetItemById(Guid Id)
        {
            Item item = null;
            var query = $"SELECT * FROM Articulo WHERE ID = '{Id}'";

            foreach (DataRow row in _db.GetData(query, autoConnect: true).Rows)
                item = new Item()
                {
                    Id = (Guid)row["ID"],
                    CreateDate = (DateTime)row["FechaRegistro"],
                    Code = (string)row["Referencia"],
                    Description = AddPossibleNullString(row, "Descripcion"),
                    Price = (decimal)row["Precio"],
                    TaxPercentaje = (int)row["PorcentajeImpuesto"],
                    Status = (bool)row["Estatus"]
                };

            return item;
        }

        public bool EditItem(Item item)
        {
            var query = $"UPDATE Articulo SET Referencia = '{item.Code}'," +
                $" Descripcion = NULLIF('{item.Description}', '')," +
                $" Precio = {item.Price}," +
                $" PorcentajeImpuesto = {item.TaxPercentaje}," +
                $" Estatus = '{item.Status}'";

            if (_db.SetConnection() && _db.WriteData(query))
                return true;
            else
                return false;
        }

        public bool DeleteItem(Item item)
        {
            var query = $"DELETE FROM Articulo WHERE ID = '{item.Id}'";

            if (_db.SetConnection() && _db.WriteData(query))
                return true;
            else
                return false;
        }

        public InventoryViewModel GetInventory()
        {
            InventoryViewModel inventoryViewModel = new InventoryViewModel();
            var query = $"SELECT " + 
                        "\nI.ID," +
                        "\nA.ID AS[ArticuloID]," +
                        "\nA.Referencia," +
                        "\nA.Descripcion," +
                        "\nA.Precio," +
                        "\nA.PorcentajeImpuesto," +
                        "\nAL.ID AS[AlmacenID]," +
                        "\nAL.Codigo AS[AlmacenCodigo]," +
                        "\nAL.Nombre AS[AlmacenNombre]," +
                        "\nAL.SucursalID," +
                        "\nS.Codigo AS[SucursalCodigo]," +
                        "\nISNULL(I.Disponible, 0) AS[Disponible]," +
                        "\nISNULL(I.Reservado, 0) AS[Reservado]," +
                        "\nISNULL(I.Estatus,0) AS [Estatus]" +
                        "\nFROM Articulo AS A" +
                        "\nFULL OUTER JOIN Almacen AS AL ON 1 = 1" +
                        "\nLEFT OUTER JOIN Inventario AS I ON A.ID = I.ArticuloID AND AL.ID = I.AlmacenID" +
                        "\nINNER JOIN Sucursal AS S ON AL.SucursalID = S.ID" +
                        "\nWHERE" +
                        "\nA.Estatus = 1 AND" +
                        "\nS.Estatus = 1 AND" +
                        "\nAL.Estatus = 1" +
                        "\nORDER BY" +
                        "\nS.Nombre ASC,"+
                        "\nAL.Nombre ASC";

            foreach (DataRow row in _db.GetData(query, autoConnect: true).Rows)
                inventoryViewModel.Inventory.Add(new Inventory()
                {
                    Id = row["ID"] == DBNull.Value ? default : (Guid)row["ID"],
                    Warehouse = new Warehouse()
                    {
                        Id = (Guid)row["AlmacenID"],
                        Code = (string)row["AlmacenCodigo"],
                        Name = (string)row["AlmacenNombre"],
                    },
                    Stock = (int)row["Disponible"],
                    Reserved = (int)row["Reservado"],
                    Item = new Item()
                    {
                        Id = (Guid)row["ArticuloID"],
                        Code = (string)row["Referencia"],
                        Description = AddPossibleNullString(row, "Descripcion"),
                        Price = (decimal)row["Precio"],
                        TaxPercentaje = (int)row["PorcentajeImpuesto"]
                    },
                    Status = (bool)row["Estatus"]
                });

            return inventoryViewModel;
        }

        public bool EditInventory(Inventory inventory)
        {
            var query = $"DECLARE @ArticuloID AS UNIQUEIDENTIFIER = '{inventory.Item.Id}'" +
                         $"\nDECLARE @AlmacenID AS UNIQUEIDENTIFIER = '{inventory.Warehouse.Id}'"+
                         $"\nDECLARE @Disponible AS INT = {inventory.Stock}" +
                         $"\nDECLARE @Reservado AS INT = {inventory.Reserved}" +
                         "\nIF EXISTS(SELECT 1 FROM Inventario WHERE ArticuloID = @ArticuloID AND AlmacenID = @AlmacenID)" +
                         "\nBEGIN" +
                         "\n    UPDATE Inventario SET Disponible = @Disponible, Reservado = @Reservado WHERE ArticuloID = @ArticuloID AND AlmacenID = @AlmacenID" +
                         "\nEND" +
                         "\nELSE" +
                         "\nBEGIN" +
                         "\n    INSERT INTO Inventario(AlmacenID, Disponible, Reservado, ArticuloID)" +
                         "\n" +
                         "\n    VALUES(@AlmacenID, ISNULL(@Disponible, 0), ISNULL(@Reservado, 0), @ArticuloID)" +
                         "\nEND";

            if (_db.SetConnection() && _db.WriteData(query))
                return true;
            else
                return false;
        }

        public Item GetItemByCode(string code, bool justActive = false)
        {
            Item item = new Item();
            var query = $"SELECT * FROM Articulo WHERE Referencia = '{code}'";
            if (justActive)
                query += $" WHERE Estatus = 1";

            foreach (DataRow row in _db.GetData(query, autoConnect: true).Rows)
            {
                item.Id = (Guid)row["ID"];
                item.CreateDate = (DateTime)row["FechaRegistro"];
                item.Code = (string)row["Referencia"];
                item.Description = AddPossibleNullString(row, "Descripcion");
                item.Price = (decimal)row["Precio"];
                item.TaxPercentaje = (int)row["PorcentajeImpuesto"];
                item.Status = (bool)row["Estatus"];
            }
            return item;
        }

        public Warehouse GetWarehouseByCode(string code, bool justActive = false)
        {
            Warehouse warehouse = null;
            var query = $"SELECT * FROM Almacen WHERE Codigo = '{code}'";
            if (justActive)
                query += $" WHERE Estatus = 1";

            foreach (DataRow row in _db.GetData(query, autoConnect: true).Rows)
                warehouse = new Warehouse()
                {
                    Id = (Guid)row["ID"],
                    CreateDate = (DateTime)row["FechaRegistro"],
                    Store = GetStoreById((Guid)row["SucursalID"]),
                    Code = AddPossibleNullString(row, "Codigo"),
                    Name = AddPossibleNullString(row, "Nombre"),
                    Status = (bool)row["Estatus"],
                    Address = AddPossibleNullString(row, "Direccion")
                };

            return warehouse;
        }
    }
}
