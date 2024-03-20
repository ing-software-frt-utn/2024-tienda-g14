using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Dominio._01_Clases
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long TipoDocumento {  get; set; }
        public long NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Domicilio { get; set; }
        public long Ciudad_Id { get; set; }
        public long Condicion_Tributaria_Id { get; set; }
        [ForeignKey("Ciudad_Id")]
        public virtual Ciudad Ciudad { get; set; }
        [ForeignKey("Condicion_Tributaria_Id")]
        public virtual Condicion_Tributaria Condicion_Tributaria { get; set; }
    }
}
