namespace ServicioWebApi.Models
{
    public class Producto
    {
        public int idProducto { get; set; }
        public string nombreProd { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public int idCategoria { get; set; }
        public int idMarca { get; set; }
        public string? nombreCate { get; set; }
        public string? nombreMarca { get; set; }
    }
}
