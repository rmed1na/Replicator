using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Web.Admin.Models
{
    public class Store
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public Company Company { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
    }
}
