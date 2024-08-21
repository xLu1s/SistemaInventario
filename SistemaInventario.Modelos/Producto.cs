using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El número de serie es obligatorio")]
        [MaxLength(60)]
        public string NumeroSerie { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(60)]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "El coste es obligatorio")]
        public double Coste { get; set; }

        public string ImagenUrl { get; set; }
        [Required(ErrorMessage = "El estado es obligatorio")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
        [Required(ErrorMessage = "La marca es obligatoria")]
        public int MarcaId { get; set; }

        [ForeignKey("MarcaId")]
        public Marca Marca { get; set; }

        public int? PadreId { get; set; }

        public virtual Producto Padre { get; set; }

    }
}
