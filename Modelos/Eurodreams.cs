using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos
{
    public class Eurodreams
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime fecha { get; set; }
        public string combinacion { get; set; }
        public int suenho { get; set; }
        public int fk_bote { get; set; }
    }
}
