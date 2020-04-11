using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Setup.Models
{
    public class Query
    {
        public Query()
        {
            Distributor = new Distributor();
            Publisher = new Publisher();
            SetQuery();
        }
        public Distributor Distributor { get; }
        public Publisher Publisher { get; }
        public string Schema { get; set;  }

        private void SetQuery()
        {
            Distributor.Add = GetResource("Distributor.Add.sql");
            Distributor.AddDb = GetResource("Distributor.AddDb.sql");
            Distributor.AddPublisher = GetResource("Distributor.AddPublisher.sql");
            
            Publisher.AddDistributor = GetResource("Publisher.AddDistributor.sql");
            Publisher.DatabaseReplOption = GetResource("Publisher.DatabaseReplOption.sql");
            Publisher.AddPublication = GetResource("Publisher.AddPublication.sql");
            Publisher.AddArticle = GetResource("Publisher.AddArticle.sql");
            Publisher.AddSubscription = GetResource("Publisher.AddSubscription.sql");
            Publisher.AddPushSubscriptionAgent = GetResource("Publisher.AddPushSubscriptionAgent.sql");
            Publisher.SetLogReaderAgent = GetResource("Publisher.SetLogReaderAgent.sql");

            Schema = GetResource("Schema.sql");
        }
        private string GetResource(string resource)
        {
            string content = null;
            Assembly assembly = Assembly.GetExecutingAssembly();
            try
            {
                resource = assembly.GetManifestResourceNames().Single(str => str.EndsWith(resource));
                Stream stream = assembly.GetManifestResourceStream(resource);
                StreamReader reader = new StreamReader(stream);
                content = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting local resource for query. {ex.Message} | {ex.StackTrace}");
            }
            return content;
        }
    }
    public class Distributor
    {
        public string Add { get; set; }
        public string AddDb { get; set; }
        public string AddPublisher { get; set; }
        public string Schema { get; set; }
    }
    public class Publisher
    {
        public string AddDistributor { get; set; }
        public string DatabaseReplOption { get; set; }
        public string AddPublication { get; set; }
        public string AddArticle { get; set; }
        public string AddSubscription { get; set; }
        public string AddPushSubscriptionAgent { get; set; }
        public string SetLogReaderAgent { get; set; }
        public string Schema { get; set; }
    }
}
