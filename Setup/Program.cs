using Setup.Models;
using System;
using System.Collections.Generic;
using TextLogs;
using mssql.dbman;
using System.Data.SqlClient;
using System.Threading;

namespace Setup
{
    class Program
    {
        static void Main(string[] args)
        {
            Log log = new Log();
            MSSQLServer distributor = new MSSQLServer(log: log);
            MSSQLServer publisher = new MSSQLServer(log: log);
            distributor.DebugMode = true;
            publisher.DebugMode = true;
            string option = null;
            string dbname;

            Menu(ref option);
            Console.Clear();
            Console.WriteLine("\r\n");
            switch (option)
            {
                case "1":
                    Console.WriteLine("New Replication \r\n----------------\r\n");
                    Console.Write("Database: ");
                    dbname = Console.ReadLine();

                    //MSSQLServer db = new MSSQLServer(log: log);
                    //db.Server = "192.168.0.181,1451";
                    //db.Database = "master";
                    //db.User = "sa";
                    //db.Password = "Pwd12345!";
                    //db.DebugMode = true;
                    //CheckForDbExistence(db, dbname);

                    if (ConfigureDistributor(ref distributor, ref publisher))
                        if (ConfigurePublisher(ref publisher, ref distributor))
                        {
                            publisher.Database = dbname;
                            if (SetupDatabaseForReplication(publisher))
                                if (CreatePublication(publisher))
                                {
                                    string answer;
                                    do
                                    {
                                        Console.Write("Add article(s)? (yes/no) (y/n) ");
                                        answer = Console.ReadLine();

                                        if (answer.ToUpper() == "Y" || answer.ToUpper() == "YES")
                                            AddArticle(publisher);

                                    } while (answer.ToUpper() == "Y" || answer.ToUpper() == "YES");

                                    Console.WriteLine();
                                    SetupPublisherLogReaderAgent(publisher);
                                    Console.WriteLine();

                                    do
                                    {
                                        Console.Write("\r\nAdd subscription(s)? (yes/no) (y/n) ");
                                        answer = Console.ReadLine();

                                        if (answer.ToUpper() == "YES" || answer.ToUpper() == "Y")
                                            AddSubscription(publisher, distributor, dbname);

                                    } while (answer.ToUpper() == "YES" || answer.ToUpper() == "Y");
                                }
                        }
                    Console.WriteLine("Done. Press any key to exit this action...");
                    Console.Clear();
                    Menu(ref option);
                    break;
                case "2":
                    MSSQLServer subscriber = new MSSQLServer(log: log);
                    Console.Clear();
                    Console.WriteLine("Adding new subscription to publication...\r\n");
                    Console.Write("Database: ");
                    dbname = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(distributor.Server))
                    {
                        Console.Write("Distributor Ip address: ");
                        distributor.Server = Console.ReadLine();

                        Console.Write("Distributor port number (blank if default): ");
                        var distributorPort = Console.ReadLine();
                        distributor.Server = $"{distributor.Server},{(string.IsNullOrWhiteSpace(distributorPort) ? "1433" : distributorPort)}";

                        Console.Write("Distributor mssql user: ");
                        distributor.User = Console.ReadLine();

                        Console.Write("Distributor mssql password: ");
                        distributor.Password = Console.ReadLine();
                        
                        Console.WriteLine();
                    }
                    
                    if (string.IsNullOrWhiteSpace(publisher.Server))
                    {
                        Console.Write("Publisher Ip address: ");
                        publisher.Server = Console.ReadLine();

                        Console.Write("Publisher port number (blank if default): ");
                        var publisherPort = Console.ReadLine();
                        publisher.Server = $"{publisher.Server},{(string.IsNullOrWhiteSpace(publisherPort) ? "1433" : publisherPort)}";

                        Console.Write("Publisher mssql user: ");
                        publisher.User = Console.ReadLine();

                        Console.Write("Publisher mssql password: ");
                        publisher.Password = Console.ReadLine();

                        Console.WriteLine();
                    }

                    publisher.Database = dbname;
                    distributor.Database = "distribution";
                    AddSubscription(publisher, distributor, dbname);
                    Console.WriteLine("\r\nSubcription added succesfully. Prease any key to exit this action");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(ref option);
                    break;
                case "9":
                    Console.WriteLine("Quitting console...");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                    break;
                default:
                    Print($"Selected option is not on the list ({option})...", log);
                    break;
            }
        }
        static void Menu(ref string option)
        {
            Console.WriteLine("SQL Server Transactional Replication Setup Console \r\n");
            Console.WriteLine("Actions:");
            Console.WriteLine("--------");
            Console.WriteLine(" 1 - Setup initial replication schema");
            Console.WriteLine(" 2 - Add subscription to publication");
            Console.WriteLine(" 9 - Exit");

            Console.Write("\r\nChoose an action: ");
            option = Console.ReadLine();
        }
        static void Print(string message, Log log, bool isError = false)
        {
            try
            {
                log.Write(message, isError);
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not write trace to {log.filename}. {ex.Message} | {ex.StackTrace}");
            }
        }
        static bool ConfigureDistributor(ref MSSQLServer distributor, ref MSSQLServer publisher)
        {
            Query query = new Query();
            bool success = true;
            try
            {
                Console.Write("Distributor Ip address: ");
                distributor.Server = Console.ReadLine();

                Console.Write("Distributor Port number (blank if default): ");
                var distributorPort = Console.ReadLine();
                distributor.Server = $"{distributor.Server},{(string.IsNullOrWhiteSpace(distributorPort) ? "1433" : distributorPort)}";

                Console.Write("Distributor mssql User: ");
                distributor.User = Console.ReadLine();

                Console.Write("Distributor mssql Password: ");
                distributor.Password = Console.ReadLine();

                Console.Write("\r\nPublisher Ip address: ");
                publisher.Server = Console.ReadLine();

                Console.Write("Publisher Port number (blank if default): ");
                var publisherPort = Console.ReadLine();
                publisher.Server = $"{publisher.Server},{(string.IsNullOrWhiteSpace(publisherPort) ? "1433" : publisherPort)}";

                Console.Write("Publisher mssql User: ");
                publisher.User = Console.ReadLine();

                Console.Write("Publisher mssql Password: ");
                publisher.Password = Console.ReadLine();

                distributor.Database = "master";
                publisher.Database = "master";

                if (distributor.SetConnection())
                {
                    Console.WriteLine($"\r\nSuccessfully connected to distributor engine on {distributor.Server} with {distributor.User}");

                    //Add Distributor
                    Console.Write("Setting up distributor... ");
                    if (!distributor.WriteData(query.Distributor.Add.Replace("$Password$", distributor.Password)))
                        success = false;
                    Console.WriteLine("Done");

                    //Create Distribution Database
                    Console.Write("Creating distribution database... ");
                    distributor.Database = "distribution";
                    if (!distributor.WriteData(query.Distributor.AddDb.Replace("$DbName$", distributor.Database)))
                        success = false;
                    Console.WriteLine("Done");

                    if (!AddDistributorPublisher(distributor, publisher))
                        success = false;
                }
                else
                {
                    success = false;
                    Console.WriteLine($"(ERROR) Could not connect to distributor engine on {distributor.Server} with {distributor.User}");
                }

                if (success)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        static bool ConfigurePublisher(ref MSSQLServer publisher, ref MSSQLServer distributor)
        {
            bool success = true;
            bool alreadyConfigured = false;
            Query query = new Query();
            try
            {
                Console.Write("Setting up publisher... ");

                if (publisher.Server == distributor.Server)
                {
                    Console.WriteLine("Distributor and Publisher are the same server. Skipping this step... ");
                    alreadyConfigured = true;
                }

                if (!alreadyConfigured)
                {
                    var distributorHostName = distributor.GetData("SELECT @@SERVERNAME").Rows[0][0].ToString();
                    if (!publisher.WriteData(query.Publisher.AddDistributor
                        .Replace("$DistributorHostName$", distributorHostName)
                        .Replace("$Password$", publisher.Password)))
                        success = false;
                    else
                        Console.WriteLine("Done");
                }
                if (success) return true; else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        static bool SetupDatabaseForReplication(MSSQLServer publisher)
        {
            Query query = new Query();
            bool success = true;
            try
            {
                //CheckForDbExistence(publisher, publisher.Database);

                Console.Write("\r\nSetting up database for replication... ");
                if (!publisher.WriteData(query.Publisher.DatabaseReplOption.Replace("$Database$", publisher.Database)))
                    success = false;
                else
                    Console.WriteLine("Done");

                if (success) return true; else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up database for replication. {ex.Message} | {ex.StackTrace}");
                return false;
            }
        }
        
        static bool CreatePublication(MSSQLServer publisher)
        {
            Query query = new Query();
            bool success = true;
            try
            {
                if (!publisher.SetConnection())
                    return false;

                Console.Write("\r\nCreating publication... ");
                if (!publisher.WriteData(query.Publisher.AddPublication.Replace("$Database$", publisher.Database)))
                    success = false;
                Console.WriteLine("Done");

                if (success) return true; else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        static bool AddArticle(MSSQLServer publisher)
        {
            Query query = new Query();
            bool success = true;
            try
            {
                Console.Write("Article name? ");
                var article = Console.ReadLine();

                if (publisher.GetData($"SELECT * FROM sys.tables WHERE name = NULLIF('{article}','')").Rows.Count > 0)
                {
                    Console.Write($"Article existence validated... Adding it to this database publication (Db: {publisher.Database})... ");
                    if (publisher.WriteData(query.Publisher.AddArticle.Replace("$article$", article).Replace("$database$", publisher.Database)))
                    {
                        Console.WriteLine("Done");
                    }
                    else
                        success = false;
                }

                if (success) return true; else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        static bool AddSubscription(MSSQLServer publisher, MSSQLServer distributor, string dbname)
        {
            bool success = true;
            MSSQLServer subscriber  = new MSSQLServer();
            Query query = new Query();
            subscriber.DebugMode = true;
            try
            {
                Console.Write("Subscriber Ip address: ");
                subscriber.Server = Console.ReadLine();

                Console.Write("Subscriber Port number (blank if default): ");
                var subscriberPort = Console.ReadLine();
                subscriber.Server = $"{subscriber.Server},{(string.IsNullOrWhiteSpace(subscriberPort) ? "1433" : subscriberPort)}";

                Console.Write("Subscriber mssql User: ");
                subscriber.User = Console.ReadLine();

                Console.Write("Subscriber mssql Password: ");
                subscriber.Password = Console.ReadLine();

                if (subscriber.SetConnection())
                {
                    Console.Write("Adding new subscription... ");
                    var subscriberHostName = subscriber.GetData("SELECT @@SERVERNAME").Rows[0][0].ToString();

                    publisher.SetConnection();
                    if (!publisher.WriteData(query.Publisher.AddSubscription.Replace("$database$", publisher.Database).Replace("$hostname$", subscriberHostName)))
                        success = false;
                    else
                        Console.WriteLine("Done");

                    Console.Write("Adding it's push subscription agent... ");
                    if (!publisher.WriteData(query.Publisher.AddPushSubscriptionAgent
                        .Replace("$database$", publisher.Database)
                        .Replace("$subscriberHostname$", subscriberHostName)
                        .Replace("$user$", subscriber.User)
                        .Replace("$password$", subscriber.Password)))
                        success = false;
                    else
                        Console.WriteLine("Done");

                    Console.Write("\r\nSetup birectional replication for this subscriber? (yes/no) (y/n) ");
                    var answer = Console.ReadLine();

                    if (answer.ToUpper() == "YES" || answer.ToUpper() == "Y")
                    {
                        if (AddDistributorPublisher(distributor, subscriber))
                            if (ConfigurePublisher(ref subscriber, ref distributor))
                            {
                                subscriber.Database = dbname;
                                if (SetupDatabaseForReplication(subscriber))
                                    if (CreatePublication(subscriber))
                                    {
                                        do
                                        {
                                            Console.Write("Add article(s) (yes/no) (y/n) ");
                                            answer = Console.ReadLine();

                                            if (answer.ToUpper() == "YES" || answer.ToUpper() == "Y")
                                                AddArticle(subscriber);

                                        } while (answer.ToUpper() == "YES" || answer.ToUpper() == "Y");

                                        Console.Write("Adding new subscription for this subscriber... ");
                                        var publisherHostName = publisher.GetData("SELECT @@SERVERNAME").Rows[0][0].ToString();

                                        if (!subscriber.WriteData(query.Publisher.AddSubscription
                                            .Replace("$database$", subscriber.Database)
                                            .Replace("$hostname$", publisherHostName)))
                                            success = false;
                                        else
                                            Console.WriteLine("Done");

                                        Console.Write("Adding it's push subscription agent... ");
                                        if (!subscriber.WriteData(query.Publisher.AddPushSubscriptionAgent
                                            .Replace("$database$", subscriber.Database)
                                            .Replace("$subscriberHostname$", publisherHostName)
                                            .Replace("$user$", publisher.User)
                                            .Replace("$password$", publisher.Password)))
                                            success = false;
                                        else
                                            Console.WriteLine("Done");

                                        if (success)
                                            SetupPublisherLogReaderAgent(subscriber);
                                    }
                            }
                    }
                }
                else
                {
                    success = false;
                    Console.WriteLine("(ERROR) Could not connect to subscriber. Verify credentials");
                }

                if (success) return true; return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        static bool SetupPublisherLogReaderAgent(MSSQLServer publisher)
        {
            Query query = new Query();
            bool success = true;
            try
            {
                Console.Write("Setting up log reader agent security context... ");
                publisher.SetConnection();
                if (!publisher.WriteData(query.Publisher.SetLogReaderAgent
                    .Replace("$user$", publisher.User)
                    .Replace("$password$", publisher.Password)))
                    success = false;
                else
                    Console.WriteLine("Done");

                if (success) return true; else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        static bool AddDistributorPublisher(MSSQLServer distributor, MSSQLServer publisher)
        {
            bool success = true;
            Query query = new Query();
            try
            {
                //Add publisher to distributor
                Console.Write("Adding publisher on distributor... ");
                if (publisher.SetConnection())
                {
                    var publisherHostname = publisher.GetData("SELECT @@SERVERNAME").Rows[0][0].ToString();
                    distributor.SetConnection();
                    var sql = query.Distributor.AddPublisher.Replace("$HostName$", publisherHostname).Replace("$DistributionDbName$", distributor.Database);
                    if (!distributor.WriteData(sql))
                        success = false;
                    else
                        Console.WriteLine("Done");
                }
                else
                {
                    success = false;
                    Console.WriteLine($"(ERROR) Could not connect to publisher");
                }

                if (success) return true; else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR - Could not add publisher to distributor. {ex.Message} | {ex.StackTrace}");
                return false;
            }
        }

        static bool CheckForDbExistence(MSSQLServer dbserver, string dbname)
        {
            Query query = new Query();
            try
            {
                bool success = true;
                bool exists = false;
                dbserver.Database = "master";

                if (dbserver.SetConnection())
                    exists = (dbserver.GetData($"SELECT * FROM sys.databases WHERE name = '{dbname}'").Rows.Count > 0) ? true : false;

                if (!exists)
                {
                    Console.Write($"\r\nDatabase {dbname} does not exist on {dbserver.Server}. Auto-create with schema script? (yes/no) (y/n):  ");
                    var answer = Console.ReadLine();

                    if (answer.ToUpper() == "YES" || answer.ToUpper() == "Y")
                    {
                        Console.Write("Creating database... ");
                        dbserver.WriteData($"CREATE DATABASE {dbname}");
                        Console.WriteLine("Done");

                        dbserver.Database = dbname;
                        Console.WriteLine("Executing schema script... ");

                        //var sql = query.Schema.Replace("$DbName$", dbname);
                        if (!dbserver.WriteData(query.Schema))
                        {
                            success = false;
                            Console.WriteLine("(ERROR) Could not complete schama script execution. Please verify.");
                        }
                        else
                            Console.WriteLine("Done");
                    }
                    else
                        Console.WriteLine("Ignoring auto-create... Make sure the schema exists on this server.");
                }

                return success;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}