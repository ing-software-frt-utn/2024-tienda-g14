using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Dominio._01_Clases
{
    public class Ciudad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Nombre { get; set; }
        public long Provincia_Id { get; set; }
        [ForeignKey("Provincia_Id")]
        public virtual Provincia Provincia { get; set; }
    }
}
