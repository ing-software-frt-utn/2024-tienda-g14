namespace Tienda_Dominio._01_Clases
{
    public class DatosAfip
    {
        public String Token {  get; set; }
        public DateTime Vencimiento { get; set; }
        public List<Factura> Facturas {  get; set; }
        public DatosAfip() 
        {
            Token = string.Empty;
            Vencimiento = DateTime.MinValue;
            Facturas = new List<Factura>();
        }
    }
    public class Factura
    {
        public long Id { get; set; }
        public long Numero { get; set; }
        public Factura()
        {
            Id = 0;
            Numero = 0;
        }

        public Factura(long id, long numero)
        {
            Id = id;
            Numero = numero;
        }
    }

}
