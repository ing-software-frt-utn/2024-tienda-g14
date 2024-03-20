using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Dominio._01_Clases
{
    public class Autorizacion_Afip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long Venta_Id { get; set; }
        public System.DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public byte Estado { get; set; }
        public string Observaciones { get; set; }
        public string Cae { get; set; }
        [ForeignKey("Venta_Id")]
        public virtual Venta Venta { get; set; }

        public Autorizacion_Afip()
        {
        }

        public Autorizacion_Afip(Venta venta)
        {
            Venta_Id = venta.Id;
            Fecha = venta.Fecha;
            Monto = venta.Monto;
            Estado = 0;
            Cae = String.Empty;
            Observaciones = String.Empty;
        }
    }
}
