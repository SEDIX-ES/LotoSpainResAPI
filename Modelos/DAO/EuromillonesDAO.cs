using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos.DAO
{
    public class EuromillonesDAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime fecha { get; set; }
        public int[] combinacion { get; set; }
        public int[] estrellas { get; set; }
    }
}
