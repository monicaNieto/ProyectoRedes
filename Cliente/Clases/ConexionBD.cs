using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Cliente.Clases
{
    internal class ConexionBD
    {
        public SqlConnection conectBD = new SqlConnection();

        string stringconexion = "Data Source=NP300\\SQLEXPRESS;Initial Catalog=Personas; Integrated Security=true";

        public SqlConnection conectarDB()
        {
            try
            {
                conectBD.ConnectionString = stringconexion;

                conectBD.Open();

                Console.WriteLine("Conexión a Base de Datos exitosa");
            }
            catch (SqlException ex)
            {

                Console.WriteLine("No se pudió conectar a la Base de Datos", ex.ToString());
            }
            return conectBD;
        }
    }
}
