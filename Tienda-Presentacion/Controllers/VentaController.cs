using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tienda_Presentacion.Data;
using Tienda_Dominio._01_Clases;
using Tienda_Dominio._02_Enumeraciones;
using Tienda_Dominio._03_Interfaces;


namespace Tienda_Presentacion.Controllers
{
    public class VentaController : Controller
    {
        private readonly ILogger<VentaController> _logger;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IOptionsMonitor<Configuraciones> _configuraciones;
        private readonly IServCache _servCache;
        private readonly IServVenta _servVenta;
        public VentaController(
            ILogger<VentaController> logger,
            SignInManager<Usuario> signInManager,
            IOptionsMonitor<Configuraciones> configuraciones,
            IServCache servCache,
            IServVenta servVenta)
        {
            _logger = logger;
            _signInManager = signInManager;
            _configuraciones = configuraciones;
            _servCache = servCache;
            _servVenta = servVenta;
        }
        public IActionResult Venta()
        {
            if (_signInManager.IsSignedIn(User))
            {
                LimpiarCampos();
                return View("Venta");
            }
            else
            {
                return Redirect("Identity/Account/Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IniciarVenta()
        {
            var _user = _signInManager.UserManager.GetUserAsync(User).Result;
            try
            {
                _servVenta.IniciarVenta(_user.Id, _user.Empleado_Id);
            }
            catch (Exception ex)
            {
                ViewBag.Alerta = ex.Message;
            }
            CargarPagina();
            return View("Venta");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelarVenta()
        {
            LimpiarCampos();
            CargarPagina();
            return View("Venta");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MostrarFormCliente()
        {
            ViewBag.formCliente = true;
            CargarPagina();
            return View("Venta");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelarCambioCliente()
        {
            ViewBag.formCliente = false;
            CargarPagina();
            return View("Venta");
        }
        public IActionResult CambiarMetodo(string metodoId)
        {
            var _userId = _signInManager.UserManager.GetUserId(User);
            _servVenta.CambiarMetodoPago(long.Parse(metodoId), _userId);
            CargarPagina();
            return View("Venta");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SumarCantidad(long item)
        {
            var _userId = _signInManager.UserManager.GetUserId(User);
            try
            {
                _servVenta.SumarCantidad(item, _userId);
            }
            catch (Exception ex)
            {
                ViewBag.Alerta = ex.Message; 
            }
            var articulo = _servCache.RecuperarCache<Articulo>("Articulo", _userId);
            return BuscaProductos(articulo.Codigo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarLineaVenta(int item)
        {
            var _userId = _signInManager.UserManager.GetUserId(User);
            try
            {
                _servVenta.EliminarLineaVenta(item, _userId);
            }
            catch (Exception ex)
            {
                ViewBag.Alerta = ex.Message;
            }
            var articulo = _servCache.RecuperarCache<Articulo>("Articulo", _userId);
            return BuscaProductos(articulo.Codigo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RestarCantidad(int item)
        {
            var _userId = _signInManager.UserManager.GetUserId(User);
            try
            {
                _servVenta.RestarCantidad(item, _userId);
            }
            catch (Exception ex)
            {
                ViewBag.Alerta = ex.Message;
            }
            var articulo = _servCache.RecuperarCache<Articulo>("Articulo", _userId);
            return BuscaProductos(articulo.Codigo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarCliente(long doc)
        {
            var _userId = _signInManager.UserManager.GetUserId(User);
            try
            {
                _servVenta.CambiarCliente(doc, _userId);
            }
            catch (Exception ex)
            {
                ViewBag.formCliente = true;
                ViewBag.MensajeClienteError = ex.Message;
            }
            CargarPagina();
            return View("Venta");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BuscaProductos(long codigo)
        {
            var _userId = _signInManager.UserManager.GetUserId(User);
            try
            {
                _servVenta.BuscarProductos(codigo, _userId);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
            }
            CargarPagina();
            return View("Venta");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarProducto(long producto, long cantidad)
        {
            var _userId = _signInManager.UserManager.GetUserId(User);
            try
            {
                _servVenta.AgregarProducto(producto, cantidad, _userId);
            }
            catch (Exception ex)
            {
                ViewBag.Alerta = ex.Message;
            }
            CargarPagina();
            return View("Venta");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinalizarVenta(string numeroTarjeta = "", string nombreTitular = "", string dniTitular = "", string mesVencimiento = "", string anioVencimiento = "", string codSeguridad = "")
        {
            var _userId = _signInManager.UserManager.GetUserId(User);
            try
            {
                DatosTarjeta datosTarjeta = new DatosTarjeta(numeroTarjeta, nombreTitular, dniTitular, mesVencimiento, anioVencimiento, codSeguridad);
                _servVenta.FinalizarVenta(datosTarjeta, _userId);
            }
            catch(Exception ex)
            {
                ViewBag.Alerta = ex.Message;
            }
            CargarPagina();
            return View("Venta");
        }
        private void LimpiarCampos()
        {
            string userId = _signInManager.UserManager.GetUserId(User);
            _servCache.LimpiarCache(userId);
        }
        private void CargarPagina()
        {
            var _userId = _signInManager.UserManager.GetUserId(User);
            Venta venta = _servCache.RecuperarCache<Venta>("Venta", _userId);
            if (venta != null)
            {
                if (venta.Estado == (long)Estados.Aprobado || venta.Estado == (long)Estados.Aprobado_con_obs)
                {
                    ViewBag.Alerta = "VENTA NUM. " + venta.Id.ToString() + ": REGISTRADA Y APROBADA.";
                    LimpiarCampos();
                }
                else
                {
                    ViewBag.Cliente = $"{venta.Cliente.NumeroDocumento} - {venta.Cliente.Nombres} {venta.Cliente.Apellidos}";
                    ViewBag.Comprobante = venta.Comprobante.Nombre;
                    ViewBag.Venta = venta;
                    ViewBag.MontoTotal = venta.Monto;

                    List<Linea_Venta> lineasVentas = venta.ObtenerLineasdeVenta();
                    if (lineasVentas != null)
                    {
                        ViewBag.LineasVentas = lineasVentas;
                        ViewBag.CantidadTotal = lineasVentas.Sum(x => x.Cantidad);
                    }
                    else
                    {
                        ViewBag.CantidadTotal = 0;
                    }

                    Pago pago = _servCache.RecuperarCache<Pago>("Pago", _userId); ;
                    switch (pago.Metodo_Pago_Id)
                    {
                        case (int)MetPago.Efectivo:
                            ViewBag.Radio1 = "true";
                            break;
                        case (int)MetPago.Tarjeta:
                            ViewBag.Radio2 = "true";
                            break;
                        default:
                            ViewBag.Radio1 = "true";
                            break;
                    }

                    Articulo articulo = _servCache.RecuperarCache<Articulo>("Articulo", _userId);
                    if (articulo != null && articulo.Inventarios != null && articulo.Inventarios.Count > 0)
                    {
                        ViewBag.MostrarArticulo = true;
                        ViewBag.ArtCodigo = articulo.Codigo;
                        ViewBag.ArtDescripcion = articulo.Descripcion;
                        ViewBag.ArtTipoTalle = articulo.Tipo_Talle.Descipcion;
                        ViewBag.ArtPrecio = articulo.Costo * ((100 + articulo.IVA + articulo.Margen_Ganancia) / 100);
                        ViewBag.Inventarios = articulo.Inventarios.Where(inv => inv.Cantidad > 0).ToList();
                    }
                }
            }
        }
    }
}
