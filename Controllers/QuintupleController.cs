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

namespace LotoSpain_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuintupleController : ControllerBase
    {
        private readonly MySqlConnection _db;

        public QuintupleController()
        {
            _db = ConexionMySQL.getConexion();
        }

        // GET: api/Quintuple
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuintupleDAO>>> GetQuintupleAll()
        {
            var query = @"Select * from quintuple_res order by fecha desc";

            IEnumerable<Quintuple> result = await _db.QueryAsync<Quintuple>(query);
            if (result == null)
            {
                return NoContent();
            }
            List<Quintuple> resultList = result.AsList();

            List<QuintupleDAO> DaoList = new List<QuintupleDAO>();

            foreach (var item in resultList)
            {
                QuintupleDAO aux = new QuintupleDAO();

                aux.fecha = item.fecha;
                aux.jornada = item.jornada;
                aux.combinacion = stringAInt(item.combinacion);
                DaoList.Add(aux);
            }

            return DaoList;
        }

        // GET: api/quintuplees/5
        [HttpGet("{fecha}")]
        public async Task<ActionResult<QuintupleDAO>> GetQuintuple(DateTime fecha)
        {
            var query = @"Select * from quintuple_res where fecha=@date";

            Quintuple result = await _db.QueryFirstOrDefaultAsync<Quintuple>(query, new { date = fecha });
            if (result == null)
            {
                return NoContent();
            }
            QuintupleDAO DaoList = new QuintupleDAO();

            DaoList.fecha = result.fecha;
            DaoList.jornada = result.jornada;
            DaoList.combinacion = stringAInt(result.combinacion);

            return DaoList;
        }

        // PUT: api/quintuplees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{fecha}")]
        public async Task<IActionResult> PutQuintuple(DateTime fecha, QuintupleDAO quintuple)
        {
            var query = "UPDATE quintuple_res SET combinacion=@combi WHERE fecha=@fecha2;";
            var combin = "";
            for (int i = 0; i < quintuple.combinacion.Length; i++)
            {
                if (i == quintuple.combinacion.Length - 1)
                {
                    combin = combin + quintuple.combinacion[i];
                }
                else
                {
                    combin = combin + quintuple.combinacion[i] + ",";
                }
            }
            await _db.QueryAsync(query, new {combi = combin, fecha2 = fecha });

            return NoContent();
        }

        // POST: api/quintuplees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Quintuple>> PostQuintuple(QuintupleDAO quintuple)
        {
            var query = @"Insert into quintuple_res values (@fechaID, @jorn, @combi)";
            var combin = "";
            for (int i = 0; i < quintuple.combinacion.Length; i++)
            {
                if (i == quintuple.combinacion.Length - 1)
                {
                    combin = combin + quintuple.combinacion[i];
                }
                else
                {
                    combin = combin + quintuple.combinacion[i] + ",";
                }
            }

            await _db.QueryAsync(query, new { fechaID = quintuple.fecha, jorn = quintuple.jornada, combi = combin});

            return NoContent();
        }

        // DELETE: api/quintuplees/5
        [HttpDelete("{fecha}")]
        public async Task<ActionResult> deleteQuintuple(DateTime fecha)
        {
            var query = "Delete from quintuple_res where fecha=@date";

            await _db.QueryAsync(query, new { date = fecha });

            return NoContent();
        }

        private string[] stringAInt(string s)
        {
            string[] strings = s.Split(',');


            return strings;
        }
    }
}
