using Tienda_Dominio._01_Clases;
using Tienda_Dominio._02_Enumeraciones;
using Tienda_Dominio._03_Interfaces;

namespace Tienda_Presentacion.Models
{
    public class VentaModel
    {
        public String? Alerta { get; set; }
        public bool MostrarVenta { get; set; }
        public bool MostrarFormCliente { get; set; }
        public String? Cliente {  get; set; }
        public String? ErrorFormCliente { get; set; }
        public String? Comprobante { get; set; }
        public String? Monto { get; set; }
        public List<Linea_Venta> LineasVenta { get; set; }
        public long Cantidad { get; set; }
        public bool Efectivo {  get; set; }
        public bool MostrarArticulo { get; set; }
        public String? ArtCodigo { get; set; }
        public String? ErrorFormArticulo { get; set; }
        public String? ArtDescripcion { get; set; }
        public String? ArtTipoTalle { get; set; }
        public String? ArtPrecio { get; set; }
        public List<Inventario> Inventarios { get; set; }

        public VentaModel()
        {
            MostrarVenta = false;
            MostrarArticulo = false;
            MostrarFormCliente = false;
            Efectivo = true;
        }
        public void CargarModelo(Venta? venta, Articulo? articulo)
        {
            if(venta != null)
            { 
                if (venta.Estado == (long)Estados.Aprobado || venta.Estado == (long)Estados.Aprobado_con_obs)
                {
                    Alerta = "VENTA NUM. " + venta.Id.ToString() + ": REGISTRADA Y APROBADA.";
                }
                else
                {
                    MostrarVenta = true;
                    Cliente = $"{venta.Cliente.NumeroDocumento} - {venta.Cliente.Nombres} {venta.Cliente.Apellidos}";
                    Comprobante = venta.Comprobante.Nombre;
                    Monto = venta.Monto.ToString("#,##0.00");

                    List<Linea_Venta> lineasVentas = venta.ObtenerLineasdeVenta();
                    if (lineasVentas != null)
                    {
                        LineasVenta = lineasVentas;
                        Cantidad = lineasVentas.Sum(x => x.Cantidad);
                    }
                    else
                    {
                        Cantidad = 0;
                    }

                    //Pago pago = _servCache.RecuperarCache<Pago>("Pago", _userId); ;
                    //switch (pago.Metodo_Pago_Id)
                    //{
                    //    case (int)MetPago.Efectivo:
                    //        ViewBag.Radio1 = "true";
                    //        break;
                    //    case (int)MetPago.Tarjeta:
                    //        ViewBag.Radio2 = "true";
                    //        break;
                    //    default:
                    //        ViewBag.Radio1 = "true";
                    //        break;
                    //}

                    if (articulo != null && articulo.Inventarios != null && articulo.Inventarios.Count > 0)
                    {
                        MostrarArticulo = true;
                        ArtCodigo = articulo.Codigo.ToString();
                        ArtDescripcion= articulo.Descripcion;
                        ArtTipoTalle = articulo.Tipo_Talle.Descipcion;
                        ArtPrecio = (articulo.Costo * ((100 + articulo.IVA + articulo.Margen_Ganancia) / 100)).ToString("#,##0.00");
                        Inventarios = articulo.Inventarios.Where(inv => inv.Cantidad > 0).ToList();
                    }
                }
            }
        }
    }
}
