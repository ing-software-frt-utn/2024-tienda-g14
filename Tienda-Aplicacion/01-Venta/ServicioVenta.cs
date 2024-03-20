using Microsoft.Extensions.Options;
using Tienda_Dominio._01_Clases;
using Tienda_Dominio._02_Enumeraciones;
using Tienda_Dominio._03_Interfaces;

namespace Tienda_Aplicacion._01_Venta
{
    public class ServicioVenta : IServVenta
    {
        private IServDatos _servDatos;
        private IServCache _servCache;
        private IServAFIP _servAfip;
        private IServTarjeta _servTarjeta;
        private readonly IOptions<Configuraciones> _configuraciones;
        public ServicioVenta(IServDatos servDatos, IServCache servCache, IServAFIP servAfip, IServTarjeta servTarjeta, IOptions<Configuraciones> configuraciones)
        {
            _servDatos = servDatos;
            _servCache = servCache;
            _servAfip = servAfip;
            _servTarjeta = servTarjeta;
            _configuraciones = configuraciones;
        }
        public Venta RecuperarVenta(string usuarioId)
        {
            try
            {
                return _servCache.RecuperarCache<Venta>("Venta", usuarioId);
            }
            catch
            {
                return default;
            }
        }
        public Articulo RecuperarArticulo(string usuarioId)
        {
            try
            {
                return _servCache.RecuperarCache<Articulo>("Articulo", usuarioId);
            }
            catch
            {
                return default;
            }
        }
        public void IniciarVenta(string usuarioId, long empleadoId)
        {
            try
            {
                Cliente cliente = _servDatos.obtenerClientexDoc(0);
                Sucursal sucursal = _servDatos.obtenerSucursal(empleadoId);
                Comprobante comprobante = _servDatos.obtenerComprobante(sucursal.Condicion_Tributaria_Id, cliente.Condicion_Tributaria_Id);

                Venta venta = new Venta(cliente, usuarioId, sucursal, comprobante);
                _servCache.GuardarCache("Venta", venta, usuarioId);

                Pago pago = new Pago((int)MetPago.Efectivo);
                _servCache.GuardarCache("Pago", pago, usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al iniciar una venta: {ex.Message}");
            }
        }
        public void CambiarCliente(long doc, string usuarioId)
        {
            Venta venta = _servCache.RecuperarCache<Venta>("Venta", usuarioId);
            if (venta != null)
            {
                Cliente cliente = _servDatos.obtenerClientexDoc(doc);
                if (cliente != null)
                {
                    Comprobante comprobante = _servDatos.obtenerComprobante(venta.Sucursal.Condicion_Tributaria_Id, cliente.Condicion_Tributaria_Id);
                    venta.CambiarCliente(cliente, comprobante);

                    _servCache.GuardarCache("Venta", venta, usuarioId);
                }
                else
                {
                    throw new Exception("No se encuentra un cliente con el documento ingresado.");
                }
            }
            else
            {
                throw new Exception("Debe tener una venta iniciada.");
            }
        }
        public void BuscarProductos(long codigo, string usuarioId)
        {
            Venta venta = _servCache.RecuperarCache<Venta>("Venta", usuarioId);

            if (venta != null)
            {
                Articulo art = _servDatos.obtenerArticulo(codigo); // BUSCO EL ARTICULO QUE TENGA EL CODIGO SOLICITADO. LA BUSQUEDA TRAE EL INVENTARIO COMPLETO DEL ARTICULO
                if (art != null)
                {
                    art.ActualizarInventario(venta);
                    _servCache.GuardarCache("Articulo", art, usuarioId);
                }
                else
                {
                    throw new Exception("No existe articulos con ese código.");
                }
            }
            else
            {
                throw new Exception("Venta no iniciada");
            }
        }
        public void AgregarProducto(long productoId, long cantidad, string usuarioId)
        {
            try
            { 
                Articulo articulo = _servCache.RecuperarCache<Articulo>("Articulo", usuarioId);
                Venta venta = _servCache.RecuperarCache<Venta>("Venta", usuarioId);

                List<Linea_Venta> lineasVentas = venta.Lineas_Ventas;
                var inv = articulo.ObtenerInventario(productoId);
                if (cantidad <= inv.Cantidad)
                {
                    venta.AgregarLineaVenta(inv, cantidad);
                    _servCache.GuardarCache("Venta", venta, usuarioId);
                }
                else
                {
                    throw new Exception("No hay suficiente inventario de este producto en la sucursal.");
                }
                BuscarProductos(articulo.Codigo, usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void CambiarMetodoPago(long metodoId, string usuarioId)
        {
            Pago pago = _servCache.RecuperarCache<Pago>("Pago", usuarioId);
            if (pago != null)
            {
                pago.CambiarMetodoPago(metodoId);
                _servCache.GuardarCache("Pago", pago, usuarioId);
            }
            else
            {
                throw new Exception("DEBE INICIAR UNA VENTA");
            }
        }
        public void SumarCantidad(long item, string usuarioId)
        {
            try
            {
                Venta venta = _servCache.RecuperarCache<Venta>("Venta", usuarioId);
                venta.SumarCantidad(item);
                _servCache.GuardarCache("Venta", venta, usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void RestarCantidad(long item, string usuarioId)
        {
            try
            {
                Venta venta = _servCache.RecuperarCache<Venta>("Venta", usuarioId);
                venta.RestarCantidad(item);
                _servCache.GuardarCache("Venta", venta, usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void EliminarLineaVenta(long item, string usuarioId)
        {
            try
            {
                Venta venta = _servCache.RecuperarCache<Venta>("Venta", usuarioId);
                venta.EliminarLineaVenta(item);
                _servCache.GuardarCache("Venta", venta, usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void FinalizarVenta(DatosTarjeta datosTarjeta, string usuarioId)
        {
            Venta venta = _servCache.RecuperarCache<Venta>("Venta", usuarioId);
            if (venta.Estado == (int)Estados.Iniciado)
            {
                venta.CambiarAPendiente();
                venta.Id = GuardarVenta(venta);
                _servCache.GuardarCache("Venta", venta, usuarioId);
                GuardarLineasVenta(venta.Lineas_Ventas, venta.Id);
            }
            else
            {
                venta = _servDatos.obtenerVenta(venta.Id);
            }

            if (venta.Num_Comprobante == 0)
            {
                AutorizarAfip(venta);
            }

            if (venta.Num_Comprobante != 0 && (venta.Estado == (int)Estados.Pendiente || venta.Estado == (int)Estados.Rechazado))
            {
                Pago pago = _servCache.RecuperarCache<Pago>("Pago", usuarioId);
                AutorizarPago(venta, pago, datosTarjeta);
            }
            ActualizarVenta();
            _servCache.GuardarCache("Venta", venta, usuarioId);
        }
        private long GuardarVenta(Venta venta)
        {
            return _servDatos.guardarVenta(venta);
        }
        private void GuardarLineasVenta(List<Linea_Venta> lineasVenta, long ventaId)
        {
            long numero_item = 1;
            foreach (Linea_Venta lv in lineasVenta)
            {
                lv.ActualizarLineaVenta(numero_item, ventaId);
                _servDatos.guardarLineaVenta(lv);
                _servDatos.actualizarInventario(lv.Inventario_Id, lv.Cantidad);
                numero_item++;
            }
        }
        private void AutorizarAfip(Venta venta)
        {
            DatosAfip datosAfip = _servCache.RecuperarCache<DatosAfip>("DatosAfip");

            if (datosAfip == null)
            {
                datosAfip = new DatosAfip();
                obtenerUltimosComprobantes(datosAfip);
            }

            venta.Num_Comprobante = datosAfip.Facturas.First(comp => comp.Id == venta.Comprobante_Id).Numero + 1;

            Autorizacion_Afip autorizacionAfip = new Autorizacion_Afip(venta);

            _servAfip.AutorizarVenta(venta, autorizacionAfip, datosAfip.Token);

            _servDatos.guardarAutorizacion(autorizacionAfip);

            if (autorizacionAfip.Estado == (int)Estados.Aprobado || autorizacionAfip.Estado == (int)Estados.Aprobado_con_obs)
            {
                ActualizarComprobantes(datosAfip, venta);
            }
            else
            {
                obtenerUltimosComprobantes(datosAfip);
            }

            if (!string.IsNullOrEmpty(autorizacionAfip.Observaciones))
            {
                venta.AgregarObservaciones(autorizacionAfip.Observaciones);
            }
        }
        private void ActualizarComprobantes(DatosAfip datosAfip, Venta venta)
        {
            datosAfip.Facturas.First(comp => comp.Id == venta.Comprobante_Id).Numero = (int)venta.Num_Comprobante;

            _servCache.GuardarCache("DatosAfip", datosAfip);
        }
        // SOLICITA LOS ULTIMOS COMPROBANTES
        // SI NO SE TIENE TOKEN O ESTA VENCIDO, SOLICITA UNO NUEVO
        private void obtenerUltimosComprobantes(DatosAfip datosAfip)
        {
            if (string.IsNullOrEmpty(datosAfip.Token) || datosAfip.Vencimiento < DateTime.Now)
            {
                obtenerAutorizacion(datosAfip);
            }

            _servAfip.obtenerUltimosComprobantes(datosAfip);

            _servCache.GuardarCache("DatosAfip", datosAfip);
        }
        // OBTIENE EL TOKEN DE AUTORIZACION DE AFIP
        // GUARDA EN CACHE EL TOKEN PARA USOS FUTUROS
        private void obtenerAutorizacion(DatosAfip datosAfip)
        {
            var codigo = _configuraciones.Value.Codigo_Grupo;
            _servAfip.obtenerAutorizacion(codigo, datosAfip);
        }
        private void AutorizarPago(Venta venta, Pago pago, DatosTarjeta datosTarjeta)
        {
            pago.ActualizarPago(venta);

            switch (pago.Metodo_Pago_Id)
            {
                case (int)MetPago.Efectivo:
                    pago.CompletarPago(Estados.Aprobado, "PAGO EN EFECTIVO");
                    venta.Observaciones = "PAGO EN EFECTIVO";
                    _servDatos.guardarPago(pago);

                    venta.CambiarAAprobada();
                    break;

                case (int)MetPago.Tarjeta:
                    _servTarjeta.AutorizarVenta(venta, datosTarjeta, pago);
                    _servDatos.guardarPago(pago);

                    if (pago.Estado == (int)Estados.Aprobado)
                    {
                        venta.CambiarAAprobada();
                        break;
                    }
                    else
                    {
                        venta.Estado = (long)pago.Estado;
                        venta.AgregarObservaciones(pago.Observaciones);
                        break;
                    }

                default:
                    break;
            }
        }
        private void ActualizarVenta()
        {
            _servDatos.guardarCambios();
        }
    }
}
