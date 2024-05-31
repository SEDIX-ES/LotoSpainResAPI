using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos
{
    public class Bote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public int cantidad { get; set; }
        public int fk_sorteo { get; set; }
        public int? jornada { get; set; }
    }
}
