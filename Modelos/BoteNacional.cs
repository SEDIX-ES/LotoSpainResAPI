using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos
{
    public class BoteNacional
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int fk_bote { get; set; }
        public string tipo { get; set; }
        public string? nombreExtra { get; set; }
    }
}
