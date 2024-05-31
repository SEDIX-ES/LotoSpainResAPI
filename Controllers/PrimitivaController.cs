using Microsoft.AspNetCore.Mvc;
using LotoSpain_API.Datos;
using LotoSpain_API.Modelos;
using Dapper;
using LotoSpain_API.Modelos.DAO;
using MySql.Data.MySqlClient;

namespace LotoSpain_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrimitivaController : ControllerBase
    {
        private readonly MySqlConnection _db;

        public PrimitivaController()
        {
            _db = ConexionMySQL.getConexion();
        }

        // GET: api/primitivaes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrimitivaDAO>>> GetPrimitivaAll()
        {
            var query = @"Select * from primitiva_res order by fecha desc";

            IEnumerable<Primitiva> result = await _db.QueryAsync<Primitiva>(query);
            if (result == null)
            {
                return NoContent();
            }
            List<Primitiva> resultList = result.AsList();

            List<PrimitivaDAO> DaoList = new List<PrimitivaDAO>();

            foreach (var item in resultList)
            {
                PrimitivaDAO aux = new PrimitivaDAO();

                aux.fecha = item.fecha;
                aux.complementario = item.complementario;
                aux.reintegro = item.reintegro;
                aux.combinacion = stringAInt(item.combinacion);
                DaoList.Add(aux);
            }

            return DaoList;
        }

        // GET: api/primitivaes/5
        [HttpGet("{fecha}")]
        public async Task<ActionResult<PrimitivaDAO>> GetPrimitiva(DateTime fecha)
        {
            var query = @"Select * from primitiva_res where fecha=@date order by id asc";

            Primitiva result = await _db.QueryFirstOrDefaultAsync<Primitiva>(query, new { date = fecha });
            if (result == null)
            {
                return NoContent();
            }
            PrimitivaDAO DaoList = new PrimitivaDAO();

            DaoList.fecha = result.fecha;
            DaoList.complementario = result.complementario;
            DaoList.reintegro = result.reintegro;
            DaoList.combinacion = stringAInt(result.combinacion);

            return DaoList;
        }

        // PUT: api/primitivaes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{fecha}")]
        public async Task<IActionResult> PutPrimitiva(DateTime fecha, PrimitivaDAO primitiva)
        {
            var query = "UPDATE primitiva_res SET combinacion=@combi, complementario=@comp, reintegro=@rein WHERE fecha=@fecha2;";
            
            var combin = "";
            for (int i = 0; i < primitiva.combinacion.Length; i++)
            {
                if (i == primitiva.combinacion.Length - 1)
                {
                    combin = combin + primitiva.combinacion[i];
                }
                else
                {
                    combin = combin + primitiva.combinacion[i] + ",";
                }
            }

            await _db.QueryAsync(query, new { combi = combin, comp=primitiva.complementario, rein = primitiva.reintegro, fecha2 = fecha });

            return NoContent();
        }

        // POST: api/primitivaes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Primitiva>> PostPrimitiva(PrimitivaDAO primitiva)
        {
            var query = @"Insert into primitiva_res values (@fechaID, @combi, @comp, @rein)";

            var combin = "";
            for (int i = 0; i < primitiva.combinacion.Length; i++)
            {
                if (i == primitiva.combinacion.Length - 1)
                {
                    combin = combin + primitiva.combinacion[i];
                }
                else
                {
                    combin = combin + primitiva.combinacion[i] + ",";
                }
            }

            await _db.QueryAsync(query, new { fechaID = primitiva.fecha, combi = combin, comp = primitiva.complementario, rein=primitiva.reintegro});

            return NoContent();
        }

        // DELETE: api/primitivaes/5
        [HttpDelete("{fecha}")]
        public async Task<ActionResult> deletePrimitiva(DateTime fecha)
        {
            var query = "Delete from primitiva_res where fecha=@date";

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
