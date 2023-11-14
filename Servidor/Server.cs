using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.IO;

namespace Servidor
{
    class Server
    {
        Socket socket;
        Thread listenThread;
        Hashtable usersTable;

        private static readonly string ServerDirectory = "ServerFiles";

        public Server()
        {
            try
            {
                Console.WriteLine("Init Server ...");
                createFolderInServer(ServerDirectory);

                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress addr = host.AddressList[0];
                IPEndPoint endPoint = new IPEndPoint(addr, 8080);//4404);

                socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(endPoint);
                //Permite hasta 10 conexiones paralelas dentro de este socket
                socket.Listen(10);

                //Se crea un hilo nuevo para cada conexión
                listenThread = new Thread(this.Listen);
                listenThread.Start();
                usersTable = new Hashtable();
            }
            catch (Exception e)
            {
                Utils.printClassError("Server", e);
            }
        }

        private void Listen()
        {
            Socket client;
            while (true)
            {
                client = this.socket.Accept();
                listenThread = new Thread(this.ListenClient);
                listenThread.Start(client);
            }
        }

        private void ListenClient(object o)
        {
            Socket client = (Socket)o;
            object received;

            while (true)
            {
                //received = this.Receive(client);
                HandleClient(client);
            }
        }

        private static void createFolderInServer(string serverDirectory)
        {
            // Crear el directorio de archivos si no existe
            if (!Directory.Exists(serverDirectory))
            {
                Directory.CreateDirectory(serverDirectory);
            }
        }

        static SemaphoreSlim _sem = new SemaphoreSlim(2);


        static void HandleClient(Socket client)
        {
            Console.WriteLine("entrando a handle {0}", client.GetHashCode());
            _sem.Wait();
            Console.WriteLine("estoy adentro! {0}", client.GetHashCode());
            try
            {
                // Obtener la secuencia de entrada del cliente
                NetworkStream stream = new NetworkStream(client, true);//client //.GetStream();

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

               // Utils.printFileAndList(fileName, ServerDirectory);

                // Responder al cliente
                string responseMessage = "Archivo recibido con éxito en el servidor.";
                byte[] responseData = Encoding.ASCII.GetBytes(responseMessage);
                stream.Write(responseData, 0, responseData.Length);


                // Cerrar la conexión
              //  client.Close();
                Console.WriteLine("salgo handle {0}", client.GetHashCode());
                _sem.Release();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Paso POR ACÄ");
                _sem.Release();
                Utils.printClassError("HandleClient", ex);
            }
        }

    }
}
