using System;
using System.Data.SqlClient;


namespace Servidor
{
    public class ConnSingleton
    {

        private static ConnSingleton dbInstance;
        private readonly SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-TCDL3NA\SQLEXPRESS;Initial Catalog=Personas; Integrated Security=true");

        private ConnSingleton()
        {
        }

        public static ConnSingleton getDbInstance()
        {
            if (dbInstance == null)
            {
                dbInstance = new ConnSingleton();
            }

            return dbInstance;
        }

        public SqlConnection GetDBConnection()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Connected");
            }
            catch (SqlException e)
            {
                Utils.printClassError("ConnSingleton", e);
            }
            return conn;
        }
    }

    //public static void Main(string[] args)
    //{
    //    ConnSingleton cs = ConnSingleton.getDbInstance();
    //    cs.GetDBConnection();
    //    Console.WriteLine("Connection established");
    //}
}
