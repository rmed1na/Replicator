using Setup.Models;
using System;
using System.Collections.Generic;
using TextLogs;
using mssql.dbman;
using System.Data.SqlClient;

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

            Menu(ref option);
            Console.Clear();
            Console.WriteLine("\r\n");
            switch (option)
            {
                case "1":
                    Console.WriteLine("New Replication \r\n ----------------");
                    if (ConfigureDistributor(ref distributor, ref publisher))
                        if (ConfigurePublisher(ref publisher, ref distributor))
                        {
                            publisher.Database = "UTP";
                            if (SetupDatabaseForReplication(publisher))
                                if (CreatePublication(publisher))
                                {
                                    do
                                    {
                                        Console.WriteLine("Add article(s)?");
                                        AddArticle(publisher);
                                    } while (Console.ReadLine() == "Y" || Console.ReadLine() == "y" || Console.ReadLine().ToUpper() == "YES");
                                }
                        }
                            
                    break;
                default:
                    Print($"Selected option is not on the list ({option})", log);
                    break;
            }
        }
        static void Menu(ref string option)
        {
            Console.WriteLine("SQL Server Setup Console \r\n");
            Console.WriteLine("Actions:");
            Console.WriteLine("--------");
            Console.WriteLine(" 1 - Setup initial replication schema");

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

                    //Add publisher to distributor
                    Console.Write("Adding publisher on distributor... ");
                    if (publisher.SetConnection())
                    {
                        var publisherHostname = publisher.GetData("SELECT @@SERVERNAME").Rows[0][0].ToString();
                        var sql = query.Distributor.AddPublisher.Replace("$HostName$", publisherHostname).Replace("$DistributionDbName$", distributor.Database);
                        if (!distributor.WriteData(sql))
                            success = false;
                        Console.WriteLine("Done");
                    }
                    else
                    {
                        success = false;
                        Console.WriteLine($"(ERROR) Could not connect to publisher");
                    }
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
            Query query = new Query();
            try
            {
                Console.Write("Setting up publisher... ");
                var distributorHostName = distributor.GetData("SELECT @@SERVERNAME").Rows[0][0].ToString();
                if (publisher.WriteData(query.Publisher.AddDistributor
                    .Replace("$DistributorHostName$", distributorHostName)
                    .Replace("$Password$", publisher.Password)))
                {
                    Console.WriteLine("Done");
                    return true;
                }
                else
                    return false;
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
                Console.Write("\r\nSetting up database for replication... ");
                if (!publisher.WriteData(query.Publisher.DatabaseReplOption.Replace("$Database$", publisher.Database)))
                    success = false;
                Console.WriteLine("Done");

                if (success) return true;
                else return false;
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
            try
            {
                Console.Write("Article name?");
                var article = Console.ReadLine();

                if (publisher.GetData($"SELECT * FROM sys.tables WHERE name = NULLIF('{article}','')").Rows.Count > 0)
                {
                    Console.Write($"Article existence validated... Adding it to this database publication (Db: {publisher.Database})");

                    Console.WriteLine("Done");
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
