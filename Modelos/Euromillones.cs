using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos
{
    public class Euromillones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime fecha { get; set; }
        public string combinacion { get; set; }
        public string estrellas { get; set; }
    }
}
