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
    public class LototurfController : ControllerBase
    {
        private readonly MySqlConnection _db;

        public LototurfController()
        {
            _db = ConexionMySQL.getConexion();
        }

        // GET: api/lototurfes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LototurfDAO>>> GetLototurfAll()
        {
            var query = @"Select * from lototurf_res order by fecha desc";

            IEnumerable<Lototurf> result = await _db.QueryAsync<Lototurf>(query);
            if (result == null)
            {
                return NoContent();
            }
            List<Lototurf> resultList = result.AsList();

            List<LototurfDAO> DaoList = new List<LototurfDAO>();

            foreach (var item in resultList)
            {
                LototurfDAO aux = new LototurfDAO();

                aux.fecha = item.fecha;
                aux.jornada = item.jornada;
                aux.reintegro = item.reintegro;
                aux.caballo = item.caballo;
                aux.combinacion = stringAInt(item.combinacion);
                DaoList.Add(aux);
            }

            return DaoList;
        }

        // GET: api/lototurfes/5
        [HttpGet("{fecha}")]
        public async Task<ActionResult<LototurfDAO>> GetLototurf(DateTime fecha)
        {
            var query = @"Select * from lototurf_res where fecha=@date";

            Lototurf result = await _db.QueryFirstOrDefaultAsync<Lototurf>(query, new { date = fecha });
            if (result == null)
            {
                return NoContent();
            }
            LototurfDAO DaoList = new LototurfDAO();

            DaoList.fecha = result.fecha;
            DaoList.jornada = result.jornada;
            DaoList.reintegro = result.reintegro;
            DaoList.caballo = result.caballo;
            DaoList.combinacion = stringAInt(result.combinacion);

            return DaoList;
        }

        // PUT: api/lototurfes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{fecha}")]
        public async Task<IActionResult> PutLototurf(DateTime fecha, LototurfDAO lototurf)
        {
            var query = "UPDATE lototurf_res SET fecha=@date, jornada=@jorn, combinacion=@combi, caballo=@comp, reintegro=@rein WHERE fecha=@fecha2;";
            var combin = "";
            for (int i = 0; i < lototurf.combinacion.Length; i++)
            {
                if (i == lototurf.combinacion.Length - 1)
                {
                    combin = combin + lototurf.combinacion[i];
                }
                else
                {
                    combin = combin + lototurf.combinacion[i] + ",";
                }
            }
            await _db.QueryAsync(query, new { date = lototurf.fecha, jorn=lototurf.jornada, combi = combin, comp = lototurf.caballo, rein = lototurf.reintegro, fecha2 = fecha });

            return NoContent();
        }

        // POST: api/lototurfes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lototurf>> PostLototurf(LototurfDAO lototurf)
        {
            var query = @"Insert into lototurf_res values (@fechaID, @jorn, @combi, @comp, @rein)";
            var combin = "";
            for (int i = 0; i < lototurf.combinacion.Length; i++)
            {
                if (i == lototurf.combinacion.Length - 1)
                {
                    combin = combin + lototurf.combinacion[i];
                }
                else
                {
                    combin = combin + lototurf.combinacion[i] + ",";
                }
            }

            await _db.QueryAsync(query, new { fechaID = lototurf.fecha, jorn=lototurf.jornada, combi = combin, comp = lototurf.caballo, rein = lototurf.reintegro});

            return NoContent();
        }

        // DELETE: api/lototurfes/5
        [HttpDelete("{fecha}")]
        public async Task<ActionResult> deleteLototurf(DateTime fecha)
        {
            var query = "Delete from lototurf_res where fecha=@date";

            await _db.QueryAsync(query, new { date = fecha });

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
