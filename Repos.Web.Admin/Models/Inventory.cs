using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Web.Admin.Models
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public Warehouse Warehouse { get; set; }
        public int Stock { get; set; }
        public int Reserved { get; set; }
        public Item Item { get; set; }
        public bool Status { get; set; }
    }
}
