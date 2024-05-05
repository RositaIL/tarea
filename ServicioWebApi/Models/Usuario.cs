namespace ServicioWebApi.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string usuario { get; set; }
        public string? contrasena { get; set; }
        public int idTipoUsu { get; set; }
    }
}
