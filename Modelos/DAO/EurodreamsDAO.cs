using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos.DAO
{
    public class EurodreamsDAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime fecha { get; set; }
        public int[] combinacion { get; set; }
        public int suenho { get; set; }
    }
}
