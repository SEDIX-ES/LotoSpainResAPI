using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos
{
    public class Cantidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public DateTime fecha { get; set; }
    }
}
