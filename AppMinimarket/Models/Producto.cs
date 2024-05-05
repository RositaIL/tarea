using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AppMinimarket.Models
{
    public class Producto
    {
        [Display(Name = "Id")]
        public int idProducto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        [Display(Name = "Producto")]
        public string nombreProd { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(50, ErrorMessage = "La descripción no puede tener más de 250 caracteres.")]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, 10000, ErrorMessage = "El precio debe ser entre 0 y 10,000.")]
        [Display(Name = "Precio")]
        public decimal precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, 1000, ErrorMessage = "El stock debe estar entre 0 y 1,000.")]
        [Display(Name = "Stock")]
        public int stock { get; set; }

        [Display(Name = "Categoría")]
        public int idCategoria { get; set; }

        [Display(Name = "Marca")]
        public int idMarca { get; set; }

        [Display(Name = "Categoría")]
        public string? nombreCate { get; set; }

        [Display(Name = "Marca")]
        public string? nombreMarca { get; set; }
    }
}
