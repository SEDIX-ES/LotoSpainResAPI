using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos.DAO
{
    public class NacionalDAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime fecha { get; set; }
        public string tipo { get; set; }
        public string combinacion1 { get; set; }
        public string combinacion2 { get; set; }
        public int[] reintegros { get; set; }
        public string? nombreExtra { get; set; }
    }
}
