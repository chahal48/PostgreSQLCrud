using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSQLCrudEntity
{
    public enum Category
    {
        [Display(Name = "Client")]
        Client,
        [Display(Name = "Vendor")]
        Vendor
    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
