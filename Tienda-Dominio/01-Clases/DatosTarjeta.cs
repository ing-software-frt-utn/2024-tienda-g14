namespace Tienda_Dominio._01_Clases
{

    public class DatosTarjeta
    {
        public string numTarjeta { get; set; }
        public string nombreTitular { get; set; }
        public string dniTitular { get; set; }
        public string mesVencimiento { get; set; }
        public string anioVencimiento { get; set; }
        public string codSeguridad { get; set; }
        public DatosTarjeta(string numTarj, string nombTit, string dniTit, string mesVenc, string anioVenc, string codSeg)
        {
            numTarjeta = numTarj;
            nombreTitular = nombTit;
            dniTitular = dniTit;
            mesVencimiento = mesVenc;
            anioVencimiento = anioVenc;
            codSeguridad = codSeg;
        }
    }
}
