using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ejercicio.Domain
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Nombre usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        public string Roles { get; set; }
    }
}
