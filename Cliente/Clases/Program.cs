using Cliente.Clases;
using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;

class Program
{
    // Establecer la dirección IP y el puerto del servidor
    public static string serverIp = "127.0.0.1";
    public static int serverPort = 8080;

    // Crear el objeto TcpClient y conectar al servidor
    public static TcpClient client = null; //new TcpClient(serverIp, serverPort);

    static ConexionBD conexionBD = new ConexionBD(); //Crear conexionBD desde clase ConexionBD
    
    static void Main()
    {
        conexionBD.conectarDB();
        iniciarSesion();


    }

    static void iniciarSesion()
    {
        string masterKey = "ClaveMaestraSecreta";

        string consultaSalt1 = "select ISNULL(contraseniaSalt, 0) from Usuario where id=1";


        SqlCommand comandoSalt = new SqlCommand(consultaSalt1, conexionBD.conectBD);

        string salt = comandoSalt.ExecuteScalar().ToString();


        string consultaHash1 = "select ISNULL(contraseniaHashed, 0) from Usuario where id=1";

        SqlCommand comandoHash = new SqlCommand(consultaHash1, conexionBD.conectBD);

        string hashedPassword = comandoHash.ExecuteScalar().ToString();

        //Console.WriteLine("Salt en BD " + salt);
        //Console.WriteLine("Hash en BD " + hashedPassword);

        byte[] saltb = Convert.FromBase64String(salt); // R

        Console.Write("Ingresa tu contraseña:");
        
        string userInputPassword;

        userInputPassword = Console.ReadLine();

        bool isLoginSuccessful = VerifyPassword(userInputPassword, saltb, hashedPassword, masterKey);

        if (isLoginSuccessful)
        {
            Console.WriteLine("Inicio de sesión exitoso.");
            
            conectarServer();
        }
        else
        {
            Console.WriteLine("Inicio de sesión fallido.");

            iniciarSesion();
        }
    }

    //Hashea la contraseña ingresada
    static string HashPassword(string password, byte[] salt, string masterKey)
    {
        int iterations = 10000;
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
        {
            byte[] hashBytes = pbkdf2.GetBytes(32);
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(masterKey)))
            {
                byte[] masterKeyBytes = hmac.ComputeHash(hashBytes);
                return Convert.ToBase64String(masterKeyBytes);
            }
        }
    }

    // Verificar la contraseña ingresada utilizando la clave maestra
    static bool VerifyPassword(string userInputPassword, byte[] salt, string hashedPassword, string masterKey)
    {
        string hashedInputPassword = HashPassword(userInputPassword, salt, masterKey);
        return hashedInputPassword == hashedPassword;
    }


    static void conectarServer()
    {
        try
        {
            client = new TcpClient(serverIp, serverPort);

            Console.WriteLine("Cliente conectado al servidor.");
            subirArchivo();
        }
        catch (Exception)
        {
            Console.WriteLine("Servidor apagado o fuera de servicio.");
            Console.WriteLine("Verifique estado y presione una tecla para continuar");
            Console.ReadKey();
            conectarServer();
        }
        
    }
    
    static void subirArchivo()
    {
        // Obtener la secuencia de salida del cliente
        NetworkStream stream = client.GetStream();

        // Solicitar al usuario el nombre del archivo a enviar
        Console.Write("Ingrese el nombre del archivo a enviar: ");
        string fileNameToSend = Console.ReadLine();
        //string fileNameToSend = "Archivo_1.txt";


        // Enviar el nombre del archivo al servidor
        byte[] fileNameData = Encoding.ASCII.GetBytes(fileNameToSend);
        stream.Write(fileNameData, 0, fileNameData.Length);

        // Leer el archivo y enviarlo al servidor
        using (FileStream fileStream = File.OpenRead(fileNameToSend))
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                stream.Write(buffer, 0, bytesRead);
            }
            Console.WriteLine("EL archivo paso ok");
        }
        client.Close();

        Console.WriteLine("Quere volver a pasar un archivo? (y/n)");
        var ingreso = Console.ReadLine();

        if (ingreso == "yes" || ingreso == "YES" || ingreso == "y") {
            conectarServer();
            subirArchivo();
        }
        else
        {
            Console.WriteLine("Se cierra la conexión con el servidor.");
            // Cerrar la conexión
            client.Close();
        }
            

        
        Console.ReadLine();
    }
}
