using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Web.Admin.Models
{
    public class Item
    {
        public Guid Id { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Código de artículo")]
        public string Code { get; set; }

        [Display(Name ="Descripción")]
        public string Description { get; set; }

        [Display(Name ="Precio")]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal Price { get; set; }

        [Display(Name ="Porcentaje de impuesto")]
        public int TaxPercentaje { get; set; }

        [Display(Name ="Estatus")]
        public bool Status { get; set; }
    }
}
