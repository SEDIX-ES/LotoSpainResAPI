using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos
{
    public class Partido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string local { get; set; }
        public string visitante { get; set; }
        public string resultado { get; set; }
    }
}
