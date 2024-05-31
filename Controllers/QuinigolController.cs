using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LotoSpain_API.Datos;
using LotoSpain_API.Modelos;
using Dapper;
using LotoSpain_API.Modelos.DAO;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Mysqlx;
using Microsoft.IdentityModel.Tokens;

namespace LotoSpain_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuinigolController : ControllerBase
    {
        private readonly MySqlConnection _db;

        public QuinigolController()
        {
            _db = ConexionMySQL.getConexion();
        }

        // GET: api/Quinigol
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuinigolDAO>>> GetQuinigolAll()
        {
            var query = @"Select * from jornadasQuinigol order by fecha desc";
            var queryPartidos = @"Select local, visitante, resultado from quinigol_res where jornada=@jorn";
            IEnumerable<jornadasQuinigol> result = await _db.QueryAsync<jornadasQuinigol>(query);
            if (result.IsNullOrEmpty())
            {
                return NoContent();
            }
            List<jornadasQuinigol> resultList = result.AsList();

            List<QuinigolDAO> DaoList = new List<QuinigolDAO>();
            List<Partido> partidos = new List<Partido>();
            foreach (var item in resultList)
            {
                QuinigolDAO aux = new QuinigolDAO();
                IEnumerable<Partido> resultP = await _db.QueryAsync<Partido>(queryPartidos, new { jorn = item.jornada });
                partidos = resultP.ToList();

                aux.fecha = item.fecha;
                aux.jornada = item.jornada;
                aux.partidos = partidos.ToArray();
                DaoList.Add(aux);
            }
            return DaoList;
        }

        // GET: api/quinigoles/5
        [HttpGet("{fecha}")]
        public async Task<ActionResult<object>> GetQuinigolFecha(DateTime fecha)
        {
            var queryPartidos = @"Select local, visitante, resultado from quinigol_res where fecha=@date";
            var queryJornada = @"Select jornada from jornadasQuinigol where fecha=@date2";
            List<Partido> partidos = new List<Partido>();

            QuinigolDAO aux = new QuinigolDAO();
            IEnumerable<Partido> resultP = await _db.QueryAsync<Partido>(queryPartidos, new { date = fecha });
            if (resultP.IsNullOrEmpty())
            {
                return NoContent();
            }
            partidos = resultP.ToList();
            aux.fecha = fecha;
            aux.jornada = await _db.QueryFirstOrDefaultAsync<int>(queryJornada, new { date2 = fecha });
            aux.partidos = partidos.ToArray();

            return aux;
        }

        //PUT: api/quinigoles/5
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{fecha}")]
        public async Task<IActionResult> PutQuinigol(DateTime fecha, QuinigolDAO quinigol)
        {
            var query = @"UPDATE jornadasQuinigol SET fecha=@date, jornada=@jorn, WHERE fecha=@fecha2;";
            var queryPartido = @"UPDATE lotospain_ResYBotes.quinigol_res SET jornada=@jorn, visitante=@visit, resultado=@res WHERE fecha=@date AND local=@local;";
            await _db.QueryAsync(query, new { date = quinigol.fecha, jorn = quinigol.jornada, fecha2 = fecha });

            foreach (var item in quinigol.partidos)
            {
                await _db.QueryAsync(queryPartido, new { jorn = quinigol.jornada, visit = item.visitante, res = item.resultado, date = quinigol.fecha, local = item.local });
            }

            return NoContent();
        }

        //PUT: api/quinigoles/5
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("partido")]
        public async Task<IActionResult> PutPartido(QuinigolRow quinigol)
        {
            var queryPartido = @"UPDATE quinigol_res SET local=@local1, visitante=@visit, resultado=@res WHERE fecha=@date AND local=@old;";

            await _db.QueryAsync(queryPartido, new { jorn = quinigol.jornada, local1 = quinigol.partidos.local, visit = quinigol.partidos.visitante, res = quinigol.partidos.resultado, date = quinigol.fecha, old = quinigol.local });

            return NoContent();
        }

        // POST: api/quinigoles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Quinigol>> PostQuinigol(QuinigolDAO quinigol)
        {
            var query = @"Insert into jornadasQuinigol values (@date, @jorn)";

            var queryPartido = @"Insert into quinigol_res (fecha, jornada, local, visitante, resultado) values (@date, @jorn, @local, @visit, @res)";
            await _db.QueryAsync(query, new { date = quinigol.fecha, jorn = quinigol.jornada });

            foreach (Partido partido in quinigol.partidos)
            {
                await _db.QueryAsync(queryPartido, new { date = quinigol.fecha, jorn = quinigol.jornada, local = partido.local, visit = partido.visitante, res = partido.resultado });
            }

            return NoContent();
        }

        // DELETE: api/quinigoles/5
        [HttpDelete("{fecha}")]
        public async Task<ActionResult> deleteQuinigol(DateTime fecha)
        {
            var queryJornada = @"Delete from jornadasQuinigol where fecha=@date";
            var query = "Delete from quinigol_res where fecha=@date";

            await _db.QueryAsync(query, new { date = fecha });
            await _db.QueryAsync(queryJornada, new { date = fecha });

            return NoContent();
        }
        
        [HttpDelete("partido/{fecha}/{local}")]
        public async Task<ActionResult> deletePartidoQuinigol(string fecha, string local)
        {
            var queryJornada = @"Delete from quinigol_res where fecha=@date and local=@equipo";

            await _db.QueryAsync(queryJornada, new { date = fecha, equipo=local });

            return NoContent();
        }
    }
}
