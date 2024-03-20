using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Dominio._01_Clases
{
    public class Articulo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long Codigo { get; set; }
        public string Descripcion { get; set; }
        public long Marca_Id { get; set; }
        public long Categoria_Id { get; set; }
        public long Tipo_Talle_Id { get; set; }
        public decimal Costo { get; set; }
        public decimal Margen_Ganancia { get; set; }
        public decimal IVA { get; set; }
        [ForeignKey("Marca_Id")]
        public virtual Marca Marca { get; set; }
        [ForeignKey("Categoria_Id")]
        public virtual Categoria Categoria { get; set; }
        [ForeignKey("Tipo_Talle_Id")]
        public virtual Tipo_Talle Tipo_Talle { get; set; }
        public virtual List<Inventario> Inventarios { get; set; }

        public void ActualizarInventario(Venta venta)
        {
            if (Inventarios != null)
            {
                Inventarios.RemoveAll(inv => inv.Sucursal_Id != venta.Sucursal_Id);
                if(Inventarios.Count() > 0)
                {
                    List<Linea_Venta> lineasVentas = venta.Lineas_Ventas;
                    if (lineasVentas != null)
                    {
                        foreach (Inventario inv in Inventarios) // RECORRRO EL INVENTARIO DISPONIBLE EN ESTA SUCURSAL
                        {
                            var existe = lineasVentas.Exists(lv => lv.Inventario_Id == inv.Id); // VERIFICO SI EXISTEN LINEAS DE VENTAS CON ESTE INVENTARIO
                            if (existe)
                            {
                                var lv = lineasVentas.Single(lv => lv.Inventario_Id == inv.Id);
                                inv.Cantidad -= lv.Cantidad;   // SI EXISTE, RESTO LA CANTIDAD DE LA LINEA DE VENTA AL DISPONIBLE
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Articulo sin inventario.");
                }
            }
            else
            {
                throw new Exception("Inventario igual a null.");
            }
        }
        public Inventario ObtenerInventario(long productoId)
        {
            if (Inventarios != null && Inventarios.Count > 0)
            {
                return Inventarios.Single(inv => inv.Id == productoId);
            }
            else
            {
                throw new Exception("No hay inventario para este articulo.");
            }

        }
    }
}
