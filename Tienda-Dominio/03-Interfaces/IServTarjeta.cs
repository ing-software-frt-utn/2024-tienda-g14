using Tienda_Dominio._01_Clases;

namespace Tienda_Dominio._03_Interfaces
{
    public interface IServTarjeta
    {
        void AutorizarVenta(Venta venta, DatosTarjeta datos, Pago pago);
    }
}
