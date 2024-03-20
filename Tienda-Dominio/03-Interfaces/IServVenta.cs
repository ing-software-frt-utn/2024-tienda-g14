using Tienda_Dominio._01_Clases;

namespace Tienda_Dominio._03_Interfaces
{
    public interface IServVenta
    {
        Venta RecuperarVenta(string usuarioId);
        Articulo RecuperarArticulo(string usuarioId);
        void IniciarVenta(string usuarioId, long empleadoId);
        void BuscarProductos(long codigo, string usuarioId);
        void AgregarProducto(long productoId, long cantidad, string usuarioId);
        void CambiarCliente(long doc, string usuarioId);
        void CambiarMetodoPago(long metodo, string usuarioId);
        void EliminarLineaVenta(long item, string usuarioId);
        void SumarCantidad(long item, string usuarioId);
        void RestarCantidad(long item, string usuarioId);
        void FinalizarVenta(DatosTarjeta datosTarjeta, string usuarioId);
    }
}
