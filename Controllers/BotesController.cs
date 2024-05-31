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
using System.Globalization;

namespace LotoSpain_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotesController : ControllerBase
    {
        private readonly MySqlConnection _db;

        public BotesController()
        {
            _db = ConexionMySQL.getConexion();
        }

        // GET: api/Boteses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoteDAO>>> GetBotesAll()
        {
            var query = @"Select * from botes order by fk_sorteo asc";
            IEnumerable<Bote> result = await _db.QueryAsync<Bote>(query);
            List<Bote> resultList = result.AsList();
            IEnumerable<String> sorteos = await _db.QueryAsync<String>("Select nombre from sorteos order by id asc");
            List<String> sorteoList = sorteos.AsList();

            List<BoteDAO> DaoList = new List<BoteDAO>();

            foreach (var item in resultList)
            {
                var aux = new BoteDAO();
                aux.fecha = item.fecha;
                aux.id = item.id;
                aux.cantidad = item.cantidad;
                aux.sorteo = sorteoList[item.fk_sorteo-1];
                DaoList.Add(aux);
            }

            return DaoList;
        }
        [HttpGet("Ultimos")]
        public async Task<ActionResult<IEnumerable<Cantidad>>> GetUltimosBotes()
        {
            var query = @"SELECT s.nombre, b.cantidad, b.fecha FROM sorteos s INNER JOIN (SELECT b1.* FROM botes b1 INNER JOIN (SELECT fk_sorteo, MAX(fecha) AS ultima_fecha FROM botes GROUP BY fk_sorteo) b2 ON b1.fk_sorteo = b2.fk_sorteo AND b1.fecha = b2.ultima_fecha) b ON s.id = b.fk_sorteo order by s.id;";
            IEnumerable<Cantidad> result = await _db.QueryAsync<Cantidad>(query);
            List<Cantidad> resultList = result.AsList();

            List<Cantidad> DaoList = new List<Cantidad>();

            foreach (var item in resultList)
            {
                var aux = new Cantidad();
                aux.nombre = item.nombre;
                aux.fecha = item.fecha;
                aux.cantidad = item.cantidad;
               
                DaoList.Add(aux);
            }

            return DaoList;
        }

        // GET: api/Boteses/Bonoloto
        [HttpGet("{sorteo}")]
        public async Task<ActionResult<IEnumerable<BoteDAO>>> GetBotesSorteo(string sorteo)
        {   
            var query = @"Select botes.id, botes.fecha, botes.cantidad, sorteos.nombre as sorteo, botes.jornada from botes join sorteos on botes.fk_sorteo=sorteos.id where sorteos.nombre=@nombre order by fecha desc";
            IEnumerable<BoteDAO> result = await _db.QueryAsync<BoteDAO>(query, new {nombre=sorteo});
            List<BoteDAO> resultList = result.AsList();

            return resultList;
        }

        // GET: api/Boteses/Nacional
        [HttpGet("Nacional")]
        public async Task<ActionResult<IEnumerable<BoteNacionalDAO>>> GetBotesNacional()
        {
            var query = @"Select botes.id, botes.fecha, botes.cantidad, sorteos.nombre as sorteo, nac.tipo as tipo, nac.id as id_nacional, nac.nombre_extra as nombreExtra from bote_nacional as nac join botes on nac.fk_bote=botes.id  join sorteos on botes.fk_sorteo=sorteos.id where sorteos.nombre='Nacional' order by fecha desc";
            IEnumerable<BoteNacionalDAO> result = await _db.QueryAsync<BoteNacionalDAO>(query);
            List<BoteNacionalDAO> resultList = result.AsList();


            return resultList;
        }


        //GET: api/Boteses/5
        [HttpGet("{sorteo}/{fecha}")]
        public async Task<ActionResult<BoteDAO>> GetBoteFecha(DateTime fecha, string sorteo)
        {
            var query = @"Select botes.id, botes.fecha, botes.cantidad, sorteos.nombre as sorteo, botes.jornada from botes join sorteos on botes.fk_sorteo=sorteos.id where sorteos.nombre=@nombre and botes.fecha=@data";
            BoteDAO result = await _db.QueryFirstOrDefaultAsync<BoteDAO>(query, new { nombre = sorteo, data=fecha });
            
            return result;
        }

        //GET: api/Boteses/5
        [HttpGet("Nacional/{fecha}")]
        public async Task<ActionResult<BoteNacionalDAO>> GetNacionalFecha(DateTime fecha)
        {
            var query = @"Select botes.id as id, botes.fecha, botes.cantidad, sorteos.nombre as sorteo, nac.id as id_nacional, nac.tipo as tipo, nac.nombre_extra as nombreExtra, nac.id as id_nacional from bote_nacional as nac join botes on nac.fk_bote=botes.id  join sorteos on botes.fk_sorteo=sorteos.id where sorteos.nombre='Nacional' and botes.fecha=@data";
            BoteNacionalDAO result = await _db.QueryFirstOrDefaultAsync<BoteNacionalDAO>(query,new {data=fecha});

            return result;
        }

        // PUT: api/Boteses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBotes(Bote bote)
        {
            var query = "UPDATE botes SET cantidad=@cant, fecha=@date, jornada=@jorn WHERE id=@ID;";
            await _db.QueryAsync(query, new {cant=bote.cantidad, date=bote.fecha, jorn=bote.jornada, ID=bote.id});

            return NoContent();
        }

        // PUT: api/Boteses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Nacional")]
        public async Task<IActionResult> PutBotesNacional(BoteNacionalDAO boteNacional)
        {

            var queryBote = "UPDATE botes SET cantidad=@cant, fecha=@date WHERE id=@ID;";
            await _db.QueryAsync(queryBote, new { cant = boteNacional.cantidad, date = boteNacional.fecha, ID = boteNacional.id });

            var queryNacional = "Update bote_nacional SET tipo=@type, nombre_extra=@extra where id=@ID";
            await _db.QueryAsync(queryNacional, new {type=boteNacional.tipo, extra=boteNacional.nombreExtra, ID=boteNacional.id_nacional});

            return NoContent();
        }

        // POST: api/Boteses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bote>> PostBotes(BoteDAO bote)
        {
            var query = @"INSERT INTO botes (cantidad, fecha, jornada, fk_sorteo) VALUES (@cant, @date, @jorn, (select sorteos.id from sorteos where sorteos.nombre=@name));";

            await _db.QueryAsync(query, new {cant=bote.cantidad, date= bote.fecha, jorn=bote.jornada, name = bote.sorteo});

            return NoContent();
        }

        // POST: api/Boteses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Nacional")]
        public async Task<ActionResult<Object>> PostBoteNacional(BoteNacionalDAO bote)
        {
            var query = @"INSERT INTO botes (cantidad, fecha, fk_sorteo) VALUES (@cant, @date, (select sorteos.id from sorteos where sorteos.nombre='Nacional'));";

            await _db.QueryAsync(query, new { cant = bote.cantidad, date = bote.fecha, name = bote.sorteo });

            var queryNacional = @"INSERT INTO lotospain_ResYBotes.bote_nacional (tipo, nombre_extra, fk_bote) VALUES (@type, @extra, (select id from botes where fk_sorteo=(select id from sorteos where nombre='Nacional')order by id desc Limit 1));";

            await _db.QueryAsync(queryNacional, new {type=bote.tipo, extra=bote.nombreExtra});
            return NoContent();
        }
        
        // DELETE: api/Boteses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteBote(int id)
        {
            var query = "Delete from botes where id=@ID";

            await _db.QueryAsync(query, new { ID = id });

            return NoContent();
        }

        // DELETE: api/Boteses/5
        [HttpDelete("{id}/{id_nac}")]
        public async Task<ActionResult> deleteBoteNacional(int id, int id_nac)
        {
            var queryNac = @"Delete from bote_nacional where id=@idNac";

            await _db.QueryAsync(queryNac, new {idNac=id_nac});

            //var query = "Delete from botes where id=@ID";

            //await _db.QueryAsync(query, new { ID = id });

            await deleteBote(id);

            return NoContent();
        }
    }
}
