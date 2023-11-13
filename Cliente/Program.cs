using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

class Program
{
    // Establecer la dirección IP y el puerto del servidor
    public static string serverIp = "127.0.0.1";
    public static int serverPort = 8080;

    // Crear el objeto TcpClient y conectar al servidor
   // public static TcpClient client = null; //new TcpClient(serverIp, serverPort);

    static void Main()
    {
        iniciarSesion();


    }

    static void iniciarSesion()
    {
        string masterKey = "ClaveMaestraSecreta";

        // Genera una contraseña y una sal (salt) aleatoria para el usuario
        string password = "ContraseñaSecreta";
        byte[] salt = GenerateSalt();
        Console.WriteLine(Convert.ToBase64String(salt));


        // Hashea la contraseña usando la clave maestra y luego guarda el hash en la base de datos
        string hashedPassword = HashPassword(password, salt, masterKey);
        Console.WriteLine(hashedPassword);

        // Simula un intento de inicio de sesión

        Console.Write("Ingrese la contraseña: ");
        string userInputPassword;

        userInputPassword = Console.ReadLine();

        //string userInputPassword = "ContraseñaSecreta"; // Cambia esto a la contraseña correcta para probar
        bool isLoginSuccessful = VerifyPassword(userInputPassword, salt, hashedPassword, masterKey);

        if (isLoginSuccessful)
        {
            conectarServer();
            //subirArchivo();
        }
        else
        {
            Console.WriteLine("Inicio de sesión fallido.");
            iniciarSesion();

        }
    }

    static byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    /* Derivación de Clave con PBKDF2:

        La función Rfc2898DeriveBytes se utiliza para derivar una clave secreta a partir de la contraseña del usuario (password) y la sal (salt).
        iterations indica la cantidad de veces que se aplicará el algoritmo PBKDF2 para hacer que el proceso de derivación sea más costoso y, por lo tanto, más seguro contra ataques de fuerza bruta. En este caso, se utiliza 10,000 iteraciones, pero este valor puede ajustarse según las necesidades de seguridad.

    Hash de la Clave Derivada con HMAC-SHA256:

        Se crea una instancia de HMACSHA256 utilizando la clave maestra (masterKey) como clave secreta. HMAC-SHA256
    es un algoritmo de hash seguro.
        La clave derivada obtenida en el paso anterior (hashBytes) se utiliza como entrada para este algoritmo de hash.

    Conversión a Base64:

        El resultado del cálculo del hash en el paso 2 es un conjunto de bytes. Para almacenarlo y compararlo de manera más
    conveniente, se convierte en una cadena Base64 utilizando Convert.ToBase64String. Esta cadena Base64 es lo que se almacena 
    en la base de datos para verificar contraseñas en futuros intentos de inicio de sesión.
    
    La combinación de PBKDF2 y HMAC-SHA256 junto con la clave maestra aumenta significativamente la seguridad de las contraseñas
    almacenadas, ya que hace que sea extremadamente difícil para un atacante recuperar la contraseña original,
    incluso si obtienen acceso a la base de datos y a la sal. Además, la clave maestra actúa como una capa adicional de seguridad.
    */
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

    // Verifica la contraseña ingresada utilizando la clave maestra
    static bool VerifyPassword(string userInputPassword, byte[] salt, string hashedPassword, string masterKey)
    {
        string hashedInputPassword = HashPassword(userInputPassword, salt, masterKey);
        return hashedInputPassword == hashedPassword;
    }



    static void conectarServer()
    {
         IPHostEntry host;
         IPAddress addr;
         IPEndPoint endPoint;
         Socket socket;
        // Establecer la dirección IP y el puerto del servidor
        //string serverIp = "127.0.0.1";
        //int serverPort = 8080;
        // Crear el objeto TcpClient y conectar al servidor
        //client = new TcpClient(serverIp, serverPort);

        try
        {
            host = Dns.GetHostEntry("localhost");
            addr = host.AddressList[0];
            endPoint = new IPEndPoint(addr, 8080);
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //Se conecta al socket
            socket.Connect(endPoint);

            subirArchivo(socket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            Console.ReadKey();
        }


        Console.WriteLine("Cliente conectado al servidor.");
    }
    
    static void subirArchivo(Socket client)
    {
        // Obtener la secuencia de salida del cliente
        NetworkStream stream = new NetworkStream(client, true);//client.GetStream();

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

        stream.Close();

        Console.WriteLine("Quere volver a pasar un archivo? (y/n)");
        var ingreso = Console.ReadLine();

        if (ingreso == "yes" || ingreso == "YES" || ingreso == "y") {
            conectarServer();
            //subirArchivo(clie);
        }
        else
        {
            Console.WriteLine("Se cierra la conexión con el servidor.");
            // Cerrar la conexión
          //  client.Close();
        }
            

        // Leer la respuesta del servidor
        //byte[] responseBuffer = new byte[1024];
        //int responseBytes = stream.Read(responseBuffer, 0, responseBuffer.Length);
        //string responseMessage = Encoding.ASCII.GetString(responseBuffer, 0, responseBytes);
        //Console.WriteLine($"Respuesta del servidor: {responseMessage}");





        Console.ReadLine();
    }
}
