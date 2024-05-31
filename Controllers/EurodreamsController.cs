using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LotoSpain_API.Datos;
using MySql.Data.MySqlClient;
using Dapper;
using LotoSpain_API.Modelos.DAO;
using LotoSpain_API.Modelos;

namespace LotoSpain_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EurodreamsController : ControllerBase
    {
        private readonly MySqlConnection _db;

        public EurodreamsController()
        {
            _db = ConexionMySQL.getConexion();
        }

        // GET: api/eurodreamses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EurodreamsDAO>>> GetEurodreamsAll()
        {
            var query = @"Select * from eurodreams_res order by fecha desc";

            IEnumerable<Eurodreams> result = await _db.QueryAsync<Eurodreams>(query);
            if (result == null)
            {
                return NoContent();
            }
            List<Eurodreams> resultList = result.AsList();

            List<EurodreamsDAO> DaoList = new List<EurodreamsDAO>();

            foreach (var item in resultList)
            {
                EurodreamsDAO aux = new EurodreamsDAO();

                aux.fecha = item.fecha;
                aux.suenho = item.suenho;
                aux.combinacion = stringAInt(item.combinacion);
                DaoList.Add(aux);
            }

            return DaoList;
        }

        // GET: api/eurodreamses/5
        [HttpGet("{fecha}")]
        public async Task<ActionResult<EurodreamsDAO>> GetEurodreams(DateTime fecha)
        {
            var query = @"Select * from eurodreams_res where fecha=@date";

            Eurodreams result = await _db.QueryFirstOrDefaultAsync<Eurodreams>(query, new { date=fecha});
            if (result == null)
            {
                return NoContent();
            }
            EurodreamsDAO DaoList = new EurodreamsDAO();

            DaoList.fecha = result.fecha;
            DaoList.suenho = result.suenho;
            DaoList.combinacion = stringAInt(result.combinacion);

            return DaoList;
        }

        // PUT: api/eurodreamses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{fecha}")]
        public async Task<IActionResult> PutEurodreams(DateTime fecha, EurodreamsDAO eurodreams)
        {
            var query = "Update eurodreams_res set fecha=@date, combinacion=@combi, suenho=@rein where fecha=@fecha2";

            var combin = "";
            for (int i = 0; i < eurodreams.combinacion.Length; i++)
            {
                if (i == eurodreams.combinacion.Length - 1)
                {
                    combin = combin + eurodreams.combinacion[i];
                }
                else
                {
                    combin = combin + eurodreams.combinacion[i] + ",";
                }

            }

            await _db.QueryAsync(query, new { date = eurodreams.fecha, combi = combin,  rein = eurodreams.suenho, fecha2 = fecha });

            return NoContent();
        }

        // POST: api/eurodreamses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Eurodreams>> PostEurodreams(EurodreamsDAO eurodreams)
        {
            var combin = "";
            for (int i = 0; i < eurodreams.combinacion.Length; i++)
            {
                if (i == eurodreams.combinacion.Length - 1)
                {
                    combin = combin + eurodreams.combinacion[i];
                }
                else
                {
                    combin = combin + eurodreams.combinacion[i] + ",";
                }

            }
            var query = @"Insert into eurodreams_res values (@fechaID, @combi, @numero)";

            await _db.QueryAsync(query, new { fechaID = eurodreams.fecha, combi = combin, numero = eurodreams.suenho});

            return NoContent();
        }

        // DELETE: api/eurodreamses/5
        [HttpDelete("{fecha}")]
        public async Task<ActionResult> deleteEurodreams(DateTime fecha)
        {
            var query = "Delete from eurodreams_res where fecha=@date";

            await _db.QueryAsync(query, new { date=fecha });

            return NoContent();
        }

        private int[] stringAInt(string s)
        {
            string[] strings = s.Split(',');
            int[] ints = new int[strings.Length];

            for (int i = 0; i < strings.Length; i++)
            {
                ints[i] = int.Parse(strings[i]);
            }

            return ints;
        }
    }
}
