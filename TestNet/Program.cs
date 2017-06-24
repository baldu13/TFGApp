using System;
using System.Net;
using System.Net.Sockets;

namespace TestNet
{
    class Program
    {
        public static void Main(string[] args)
        {
            string ipServer = "127.0.0.1";
            int port = 12003;
            string usuario = "ex10u2", clave = "1xfULX";

            //Creacion del socket
            IPAddress ip = IPAddress.Parse(ipServer);
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //Conectamos al servidor
                clientSocket.Connect(endPoint);

                //Trabajamos con el socket
                clientSocket.Send(BitConverter.GetBytes(1)); //Codigo de la operacion

                //Usuario
                clientSocket.Send(BitConverter.GetBytes(usuario.Length)); //Longitud del string
                clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(usuario));

                //Contrasena
                clientSocket.Send(BitConverter.GetBytes(clave.Length)); //Longitud del string
                clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(clave));

                //Recibimos
                byte[] buffer = new byte[4];

                clientSocket.Receive(buffer);
                Console.WriteLine("ID: " + bytesToInt(buffer));

                clientSocket.Receive(buffer);
                Console.WriteLine("Tipo: " + bytesToInt(buffer));
                //Cerramos la conexion
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

                Console.Read();
            }
            catch (Exception)
            { }
        }

        private static int bytesToInt(byte[] buffer)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
