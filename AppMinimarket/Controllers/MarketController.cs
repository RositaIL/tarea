using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppMinimarket.Models;
using Newtonsoft.Json;


namespace AppMinimarket.Controllers
{
    public class MarketController : Controller
    {
        async Task<List<Categoria>> categorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                HttpResponseMessage mensaje = await clienteHttp.GetAsync("categorias");
                string resultado = await mensaje.Content.ReadAsStringAsync();

                categorias = JsonConvert.DeserializeObject<List<Categoria>>(resultado).Select(
                    c => new Categoria()
                    {
                        idCategoria = c.idCategoria,
                        nombreCate = c.nombreCate
                    }).ToList();
            }
            return categorias;
        }

        async Task<List<Marca>> marcas()
        {
            List<Marca> marcas = new List<Marca>();
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                HttpResponseMessage mensaje = await clienteHttp.GetAsync("marcas");
                string resultado = await mensaje.Content.ReadAsStringAsync();

                marcas = JsonConvert.DeserializeObject<List<Marca>>(resultado).Select(
                    m => new Marca()
                    {
                        idMarca = m.idMarca,
                        nombreMarca = m.nombreMarca
                    }).ToList();
            }
            return marcas;
        }

        async Task<List<Producto>> productos()
        {
            List<Producto> productos = new List<Producto>();
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                HttpResponseMessage mensaje = await clienteHttp.GetAsync("productos");
                string resultado = await mensaje.Content.ReadAsStringAsync();

                productos = JsonConvert.DeserializeObject<List<Producto>>(resultado).Select(
                    p => new Producto()
                    {
                        idProducto = p.idProducto,
                        nombreProd = p.nombreProd,
                        descripcion = p.descripcion,
                        precio = p.precio,
                        stock = p.stock,
                        idCategoria = p.idCategoria,
                        idMarca = p.idMarca,
                        nombreMarca = p.nombreMarca,
                        nombreCate = p.nombreCate
                    }).ToList();
            }
            return productos;
        }

        async Task<List<Producto>> busquedaProductos(string texto)
        {
            List<Producto> productos = new List<Producto>();
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                HttpResponseMessage mensaje = await clienteHttp.GetAsync($"buscartexto/{texto}");
                string resultado = await mensaje.Content.ReadAsStringAsync();

                productos = JsonConvert.DeserializeObject<List<Producto>>(resultado).Select(
                    p => new Producto()
                    {
                        idProducto = p.idProducto,
                        nombreProd = p.nombreProd,
                        descripcion = p.descripcion,
                        precio = p.precio,
                        stock = p.stock,
                        idCategoria = p.idCategoria,
                        idMarca = p.idMarca,
                        nombreMarca = p.nombreMarca,
                        nombreCate = p.nombreCate
                    }).ToList();
            }
            return productos;
        }

        void SesionUsuario()
        {
            if (HttpContext.Session.Get("Usuario") != null)
            {
                byte[] userBytes = HttpContext.Session.Get("Usuario");
                if (userBytes != null)
                {
                    string userJson = System.Text.Encoding.UTF8.GetString(userBytes);
                    Usuario usuario = JsonConvert.DeserializeObject<Usuario>(userJson);

                    HttpContext.Session.SetInt32("idUsu", usuario.idUsuario);
                    HttpContext.Session.SetString("nombreUsu", usuario.usuario);
                    HttpContext.Session.SetInt32("tipoUsu", usuario.idTipoUsu);
                }
            }
        }

        public IActionResult Index()
        {
            SesionUsuario();
            return View();
        }
        public async Task<IActionResult> Productos(string texto)
        {
            Console.Write(texto);
            if (!string.IsNullOrEmpty(texto))
            {
                return View(await busquedaProductos(texto));
            }
            else
            {
                return View(await productos());
            }
        }

        public async Task<IActionResult> Marcas()
        {
            return View(await marcas());
        }

        public async Task<IActionResult> AgregarProducto()
        {
            ViewBag.marcas = new SelectList(await marcas(), "idMarca", "nombreMarca");
            ViewBag.categorias = new SelectList(await categorias(), "idCategoria", "nombreCate");
            return View(await Task.Run(() => new Producto()));
        }

        [HttpPost]
        public async Task<IActionResult> AgregarProducto(Producto p)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.marcas = new SelectList(await marcas(), "idMarca", "nombreMarca");
                ViewBag.categorias = new SelectList(await categorias(), "idCategoria", "nombreCate");
                return View(p);
            }
            string mensaje = "";
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                StringContent contenido =
                    new StringContent(JsonConvert.SerializeObject(p), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage msg = await clienteHttp.PostAsync("agregarproducto", contenido);
                mensaje = await msg.Content.ReadAsStringAsync();
            }
            ViewBag.mensaje = mensaje;
            ViewBag.marcas = new SelectList(await marcas(), "idMarca", "nombreMarca");
            ViewBag.categorias = new SelectList(await categorias(), "idCategoria", "nombreCate");
            return View(await Task.Run(() => p));
        }

        public async Task<IActionResult> AgregarMarca()
        {
            return View(await Task.Run(() => new Marca()));
        }

        [HttpPost]
        public async Task<IActionResult> AgregarMarca(Marca m)
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            string mensaje = "";
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                StringContent contenido =
                    new StringContent(JsonConvert.SerializeObject(m), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage msg = await clienteHttp.PostAsync("agregarmarca", contenido);
                mensaje = await msg.Content.ReadAsStringAsync();
            }
            ViewBag.mensaje = mensaje;
            return View(await Task.Run(() => m));
        }

        public async Task<IActionResult> EditarProducto(int id)
        {
            Producto p = null;
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                HttpResponseMessage response = await clienteHttp.GetAsync($"buscarproducto/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    p = JsonConvert.DeserializeObject<Producto>(json);
                }
            }

            if (p == null)
            {
                return NotFound();
            }

            ViewBag.marcas = new SelectList(await marcas(), "idMarca", "nombreMarca");
            ViewBag.categorias = new SelectList(await categorias(), "idCategoria", "nombreCate");
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProducto(Producto p)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.marcas = new SelectList(await marcas(), "idMarca", "nombreMarca");
                ViewBag.categorias = new SelectList(await categorias(), "idCategoria", "nombreCate");
                return View(p);
            }

            string mensaje = "";
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                StringContent contenido =
                    new StringContent(JsonConvert.SerializeObject(p), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage msg = await clienteHttp.PutAsync("actualizarproducto", contenido);
                mensaje = await msg.Content.ReadAsStringAsync();
            }
            ViewBag.mensaje = mensaje;
            ViewBag.marcas = new SelectList(await marcas(), "idMarca", "nombreMarca");
            ViewBag.categorias = new SelectList(await categorias(), "idCategoria", "nombreCate");
            return View(p);
        }

        public async Task<IActionResult> EditarMarca(int id)
        {
            Marca m = null;
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                HttpResponseMessage response = await clienteHttp.GetAsync($"buscarmarca/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    m = JsonConvert.DeserializeObject<Marca>(json);
                }
            }

            if (m == null)
            {
                return NotFound();
            }

            return View(m);
        }

        [HttpPost]
        public async Task<IActionResult> EditarMarca(Marca m)
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }

            string mensaje = "";
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                StringContent contenido =
                    new StringContent(JsonConvert.SerializeObject(m), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage msg = await clienteHttp.PutAsync("actualizarmarca", contenido);
                mensaje = await msg.Content.ReadAsStringAsync();
            }
            ViewBag.mensaje = mensaje;
            return View(m);
        }

        public async Task<IActionResult> EliminarProducto(int id)
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                HttpResponseMessage response = await clienteHttp.DeleteAsync($"eliminarproducto/{id}");
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.mensaje = "Registro eliminado correctamente.";
                }
                else
                {
                    ViewBag.mensaje = "Error al eliminar el registro.";
                }
            }

            return RedirectToAction("Productos");
        }

        public async Task<IActionResult> EliminarMarca(int id)
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                HttpResponseMessage response = await clienteHttp.DeleteAsync($"eliminarmarca/{id}");
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.mensaje = "Registro eliminado correctamente.";
                }
                else
                {
                    ViewBag.mensaje = "Error al eliminar el registro.";
                }
            }

            return RedirectToAction("Marcas");
        }

        public async Task<IActionResult> Login()
        {
            return View(await Task.Run(() => new Usuario()));
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario u)
        {
            Usuario usuario;
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri("https://localhost:7244/api/api/");
                StringContent contenido =
                    new StringContent(JsonConvert.SerializeObject(u), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await clienteHttp.PostAsync("login", contenido);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    usuario = JsonConvert.DeserializeObject<Usuario>(responseContent);
                }
                else
                {
                    ViewBag.mensaje = "Credenciales incorrectas. Por favor, inténtalo de nuevo.";
                    return View("Login");
                }

                if (usuario.idUsuario != 0)
                {
                    HttpContext.Session.Set("Usuario", System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(usuario)));
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.mensaje = "Credenciales incorrectas. Por favor, inténtalo de nuevo.";
                    return View("Login");
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Usuario");
            return RedirectToAction("Index");
        }

    }
}
