using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Dominio._01_Clases
{
    public class Sucursal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Nombre { get; set; }
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
