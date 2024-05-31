using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LotoSpain_API.Modelos
{
    public class QuinigolRow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public int jornada { get; set; }
        public Partido partidos { get; set; }
        public string local { get; set; }
    }
}
