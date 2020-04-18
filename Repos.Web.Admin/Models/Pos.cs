using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Web.Admin.Models
{
    public class Pos
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public Warehouse Warehouse { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Hostname { get; set; }
        public string Ip { get; set; }
        public bool Status { get; set; }
    }
}
