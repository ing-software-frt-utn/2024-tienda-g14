using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tienda_Dominio._02_Enumeraciones;

namespace Tienda_Dominio._01_Clases
{
    public class Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long Cliente_Id { get; set; }
        public System.DateTime Fecha { get; set; }
        public long Sucursal_Id { get; set; }
        public string Usuario_Id { get; set; }
        public decimal Monto { get; set; }
        public long Comprobante_Id { get; set; }
        public long Num_Comprobante { get; set; }
        public long Estado { get; set; }
        public string Observaciones { get; set; }
        [ForeignKey("Cliente_Id")]
        public virtual Cliente Cliente { get; set; }
        [ForeignKey("Sucursal_Id")]
        public virtual Sucursal Sucursal { get; set; }
        [ForeignKey("Comprobante_Id")]
        public virtual Comprobante Comprobante { get; set; }
        public virtual List<Linea_Venta> Lineas_Ventas { get; set; }

        public Venta() { }
        public Venta(Cliente cliente, string usuarioId, Sucursal sucursal, Comprobante comprobante)
        {
            Id = 0;
            Cliente_Id = cliente.Id;
            Fecha = DateTime.Now;
            Sucursal_Id = sucursal.Id;
            Usuario_Id = usuarioId;
            Monto = 0;
            Comprobante_Id = comprobante.Id;
            Num_Comprobante = 0;
            Estado = (int)Estados.Iniciado;
            Observaciones = String.Empty;

            Cliente = cliente;
            Sucursal = sucursal;
            Comprobante = comprobante;

            Lineas_Ventas = new List<Linea_Venta>();
        }
        public List<Linea_Venta> ObtenerLineasdeVenta()
        {
            return Lineas_Ventas.ToList();
        }

        public void AgregarObservaciones(string observaciones)
        {
            Observaciones = Observaciones + observaciones;
        }
        public void EliminarObservaciones()
        {
            Observaciones = String.Empty;
        }
        public void CambiarCliente(Cliente cliente, Comprobante comprobante)
        {
            Cliente_Id = cliente.Id;
            Comprobante_Id = comprobante.Id;

            Cliente = cliente;
            Comprobante = comprobante;
        }
        public void CambiarAPendiente()
        {
            Estado = (int)Estados.Pendiente;
        }
        public void CambiarAAprobada()
        {
            Estado = (int)Estados.Aprobado;
        }
        public void AgregarLineaVenta(Inventario inv, long cantidad)
        {
            if (Lineas_Ventas != null)
            {
                if (Lineas_Ventas.Count() != 0)
                {
                    if (Lineas_Ventas.Exists(lv => lv.Inventario.Id == inv.Id))
                    {
                        Lineas_Ventas.Single(lv => lv.Inventario.Id == inv.Id).Cantidad += cantidad;
                    }
                    else
                    {
                        long item = Lineas_Ventas.Max(lv => lv.Item_num) + 1;
                        var lv = new Linea_Venta(item, cantidad, inv);
                        Lineas_Ventas.Add(lv);
                    }
                }
                else
                {
                    var lv = new Linea_Venta(1, cantidad, inv);
                    Lineas_Ventas.Add(lv);
                }
                ActualizarMonto();
            }
            else
            {
                throw new Exception("Venta no iniciada correctamente.");
            }
        }
        private void ActualizarMonto()
        {
            if (Lineas_Ventas != null && Lineas_Ventas.Count > 0)
            {
                Monto = Lineas_Ventas.Sum(lv => lv.Precio * lv.Cantidad);
            }
            else
            {
                Monto = 0;
            }
        }
        public void SumarCantidad(long item)
        {
            if (Lineas_Ventas != null && Lineas_Ventas.Count > 0)
            {
                Linea_Venta lv = Lineas_Ventas.FirstOrDefault(lv => lv.Item_num == item);
                if (lv != null)
                {
                    if (lv.Cantidad < lv.Inventario.Cantidad)
                    {
                        lv.SumarCantidad();
                        ActualizarMonto();
                    }
                    else
                    {
                        throw new Exception("No hay suficiente inventario de este producto en la sucursal.");
                    }
                }
                else
                {
                    throw new Exception("No existe la linea de venta.");
                }
            }
            else
            {
                throw new Exception("No hay lineas de venta asociadas a la venta.");
            }
        }
        public void RestarCantidad(long item)
        {
            if (Lineas_Ventas != null)
            {
                Linea_Venta lv = Lineas_Ventas.FirstOrDefault(lv => lv.Item_num == item);
                if (lv != null)
                {
                    lv.RestarCantidad();
                    if (lv.Cantidad <= 0)
                    {
                        Lineas_Ventas.Remove(lv);
                    }
                    ActualizarMonto();
                }
                else
                {
                    throw new Exception("No existe la linea de venta.");
                }
            }
            else
            {
                throw new Exception("No hay lineas de venta asociadas a la venta.");
            }
        }
        public void EliminarLineaVenta(long item)
        {
            if (Lineas_Ventas != null)
            {
                Linea_Venta lv = Lineas_Ventas.FirstOrDefault(lv => lv.Item_num == item);
                if (lv != null)
                {
                    Lineas_Ventas.Remove(lv);
                    ActualizarMonto();
                }
                else
                {
                    throw new Exception("No existe la linea de venta.");
                }
            }
            else
            {
                throw new Exception("No hay lineas de venta asociadas a la venta.");
            }
        }

        public decimal ObtenerNeto()
        {
            if (Lineas_Ventas.Any())
            {
                return Lineas_Ventas.Sum(lv => lv.Neto_Gravado);
            }
            else
            {
                throw new Exception("No hay lineas de venta asociadas a la venta.");
            }
        }

        public decimal ObtenerIva()
        {
            if (Lineas_Ventas.Any())
            {
                return Lineas_Ventas.Sum(lv => lv.Monto_Iva);
            }
            else
            {
                throw new Exception("No hay lineas de venta asociadas a la venta.");
            }
        }
    }
}
