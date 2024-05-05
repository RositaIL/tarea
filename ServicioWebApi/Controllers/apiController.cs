using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ServicioWebApi.Models;
using System.Data;

namespace ServicioWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class apiController : ControllerBase
    {
        public readonly IConfiguration config;

        public apiController(IConfiguration ic)
        {
            config = ic;
        }

        //Metodos de lectura/escritura de la base de datos:
        IEnumerable<Categoria> categorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            using (SqlConnection cn = new SqlConnection(config["ConnectionStrings:cn"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("execute listarCategorias", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    categorias.Add(new Categoria()
                    {
                        idCategoria = dr.GetInt32(0),
                        nombreCate = dr.GetString(1),
                    });
                }
            }
            return categorias;
        }

        IEnumerable<Marca> marcas()
        {
            List<Marca> marcas = new List<Marca>();
            using (SqlConnection cn = new SqlConnection(config["ConnectionStrings:cn"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("execute listarMarcas", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    marcas.Add(new Marca()
                    {
                        idMarca = dr.GetInt32(0),
                        nombreMarca = dr.GetString(1),
                    });
                }
            }
            return marcas;
        }

        IEnumerable<Producto> productos()
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(config["ConnectionStrings:cn"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("execute listarProductos", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    productos.Add(new Producto()
                    {
                        idProducto = dr.GetInt32(0),
                        nombreProd = dr.GetString(1),
                        descripcion = dr.GetString(2),
                        precio = dr.GetDecimal(3),
                        stock = dr.GetInt32(4),
                        idCategoria = dr.GetInt32(5),
                        idMarca = dr.GetInt32(6),
                        nombreCate = dr.GetString(7),
                        nombreMarca = dr.GetString(8)
                    });
                }
            }
            return productos;
        }

        IEnumerable<Producto> buscartexto(string texto)
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(config["ConnectionStrings:cn"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("buscarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@texto", texto);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    productos.Add(new Producto()
                    {
                        idProducto = dr.GetInt32(0),
                        nombreProd = dr.GetString(1),
                        descripcion = dr.GetString(2),
                        precio = dr.GetDecimal(3),
                        stock = dr.GetInt32(4),
                        idCategoria = dr.GetInt32(5),
                        idMarca = dr.GetInt32(6)
                    });
                }
            }
            return productos;
        }

        async Task<Producto> buscarproducto(int id)
        {
            return productos().FirstOrDefault(p => p.idProducto == id);
        }

        async Task<Marca> buscarmarca(int id)
        {
            return marcas().FirstOrDefault(m => m.idMarca == id);
        }

        string agregarMarca(Marca m)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(config["ConnectionStrings:cn"]))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("agregarMarca", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", m.nombreMarca);
                    cmd.ExecuteNonQuery();
                    mensaje = $"Se ha agregado la marca {m.nombreMarca}";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        string agregarProducto(Producto p)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(config["ConnectionStrings:cn"]))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("agregarProducto", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", p.nombreProd);
                    cmd.Parameters.AddWithValue("@descripcion", p.descripcion);
                    cmd.Parameters.AddWithValue("@precio", p.precio);
                    cmd.Parameters.AddWithValue("@stock", p.stock);
                    cmd.Parameters.AddWithValue("@idcate", p.idCategoria);
                    cmd.Parameters.AddWithValue("@idmarca", p.idMarca);
                    cmd.ExecuteNonQuery();
                    mensaje = $"Se ha agregado {p.nombreProd}";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        string modificarMarca(Marca m)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(config["ConnectionStrings:cn"]))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("modificarMarca", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", m.idMarca);
                    cmd.Parameters.AddWithValue("@nombre", m.nombreMarca);
                    cmd.ExecuteNonQuery();
                    mensaje = $"Se ha modificado la marca {m.nombreMarca}";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        string modificarProducto(Producto p)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(config["ConnectionStrings:cn"]))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("modificarProducto", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", p.idProducto);
                    cmd.Parameters.AddWithValue("@nombre", p.nombreProd);
                    cmd.Parameters.AddWithValue("@descripcion", p.descripcion);
                    cmd.Parameters.AddWithValue("@precio", p.precio);
                    cmd.Parameters.AddWithValue("@stock", p.stock);
                    cmd.Parameters.AddWithValue("@idcate", p.idCategoria);
                    cmd.Parameters.AddWithValue("@idmarca", p.idMarca);
                    cmd.ExecuteNonQuery();
                    mensaje = $"Se ha modificado {p.nombreProd}";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        string eliminarMarca(int id)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(config["ConnectionStrings:cn"]))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("eliminarMarca", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    mensaje = $"Se ha eliminado la marca";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        string eliminarProducto(int id)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(config["ConnectionStrings:cn"]))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("eliminarProducto", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    mensaje = $"producto eliminado";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        //Metodo de lectura del login desde la base de datos
        async Task<Usuario> login(string usu, string contra)
        {
            Usuario usuario = new Usuario();
            using (SqlConnection cn = new SqlConnection(config["ConnectionStrings:cn"]))
            {
                await cn.OpenAsync();
                try
                {
                    SqlCommand cmd = new SqlCommand("login", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usu", usu);
                    cmd.Parameters.AddWithValue("@contra", contra);
                    SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    if (await dr.ReadAsync())
                    {
                        usuario.idUsuario = dr.GetInt32(0);
                        usuario.usuario = dr.GetString(1);
                        usuario.idTipoUsu = dr.GetInt32(2);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en el método login: {ex.Message}");
                }
                finally
                {
                    await cn.CloseAsync();
                }
            }
            return usuario;
        }

        //Métodos de comunicación (Verbos) con el front:
        [HttpGet("marcas")]
        public async Task<ActionResult<IEnumerable<Marca>>> Listamarcas()
        {
            return Ok(await Task.Run(() => marcas()));
        }
        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<Categoria>>> ListaCategorias()
        {
            return Ok(await Task.Run(() => categorias()));
        }
        [HttpGet("productos")]
        public async Task<ActionResult<IEnumerable<Producto>>> ListaProductos()
        {
            return Ok(await Task.Run(() => productos()));
        }
        [HttpGet("buscartexto/{texto}")]
        public async Task<ActionResult<IEnumerable<Producto>>> BusquedaTexto(string texto)
        {
            return Ok(await Task.Run(() => buscartexto(texto)));
        }
        [HttpGet("buscarproducto/{id}")]
        public async Task<ActionResult<Producto>> BusquedaProducto(int id)
        {
            return Ok(await Task.Run(() => buscarproducto(id)));
        }
        [HttpGet("buscarmarca/{id}")]
        public async Task<ActionResult<Producto>> BusquedaMarca(int id)
        {
            return Ok(await Task.Run(() => buscarmarca(id)));
        }
        [HttpPost("agregarmarca")]
        public async Task<ActionResult<string>> AgregarMarca(Marca m)
        {
            return Ok(await Task.Run(() => agregarMarca(m)));
        }
        [HttpPost("agregarproducto")]
        public async Task<ActionResult<string>> AgregarProducto(Producto p)
        {
            return Ok(await Task.Run(() => agregarProducto(p)));
        }
        [HttpPut("actualizarmarca")]
        public async Task<ActionResult<string>> ActualizarMarca(Marca m)
        {
            return Ok(await Task.Run(() => modificarMarca(m)));
        }
        [HttpPut("actualizarproducto")]
        public async Task<ActionResult<string>> ActualizarProducto(Producto p)
        {
            return Ok(await Task.Run(() => modificarProducto(p)));
        }
        [HttpDelete("eliminarmarca/{id}")]
        public async Task<ActionResult<string>> EliminarMarca(int id)
        {
            return Ok(await Task.Run(() => eliminarMarca(id)));
        }
        [HttpDelete("eliminarproducto/{id}")]
        public async Task<ActionResult<string>> EliminarProducto(int id)
        {
            return Ok(await Task.Run(() => eliminarProducto(id)));
        }

        //Metodo de comunicación,conexión hacia el front
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(Usuario u)
        {
            Usuario usuario = await login(u.usuario, u.contrasena);

            if (usuario.idUsuario != 0)
            {
                return Ok(usuario);
            }
            else
            {
                return BadRequest("Credenciales incorrectas");
            }
        }
    }
}
