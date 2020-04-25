using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Web.Admin.Models
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public Warehouse Warehouse { get; set; }
        
        [Display(Name ="Disponible")]
        public int Stock { get; set; }

        [Display(Name ="Reservado")]
        public int Reserved { get; set; }

        public Item Item { get; set; }

        [Display(Name ="Estatus")]
        public bool Status { get; set; }
    }
}
