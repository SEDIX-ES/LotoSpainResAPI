using Microsoft.AspNetCore.Mvc;
using LotoSpain_API.Datos;
using LotoSpain_API.Modelos;
using MySql.Data.MySqlClient;
using Dapper;
using LotoSpain_API.Modelos.DAO;

namespace LotoSpain_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonolotoController : ControllerBase
    {
        private readonly MySqlConnection _db;

        public BonolotoController()
        {
            _db = ConexionMySQL.getConexion();
        }

        // GET: api/Bonolotoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BonolotoDAO>>> GetBonolotoAll()
        {
            var query = @"Select * from bonoloto_res order by fecha desc";
            IEnumerable<Bonoloto> result =await _db.QueryAsync<Bonoloto>(query);
            if (result == null)
            {
                return NoContent();
            }
            List<Bonoloto> resultList=result.AsList();

            List<BonolotoDAO> DaoList= new List<BonolotoDAO>();

            foreach (var item in resultList)
            {
                BonolotoDAO aux = new BonolotoDAO();

                aux.fecha = item.fecha;
                aux.reintegro = item.reintegro;
                aux.complementario = item.complementario;
                aux.combinacion = stringAInt(item.combinacion);
                DaoList.Add(aux);
            }
          
            return DaoList;
        }

        // GET: api/Bonolotoes/5
        [HttpGet("{fecha}")]
        public async Task<ActionResult<BonolotoDAO>> GetBonoloto(DateTime fecha)
        {
            var query = @"Select * from bonoloto_res where fecha=@date";
            Bonoloto result = await _db.QueryFirstOrDefaultAsync<Bonoloto>(query, new {date=fecha});
            if (result == null)
            {
                return NoContent();
            }
            BonolotoDAO DaoList = new BonolotoDAO();

            DaoList.fecha = result.fecha;
            DaoList.reintegro = result.reintegro;
            DaoList.complementario = result.complementario;
            DaoList.combinacion = stringAInt(result.combinacion);

            return DaoList;
        }

        // PUT: api/Bonolotoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{fecha}")]
        public async Task<IActionResult> PutBonoloto(DateTime fecha, BonolotoDAO bonoloto)
        {
            var query = "Update bonoloto_res set fecha=@date, combinacion=@combi, complementario=@comp, reintegro=@rein where fecha=@fecha2";

            var combin = "";
            for (int i = 0; i < bonoloto.combinacion.Length; i++)
            {
                if (i == bonoloto.combinacion.Length - 1)
                {
                    combin = combin + bonoloto.combinacion[i];
                }
                else
                {
                    combin = combin + bonoloto.combinacion[i] + ",";
                }
            }

            await _db.QueryAsync(query, new {date=bonoloto.fecha, combi=combin, comp=bonoloto.complementario, rein=bonoloto.reintegro, fecha2=fecha});
            
            return NoContent();
        }

        // POST: api/Bonolotoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bonoloto>> PostBonoloto(BonolotoDAO bonoloto)
        {
            var combin="";
            for(int i = 0; i < bonoloto.combinacion.Length; i++)
            {
                if (i == bonoloto.combinacion.Length - 1)
                {
                    combin= combin + bonoloto.combinacion[i];
                }
                else
                {
                    combin = combin + bonoloto.combinacion[i]+ ",";
                }

            }
            var query = @"Insert into bonoloto_res values (@fechaID, @combi, @comp, @rein)";

            await _db.QueryAsync(query, new { fechaID = bonoloto.fecha, combi = combin, comp = bonoloto.complementario, rein = bonoloto.reintegro});

            return NoContent();
        }

        // DELETE: api/Bonolotoes/5
        [HttpDelete("{fecha}")]
        public async Task<ActionResult> deletebonoloto(DateTime fecha)
        {
            var query =  "Delete from bonoloto_res where fecha=@date";

            await _db.QueryAsync(query,new {date=fecha});

            return NoContent();
        }

        private int[] stringAInt(string s)
        {
            string[] strings = s.Split(',');
            int[] ints = new int[strings.Length];

            for(int i=0; i < strings.Length; i++)
            {
                ints[i] = int.Parse(strings[i]);
            }

            return ints;
        }
    }
}
