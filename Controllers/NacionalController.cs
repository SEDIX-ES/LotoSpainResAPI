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
using System.Runtime.Intrinsics.X86;

namespace LotoSpain_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NacionalController : ControllerBase
    {
        private readonly MySqlConnection _db;

        public NacionalController()
        {
            _db = ConexionMySQL.getConexion();
        }

        // GET: api/Nacionales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NacionalDAO>>> GetNacionalAll()
        {
            var query = @"Select * from nacional_res order by fecha desc";
            IEnumerable<Nacional> result = await _db.QueryAsync<Nacional>(query);
            if (result == null)
            {
                return NoContent();
            }
            List<Nacional> resultList = result.AsList();

            List<NacionalDAO> DaoList = new List<NacionalDAO>();

            foreach (var item in resultList)
            {
                NacionalDAO aux = new NacionalDAO();

                aux.fecha = item.fecha;
                aux.tipo = item.tipo;
                aux.combinacion1 = item.combinacion1;
                aux.combinacion2 = item.combinacion2;
                aux.reintegros = stringAInt(item.reintegros);
                aux.nombreExtra = item.nombreExtra;
                DaoList.Add(aux);
            }

            return DaoList;
        }

        // GET: api/Nacionales/5
        [HttpGet("{fecha}")]
        public async Task<ActionResult<NacionalDAO>> GetNacional(DateTime fecha)
        {
            var query = @"Select * from nacional_res where fecha=@date";
            Nacional result = await _db.QueryFirstOrDefaultAsync<Nacional>(query, new { date = fecha });
            if (result == null)
            {
                return NoContent();
            }
            NacionalDAO DaoList = new NacionalDAO();

            DaoList.fecha = result.fecha;
            DaoList.tipo = result.tipo;
            DaoList.combinacion1 = result.combinacion1;
            DaoList.combinacion2 = result.combinacion2;
            DaoList.reintegros = stringAInt(result.reintegros);
            DaoList.nombreExtra = result.nombreExtra;

            return DaoList;
        }

        // PUT: api/Nacionales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{fecha}")]
        public async Task<IActionResult> PutNacional(DateTime fecha, NacionalDAO nacional)
        {
            var query = "UPDATE nacional_res SET tipo=@type, combinacion1=@combi1, combinacion2=@combi2, reintegros=@rein, nombreExtra=@extra WHERE fecha=@fecha2;";
            
            var reintegros = nacional.reintegros[0] + "," + nacional.reintegros[1] + "," + nacional.reintegros[2];
            
            await _db.QueryAsync(query, new { type=nacional.tipo ,combi1 = nacional.combinacion1, combi2 = nacional.combinacion2, rein = reintegros, extra=nacional.nombreExtra, fecha2 = fecha });

            return NoContent();
        }

        // POST: api/Nacionales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Nacional>> PostNacional(NacionalDAO nacional)
        {
            var query = @"Insert into nacional_res values (@fechaID, @combi, @comp, @rein, @extra, @type)";
            string reintegros="";
            for (int i = 0; i < nacional.reintegros.Length; i++)
            {
                if (i != 2)
                {
                    reintegros += nacional.reintegros[i] +",";
                }
                else
                {
                    reintegros += nacional.reintegros[i];
                }
            }
            await _db.QueryAsync(query, new { fechaID = nacional.fecha, combi = nacional.combinacion1, comp = nacional.combinacion2, rein = reintegros, extra = nacional.nombreExtra, type = nacional.tipo });

            return NoContent();
        }

        // DELETE: api/Nacionales/5
        [HttpDelete("{fecha}")]
        public async Task<ActionResult> deletenacional(DateTime fecha)
        {
            var query = "Delete from nacional_res where fecha=@date";

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
