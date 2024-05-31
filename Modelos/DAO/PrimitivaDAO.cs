using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos.DAO
{
    public class PrimitivaDAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime fecha { get; set; }
        public int[] combinacion { get; set; }
        public int complementario { get; set; }
        public int reintegro { get; set; }
    }
}
