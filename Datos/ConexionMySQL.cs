using Microsoft.CodeAnalysis.CSharp.Syntax;
using MySql.Data.MySqlClient;

namespace LotoSpain_API.Datos
{
    public class ConexionMySQL
    {
        private string cadenaConexion = "server=hl997.dinaserver.com;port=3306;database=lotospain_ResYBotes;uid=lotospain;password=LotoSpain2024!;";
        private static MySqlConnection conexion { get; set; }
        private ConexionMySQL() {
            conexion=new MySqlConnection(cadenaConexion);
        }
        
        public static MySqlConnection getConexion()
        {
            if (conexion == null)
            {
                new ConexionMySQL();
            }
            return conexion;
        }

        public static void cerrarConexion()
        {
            try{
                conexion.Close();
            }catch (MySqlException ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
