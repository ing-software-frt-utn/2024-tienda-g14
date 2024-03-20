
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Dominio._01_Clases
{
    public class Linea_Venta
    {
        public long Venta_Id { get; set; }
        public long Item_num { get; set; }
        public long Inventario_Id { get; set; }
        public long Cantidad { get; set; }
        public decimal Neto_Gravado { get; set; }
        public decimal Porc_Iva { get; set; }
        public decimal Monto_Iva { get; set; }
        public decimal Precio { get; set; }
        [ForeignKey("Inventario_Id")]
        public virtual Inventario Inventario { get; set; }
        [ForeignKey("Venta_Id")]
        public virtual Venta Venta { get; set; }

        public Linea_Venta() { }
        public Linea_Venta(long item, long cantidad, Inventario inv)
        {
            Item_num = item;
            Inventario = inv;
            Inventario_Id = inv.Id;
            Cantidad = cantidad;
            var art = inv.Articulo;
            var mg = art.Margen_Ganancia / 100;
            var iva = art.IVA / 100;
            Neto_Gravado = art.Costo * (1 + mg);
            Porc_Iva = art.IVA;
            Monto_Iva = Neto_Gravado * iva;
            Precio = Neto_Gravado + Monto_Iva;
        }
        public void SumarCantidad()
        {
            Cantidad++;
        }
        public void RestarCantidad()
        {
            Cantidad--;
        }
        public void ActualizarLineaVenta(long item, long ventaId)
        {
            Item_num = item;
            Venta_Id = ventaId;
        }
    }
}
