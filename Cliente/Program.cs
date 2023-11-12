using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        // Establecer la dirección IP y el puerto del servidor
        string serverIp = "127.0.0.1";
        int serverPort = 8080;

        // Crear el objeto TcpClient y conectar al servidor
        TcpClient client = new TcpClient(serverIp, serverPort);
        Console.WriteLine("Cliente conectado al servidor.");

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
        }

        // Leer la respuesta del servidor
        //byte[] responseBuffer = new byte[1024];
        //int responseBytes = stream.Read(responseBuffer, 0, responseBuffer.Length);
        //string responseMessage = Encoding.ASCII.GetString(responseBuffer, 0, responseBytes);
        //Console.WriteLine($"Respuesta del servidor: {responseMessage}");



        // Cerrar la conexión
        client.Close();

        Console.ReadLine();
    }
}
