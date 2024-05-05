using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AppMinimarket.Models
{
    public class Marca
    {
        [Display(Name = "Id")]
        public int idMarca { get; set; }

        [Required(ErrorMessage = "El nombre de marca es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        [Display(Name = "Marca")]
        public string nombreMarca { get; set; }
    }
}
