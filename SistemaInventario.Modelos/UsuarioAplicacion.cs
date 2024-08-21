using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(80)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Es obligatorio introducir los apellidos")]
        [MaxLength(80)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Introduzca una dirección")]
        [MaxLength(200)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Introduzca una ciudad")]
        [MaxLength(60)]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "Introduzca un pais")]
        [MaxLength(60)]
        public string Pais { get; set; }

        [NotMapped] // Para que no se agregue a una columna en la base de datos
        public string Role { get; set; }
    }
}
