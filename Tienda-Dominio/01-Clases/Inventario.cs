using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Dominio._01_Clases
{
    public class Inventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long Sucursal_Id { get; set; }
        public long Articulo_Id { get; set; }
        public long Color_Id { get; set; }
        public long Talle_Id { get; set; }
        public long Cantidad { get; set; }
        [ForeignKey("Sucursal_Id")]
        public virtual Sucursal Sucursal { get; set; }
        [ForeignKey("Articulo_Id")]
        public virtual Articulo Articulo { get; set; }
        [ForeignKey("Color_Id")]
        public virtual Color Color { get; set; }
        [ForeignKey("Talle_Id")]
        public virtual Talle Talle { get; set; }
    }
}
