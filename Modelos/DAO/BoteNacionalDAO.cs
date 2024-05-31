using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos.DAO
{
    public class BoteNacionalDAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int cantidad { get; set; }
        public DateTime fecha { get; set; }
        public string sorteo { get; set; }
        public string tipo { get; set; }
        public string? nombreExtra { get; set; }
        public int id_nacional { get; set; }
    }
}
