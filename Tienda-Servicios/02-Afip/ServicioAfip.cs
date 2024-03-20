using Tienda_Dominio._01_Clases;
using Tienda_Dominio._02_Enumeraciones;
using Tienda_Dominio._03_Interfaces;
using AfipWS;

namespace Tienda_Servicios._02_Afip
{
    public class ServicioAfip : IServAFIP
    {
        public void obtenerAutorizacion(String codigo, DatosAfip datos)
        {
            LoginServiceClient WS = new LoginServiceClient();
            WS.Open();
            Autorizacion autorizacion = WS.SolicitarAutorizacionAsync(codigo).Result;
            if(autorizacion != null)
            {
                datos.Token = autorizacion.Token;
                datos.Vencimiento = autorizacion.Vencimiento;
            }
            else
            {
                throw new Exception("Error al obtener el token de autorización.");
            }
        }
        public void obtenerUltimosComprobantes(DatosAfip datos)
        {
            LoginServiceClient WS = new LoginServiceClient();
            WS.Open();
            UltimoComprobante ultimoComprobante = WS.SolicitarUltimosComprobantesAsync(datos.Token).Result;
            if(ultimoComprobante != null)
            {
                datos.Facturas.Clear();
                foreach(var comprobante in ultimoComprobante.Comprobantes)
                {
                    Factura factura = new Factura(comprobante.Id, comprobante.Numero);
                    datos.Facturas.Add(factura);
                }
            }
            else
            {
                throw new Exception("Error al obtener los ultimos comprobantes.");
            }
        }
        public void AutorizarVenta(Venta venta, Autorizacion_Afip autorizacionAfip, String token)
        {
            SolicitudAutorizacion solicitud = CrearSolicitud(venta);
            ResultadoSolicitudAutorizacion resultado = AutorizarVenta(token, solicitud).Result;

            switch (resultado.Estado)
            {
                case EstadoSolicitud.Aprobada:
                    autorizacionAfip.Estado = (int)Estados.Aprobado;
                    autorizacionAfip.Cae = resultado.Cae;
                    break;
                case EstadoSolicitud.Rechazada:
                    autorizacionAfip.Estado = (int)Estados.Rechazado;
                    break;
                case EstadoSolicitud.AprobadaParcialmente:
                    autorizacionAfip.Estado = (int)Estados.Aprobado_con_obs;
                    autorizacionAfip.Cae = resultado.Cae;
                    break;
                default:
                    autorizacionAfip.Estado = (int)Estados.Rechazado;
                    break;
            }

            if (resultado.Observacion != null)
            {
                autorizacionAfip.Observaciones = resultado.Observacion;
            }
        }
        private SolicitudAutorizacion CrearSolicitud(Venta venta)
        {
            SolicitudAutorizacion solicitud = new SolicitudAutorizacion();

            solicitud.Fecha = venta.Fecha;

            solicitud.ImporteNeto = decimal.ToDouble(venta.ObtenerNeto());
            solicitud.ImporteIva = decimal.ToDouble(venta.ObtenerIva());
            solicitud.ImporteTotal = solicitud.ImporteNeto + solicitud.ImporteIva;

            switch (venta.Comprobante_Id)
            {
                case (int)Comprobantes.FacturaA:
                    solicitud.TipoComprobante = TipoComprobante.FacturaA;
                    break;
                case (int)Comprobantes.FacturaB:
                    solicitud.TipoComprobante = TipoComprobante.FacturaB;
                    break;
                default:
                    solicitud.TipoComprobante = TipoComprobante.FacturaB;
                    break;
            }

            solicitud.Numero = venta.Num_Comprobante;

            int largo = venta.Cliente.NumeroDocumento.ToString().Length;
            switch (venta.Cliente.TipoDocumento)
            {
                case 1:
                    solicitud.TipoDocumento = TipoDocumento.ConsumidorFinal;
                    solicitud.NumeroDocumento = 0;
                    break;
                case 2:
                    solicitud.TipoDocumento = TipoDocumento.Dni;
                    if (largo == 7 || largo == 8)
                    {
                        solicitud.NumeroDocumento = venta.Cliente.NumeroDocumento;
                    }
                    break;
                case 3:
                    solicitud.TipoDocumento = TipoDocumento.Cuil;
                    if (largo == 11)
                    {
                        solicitud.NumeroDocumento = venta.Cliente.NumeroDocumento;
                    }
                    break;
                case 4:
                    solicitud.TipoDocumento = TipoDocumento.Cuit;
                    if (largo == 11)
                    {
                        solicitud.NumeroDocumento = venta.Cliente.NumeroDocumento;
                    }
                    break;
                default:
                    solicitud.TipoDocumento = TipoDocumento.ConsumidorFinal;
                    solicitud.NumeroDocumento = 0;
                    break;
            }
            return solicitud;
        }
        private async Task<ResultadoSolicitudAutorizacion> AutorizarVenta(String token, SolicitudAutorizacion solicitud)
        {
            LoginServiceClient WS = new LoginServiceClient();
            WS.Open();
            ResultadoSolicitudAutorizacion resultado = await WS.SolicitarCaeAsync(token, solicitud);
            return resultado;
        }
    }
}
