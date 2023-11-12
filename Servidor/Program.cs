using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    private static readonly string ServerDirectory = "ServerFiles";

    static void Main()
    {
        // Crear el directorio de archivos si no existe
        if (!Directory.Exists(ServerDirectory))
        {
            Directory.CreateDirectory(ServerDirectory);
        }

        TcpListener server = null;

        try
        {
            // Establecer la dirección IP y el puerto para el servidor
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8080;

            // Crear el objeto TcpListener
            server = new TcpListener(ipAddress, port);

            // Iniciar la escucha de conexiones
            server.Start();

            Console.WriteLine("Servidor esperando conexiones...");

            while (true)
            {
                // Aceptar la conexión del cliente
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Cliente conectado.");

                // Crear un hilo para manejar la conexión del cliente
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        finally
        {
            // Detener el servidor (esto no se ejecutará en este ejemplo)
            server?.Stop();
        }
    }

    static void HandleClient(TcpClient client)
    {
        try
        {
            // Obtener la secuencia de entrada del cliente
            NetworkStream stream = client.GetStream();

            // Buffer para la lectura de datos
            byte[] buffer = new byte[1024];

            // Leer el nombre del archivo enviado por el cliente
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string fileName = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            // Crear un FileStream para escribir el archivo
            string filePath = Path.Combine(ServerDirectory, fileName);
            using (FileStream fileStream = File.Create(filePath))
            {
                // Leer y escribir datos en el archivo
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                }
            }

            //Console.WriteLine($"Archivo recibido y guardado: {fileName}");

            // Responder al cliente
            string responseMessage = "Archivo recibido con éxito en el servidor.";
            byte[] responseData = Encoding.ASCII.GetBytes(responseMessage);
            stream.Write(responseData, 0, responseData.Length);


            // Cerrar la conexión
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al manejar cliente: {ex.Message}");
        }
    }
}
