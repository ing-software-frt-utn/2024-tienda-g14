
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Dominio._01_Clases
{
    public class Punto_Venta
    {
        public long Sucursal_Id { get; set; }
        public long Numero_Punto_Venta { get; set; }
        public byte Habilitado { get; set; }
        [ForeignKey("Sucursal_Id")]
        public virtual Sucursal Sucursal { get; set; }
    }
}
