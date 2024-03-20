using Tienda_Dominio._01_Clases;

namespace Tienda_Dominio._03_Interfaces
{
    public interface IServAFIP
    {
        void obtenerAutorizacion(String codigo, DatosAfip datos);
        void obtenerUltimosComprobantes(DatosAfip datos);
        void AutorizarVenta(Venta venta, Autorizacion_Afip autorizacionAfip, String token);
    }
}
