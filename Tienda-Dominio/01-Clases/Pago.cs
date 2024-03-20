using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tienda_Dominio._02_Enumeraciones;

namespace Tienda_Dominio._01_Clases
{
    public class Pago
    {
        [Key]
        public long Id { get; set; }
        public long Venta_Id { get; set; }
        public System.DateTime Fecha { get; set; }
        public long Metodo_Pago_Id { get; set; }
        public decimal Monto { get; set; }
        public string Ticket { get; set; }
        public byte Estado { get; set; }
        public string Observaciones { get; set; }
        [ForeignKey("Venta_Id")]
        public virtual Venta Venta { get; set; }
        [ForeignKey("Metodo_Pago_Id")]
        public virtual Metodo_Pago Metodo_Pago { get; set; }

        public Pago()
        {

        }
        public Pago(int metodopago)
        {
            Id = 0;
            Metodo_Pago_Id = metodopago;
        }

        public void CambiarMetodoPago(long MetPagoId)
        {
            Metodo_Pago_Id = MetPagoId;
        }
        public void ActualizarPago(Venta venta)
        {
            Venta_Id = venta.Id;
            Fecha = venta.Fecha;
            Monto = venta.Monto;
        }
        public void CompletarPago(Estados estado, String observaciones = "")
        {
            Estado = (int)Estados.Aprobado;
            if (observaciones != "")
            {
                Ticket = observaciones;
            }
            Observaciones = observaciones;
        }
    }

}
