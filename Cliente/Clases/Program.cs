using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using Servidor;

class Program
{
    // Establecer la dirección IP y el puerto del servidor
    public static string serverIp = "127.0.0.1";
    public static int serverPort = 8080;

    // Crear el objeto TcpClient y conectar al servidor
    public static TcpClient client = null; //new TcpClient(serverIp, serverPort);

    static void Main()
    {

        if (Servidor.ServicesDB.iniciarSesion())
        {
            conectarServer();
        }else
        {
            Servidor.ServicesDB.iniciarSesion();
        }

    }

    static void conectarServer()
    {
        IPHostEntry host;
        IPAddress addr;
        IPEndPoint endPoint;
        Socket socket;

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

        if (ingreso == "yes" || ingreso == "YES" || ingreso == "y")
        {
            conectarServer();
            //subirArchivo(clie);
        }
        else
        {
            Console.WriteLine("Se cierra la conexión con el servidor.");
            // Cerrar la conexión
            //  client.Close();
        }

        Console.ReadLine();
    }
}
