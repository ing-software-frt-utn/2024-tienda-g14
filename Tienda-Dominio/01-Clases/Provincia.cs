using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Dominio._01_Clases
{
    public class Provincia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Nombre { get; set; }
        public long Pais_Id {  get; set; }
        [ForeignKey("Pais_Id")]
        public virtual Pais Pais { get; set; }
    }
}
