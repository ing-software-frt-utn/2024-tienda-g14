using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Dominio._01_Clases
{
    public class Talle
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long Tipo_Talle_Id { get; set; }
        public string Descripcion { get; set; }
        [ForeignKey("Tipo_Talle_Id")]
        public virtual Tipo_Talle Tipo_Talle { get; set; }
    }
}
