using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AppMinimarket.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 100 caracteres.")]
        [Display(Name = "Marca")]
        public string usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(50, ErrorMessage = "El contraseña no puede tener más de 100 caracteres.")]
        [Display(Name = "Marca")]
        public string? contrasena { get; set; }
        public int idTipoUsu { get; set; }
    }
}
