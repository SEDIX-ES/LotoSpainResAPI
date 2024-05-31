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
    public class EuromillonesController : ControllerBase
    {
        private readonly MySqlConnection _db;

        public EuromillonesController()
        {
            _db = ConexionMySQL.getConexion();
        }

        // GET: api/Euromilloneses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EuromillonesDAO>>> GetEuromillonesAll()
        {
            var query = @"Select * from euromillones_res order by fecha desc";
            IEnumerable<Euromillones> result = await _db.QueryAsync<Euromillones>(query);
            if (result == null)
            {
                return NoContent();
            }
            List<Euromillones> resultList = result.AsList();

            List<EuromillonesDAO> DaoList = new List<EuromillonesDAO>();

            foreach (var item in resultList)
            {
                EuromillonesDAO aux = new EuromillonesDAO();

                aux.fecha = item.fecha;
                aux.estrellas= stringAInt(item.estrellas);
                aux.combinacion = stringAInt(item.combinacion);
                DaoList.Add(aux);
            }

            return DaoList;
        }

        // GET: api/Euromilloneses/5
        [HttpGet("{fecha}")]
        public async Task<ActionResult<EuromillonesDAO>> GetEuromillones(DateTime fecha)
        {
            var query = @"Select * from euromillones_res where fecha=@date";
            Euromillones result = await _db.QueryFirstOrDefaultAsync<Euromillones>(query, new { date = fecha });
            if (result == null)
            {
                return NoContent();
            }
            EuromillonesDAO aux = new EuromillonesDAO();

            aux.fecha = result.fecha;
            aux.estrellas = stringAInt(result.estrellas);
            aux.combinacion = stringAInt(result.combinacion);

            return aux;
        }

        // PUT: api/Euromilloneses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{fecha}")]
        public async Task<IActionResult> PutEuromillones(DateTime fecha, EuromillonesDAO euromillones)
        {
            var query = "Update euromillones_res set fecha=@date, combinacion=@combi, estrellas=@rein where fecha=@fecha2";

            var combin = "";
            for (int i = 0; i < euromillones.combinacion.Length; i++)
            {
                if (i == euromillones.combinacion.Length - 1)
                {
                    combin = combin + euromillones.combinacion[i];
                }
                else
                {
                    combin = combin + euromillones.combinacion[i] + ",";
                }
            }

            var stars = euromillones.estrellas[0] + "," + euromillones.estrellas[1];

            await _db.QueryAsync(query, new { date = euromillones.fecha, combi = combin,  rein = stars, fecha2 = fecha });

            return NoContent();
        }

        // POST: api/Euromilloneses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Euromillones>> PostEuromillones(EuromillonesDAO euromillones)
        {
            var combin = "";
            for (int i = 0; i < euromillones.combinacion.Length; i++)
            {
                if (i == euromillones.combinacion.Length - 1)
                {
                    combin = combin + euromillones.combinacion[i];
                }
                else
                {
                    combin = combin + euromillones.combinacion[i] + ",";
                }

            }
            var stars = euromillones.estrellas[0] + "," + euromillones.estrellas[1];


            var query = @"Insert into euromillones_res values (@fechaID, @combi, @star)";

            await _db.QueryAsync(query, new { fechaID = euromillones.fecha, combi = combin, star = stars});

            return NoContent();
        }

        // DELETE: api/Euromilloneses/5
        [HttpDelete("{fecha}")]
        public async Task<ActionResult> deleteEuromillones(DateTime fecha)
        {
            var query = "Delete from euromillones_res where fecha=@date";

            await _db.QueryAsync(query, new { date= fecha });

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
