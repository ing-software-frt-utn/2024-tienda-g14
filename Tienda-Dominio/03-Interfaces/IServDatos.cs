using Tienda_Dominio._01_Clases;

namespace Tienda_Dominio._03_Interfaces
{
    public interface IServDatos
    {
        Venta obtenerVenta(long id_venta);
        Sucursal obtenerSucursal(long id_empleado);
        Cliente obtenerClientexDoc(long doc_cliente);
        Comprobante obtenerComprobante(long id_cond_emite, long id_cond_recibe);
        Articulo obtenerArticulo(long codigo_articulo);
        void guardarCambios();
        long guardarVenta(Venta venta);
        void guardarLineaVenta(Linea_Venta lineaventa);
        void actualizarInventario(long id_producto, long cantidad);
        void guardarAutorizacion(Autorizacion_Afip autorizacion);
        void guardarPago(Pago pago);
    }
}
