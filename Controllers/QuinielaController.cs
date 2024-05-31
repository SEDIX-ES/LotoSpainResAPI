using Microsoft.AspNetCore.Mvc;
using LotoSpain_API.Datos;
using LotoSpain_API.Modelos;
using Dapper;
using LotoSpain_API.Modelos.DAO;
using MySql.Data.MySqlClient;
using Microsoft.IdentityModel.Tokens;

namespace LotoSpain_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuinielaController : ControllerBase
    {
        private readonly MySqlConnection _db;

        public QuinielaController()
        {
            _db = ConexionMySQL.getConexion();
        }

        // GET: api/Quiniela
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuinielaDAO>>> GetQuinielaAll()
        {
            var query = @"Select * from jornadasQuiniela order by fecha desc";
            var queryPartidos = @"Select local, visitante, resultado from quiniela_res where jornada=@jorn";
            IEnumerable<jornadasQuiniela> result = await _db.QueryAsync<jornadasQuiniela>(query);
            if (result == null)
            {
                return NoContent();
            }
            List<jornadasQuiniela> resultList = result.AsList();

            List<QuinielaDAO> DaoList = new List<QuinielaDAO>();
            List<Partido> partidos = new List<Partido>();
            foreach (var item in resultList)
            {
                QuinielaDAO aux = new QuinielaDAO();
                IEnumerable<Partido> resultP = await _db.QueryAsync<Partido>(queryPartidos, new {jorn=item.jornada});
                partidos=resultP.ToList();

                aux.fecha = item.fecha;
                aux.jornada = item.jornada;
                aux.partidos = partidos.ToArray();
                DaoList.Add(aux);
            }
            return DaoList;
        }

        // GET: api/quinielaes/5
        [HttpGet("{fecha}")]
        public async Task<ActionResult<QuinielaDAO>> GetQuinielaFecha(DateTime fecha)
        {
            var queryPartidos = @"Select local, visitante, resultado from quiniela_res where fecha=@date";
            var queryJornada = @"Select jornada from jornadasQuiniela where fecha=@date";
            List<Partido> partidos = new List<Partido>();

            QuinielaDAO aux = new QuinielaDAO();
            IEnumerable<Partido> resultP = await _db.QueryAsync<Partido>(queryPartidos, new {date = fecha});
            if (resultP.IsNullOrEmpty())
            {
                return NoContent();
            }
            partidos = resultP.ToList();

                aux.fecha = fecha;
                aux.jornada = await _db.QueryFirstOrDefaultAsync<int>(queryJornada, new {date=fecha});
                aux.partidos = partidos.ToArray();

            return aux;
        }

        //PUT: api/quinielaes/5
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{fecha}")]
        public async Task<IActionResult> PutQuiniela(DateTime fecha, QuinielaDAO quiniela)
        {
            var query = @"UPDATE jornadasQuiniela SET fecha=@date, jornada=@jorn WHERE fecha=@fecha2;";
            var queryPartido = @"UPDATE lotospain_ResYBotes.quiniela_res SET jornada=@jorn, visitante=@visit, resultado=@res WHERE fecha=@date AND local=@local;";
            await _db.QueryAsync(query, new {date=quiniela.fecha, jorn=quiniela.jornada, fecha2=fecha});

            foreach (var item in quiniela.partidos)
            {
                await _db.QueryAsync(queryPartido, new {jorn=quiniela.jornada, visit=item.visitante, res=item.resultado, date=quiniela.fecha, local=item.local});
            }

            return NoContent();
        }

        //PUT: api/quinielaes/5
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("partido")]
        public async Task<IActionResult> PutPartido(QuinielaRow quiniela)
        {
            var queryPartido = @"UPDATE quiniela_res SET local=@local1, visitante=@visit, resultado=@res WHERE fecha=@date AND local=@old;";

            await _db.QueryAsync(queryPartido, new {jorn=quiniela.jornada, local1=quiniela.partidos.local, visit=quiniela.partidos.visitante, res=quiniela.partidos.resultado, date=quiniela.fecha, old=quiniela.local});

            return NoContent();
        }

        // POST: api/quinielaes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Quiniela>> PostQuiniela(QuinielaDAO quiniela)
        {
            var query = @"Insert into jornadasQuiniela values (@date, @jorn)";

            var queryPartido = @"Insert into quiniela_res (fecha, jornada, local, visitante, resultado) values (@date, @jorn, @local, @visit, @res)";
           
            await _db.QueryAsync(query, new { date = quiniela.fecha, jorn = quiniela.jornada});
            
            foreach(Partido partido in quiniela.partidos){
                await _db.QueryAsync(queryPartido, new {date=quiniela.fecha, jorn=quiniela.jornada, local=partido.local, visit=partido.visitante, res=partido.resultado});
            }

            return NoContent();
        }

        // DELETE: api/quinielaes/5
        [HttpDelete("{fecha}")]
        public async Task<ActionResult> deleteQuiniela(DateTime fecha)
        {
            var queryJornada = @"Delete from jornadasQuiniela where fecha=@date";
            var query = "Delete from quiniela_res where fecha=@date";

            await _db.QueryAsync(query, new { date = fecha });
            await _db.QueryAsync(queryJornada, new { date = fecha });

            return NoContent();
        }

        [HttpDelete("partido/{fecha}/{local}")]
        public async Task<ActionResult> deletePartidoQuiniela(string fecha, string local)
        {
            var queryJornada = @"Delete from quiniela_res where fecha=@date and local=@equipo";

            await _db.QueryAsync(queryJornada, new { date = fecha, equipo = local });

            return NoContent();
        }
    }
}
