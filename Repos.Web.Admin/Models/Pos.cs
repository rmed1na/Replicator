using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Web.Admin.Models
{
    public class Pos
    {
        public Guid Id { get; set; }
        
        [Display(Name="Fecha de creación")]
        public DateTime CreateDate { get; set; }
        
        [Display(Name="Almacén")]
        public Warehouse Warehouse { get; set; }
        
        [Display(Name="Código de caja")]
        public string Code { get; set; }
        
        [Display(Name="Nombre")]
        public string Name { get; set; }
        
        public string Hostname { get; set; }
        
        public string Ip { get; set; }
        
        [Display(Name="Estatus")]
        public bool Status { get; set; }
    }
}
