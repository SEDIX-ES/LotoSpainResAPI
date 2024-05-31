using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos
{
    public class Primitiva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime fecha { get; set; }
        public string combinacion { get; set; }
        public int complementario { get; set; }
        public int reintegro { get; set; }
    }
}
