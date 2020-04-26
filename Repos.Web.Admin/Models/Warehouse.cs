using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Web.Admin.Models
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        
        [Display(Name="Fecha de creación")]
        public DateTime CreateDate { get; set; }

        [Display(Name="Tienda")]
        public Store Store { get; set; }

        [Display(Name="Código de almacén")]
        public string Code { get; set; }

        [Display(Name="Nombre de almacén")]
        public string Name { get; set; }

        [Display(Name="Estatus")]
        public bool Status { get; set; }

        [Display(Name="Direccción")]
        public string Address { get; set; }
    }
}
