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
    public class GordoController : ControllerBase
    {
        private readonly MySqlConnection _db;

        public GordoController()
        {
            _db = ConexionMySQL.getConexion();
        }

        // GET: api/gordoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GordoDAO>>> GetGordoAll()
        {
            var query = @"Select * from gordo_res order by fecha desc";

            IEnumerable<Gordo> result = await _db.QueryAsync<Gordo>(query);
            if (result == null)
            {
                return NoContent();
            }
            List<Gordo> resultList = result.AsList();

            List<GordoDAO> DaoList = new List<GordoDAO>();

            foreach (var item in resultList)
            {
                GordoDAO aux = new GordoDAO();

                aux.fecha = item.fecha;
                aux.clave = item.clave;
                aux.combinacion = stringAInt(item.combinacion);
                DaoList.Add(aux);
            }

            return DaoList;
        }

        // GET: api/gordoes/5
        [HttpGet("{fecha}")]
        public async Task<ActionResult<GordoDAO>> GetGordo(DateTime fecha)
        {
            var query = @"Select * from gordo_res where fecha=@date";

            Gordo result = await _db.QueryFirstOrDefaultAsync<Gordo>(query, new { date = fecha });
            if (result == null)
            {
                return NoContent();
            }
            GordoDAO DaoList = new GordoDAO();

            DaoList.fecha = result.fecha;
            DaoList.clave = result.clave;
            DaoList.combinacion = stringAInt(result.combinacion);

            return DaoList;
        }

        // PUT: api/gordoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{fecha}")]
        public async Task<IActionResult> PutGordo(DateTime fecha, GordoDAO gordo)
        {
            var query = "Update gordo_res set fecha=@date, combinacion=@combi, clave=@rein where fecha=@fecha2";
            var combin = "";
            for (int i = 0; i < gordo.combinacion.Length; i++)
            {
                if (i == gordo.combinacion.Length - 1)
                {
                    combin = combin + gordo.combinacion[i];
                }
                else
                {
                    combin = combin + gordo.combinacion[i] + ",";
                }
            }
            await _db.QueryAsync(query, new { date = gordo.fecha, combi = combin, rein = gordo.clave, fecha2 = fecha });

            return NoContent();
        }

        // POST: api/gordoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Gordo>> PostGordo(GordoDAO gordo)
        {
            var query = @"Insert into gordo_res values (@fechaID, @combi, @numero)";

            var combin = "";
            for (int i = 0; i < gordo.combinacion.Length; i++)
            {
                if (i == gordo.combinacion.Length - 1)
                {
                    combin = combin + gordo.combinacion[i];
                }
                else
                {
                    combin = combin + gordo.combinacion[i] + ",";
                }
            }

            await _db.QueryAsync(query, new { fechaID = gordo.fecha, combi = combin, numero = gordo.clave});

            return NoContent();
        }

        // DELETE: api/gordoes/5
        [HttpDelete("{fecha}")]
        public async Task<ActionResult> deleteGordo(DateTime fecha)
        {
            var query = "Delete from gordo_res where fecha=@date";

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
