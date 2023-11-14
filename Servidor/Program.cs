using Servidor;
using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        try
        {
            Server server = new Server();
            Console.ReadKey();
        }
        catch (Exception e)
        {
            Utils.printClassError("Main", e);
        }

    }
}
