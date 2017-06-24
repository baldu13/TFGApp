using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace ClientApp
{
    public class ServerConnection
    {
        private static string ipServer = getServerIp();
        private static int port = 12003;
        private static int portAdmin = 12002;

        public static model.Experimento logeaExperimento(string usuario, string clave)
        {

            model.Experimento exp = new model.Experimento();

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

                //Enviamos
                clientSocket.Send(BitConverter.GetBytes(usuario.Length)); //Longitud del string
                clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(usuario));
                
                clientSocket.Send(BitConverter.GetBytes(clave.Length)); //Longitud del string
                clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(clave));

                //Recibimos
                byte[] buffer = new byte[4];

                clientSocket.Receive(buffer);
                exp.Id = bytesToInt(buffer);

                clientSocket.Receive(buffer);
                exp.Tipo = bytesToInt(buffer);

                clientSocket.Receive(buffer);
                exp.Rondas = bytesToInt(buffer);
                //Cerramos la conexion
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

            }catch (Exception) {
                exp = new model.Experimento();
                exp.Id = -1;
                exp.Tipo = -1;
            }
            return exp;
        }

        public static void enviaResultadoBeautyContest(float numElegido, string usuario, int ronda)
        {
            //Creacion del socket
            IPAddress ip = IPAddress.Parse(ipServer);
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //Conectamos al servidor
                clientSocket.Connect(endPoint);

                //Trabajamos con el socket
                clientSocket.Send(BitConverter.GetBytes(2)); //Codigo de la operacion

                //Enviamos
                clientSocket.Send(BitConverter.GetBytes(1)); //Tipo Resultado

                clientSocket.Send(BitConverter.GetBytes(numElegido)); //Numero elegido

                clientSocket.Send(BitConverter.GetBytes(usuario.Length)); //Longitud del string
                clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(usuario)); //Username

                clientSocket.Send(BitConverter.GetBytes(ronda)); //Ronda

                //Cerramos la conexion
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

            }
            catch (Exception) { }
        }

        public static void enviaResultadoFondos(float fondoPublico, float fondoPrivado, string usuario, int ronda)
        {
            //Creacion del socket
            IPAddress ip = IPAddress.Parse(ipServer);
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //Conectamos al servidor
                clientSocket.Connect(endPoint);

                //Trabajamos con el socket
                clientSocket.Send(BitConverter.GetBytes(2)); //Codigo de la operacion

                //Enviamos
                clientSocket.Send(BitConverter.GetBytes(2)); //Tipo Resultado

                clientSocket.Send(BitConverter.GetBytes(fondoPublico)); //Fondo publico
                clientSocket.Send(BitConverter.GetBytes(fondoPrivado)); //Fondo privado

                clientSocket.Send(BitConverter.GetBytes(usuario.Length)); //Longitud del string
                clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(usuario)); //Username

                clientSocket.Send(BitConverter.GetBytes(ronda)); //Ronda

                //Cerramos la conexion
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

            }
            catch (Exception) { }
        }

        public static model.Resultado[] resultadosExperimento(string usuario)
        {
            //Creacion del socket
            IPAddress ip = IPAddress.Parse(ipServer);
            IPEndPoint endPoint = new IPEndPoint(ip, portAdmin);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            model.Resultado[] resultados;

            try
            {
                //Conectamos al servidor
                clientSocket.Connect(endPoint);

                //Trabajamos con el socket
                clientSocket.Send(BitConverter.GetBytes(5)); //Codigo de la operacion

                //Enviamos
                clientSocket.Send(BitConverter.GetBytes(usuario.Length)); //Longitud del string
                clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(usuario)); //Username

                //Recibimos
                byte[] buffer = new byte[4];
                byte[] bufferString;

                clientSocket.Receive(buffer);
                int longitud = bytesToInt(buffer);

                resultados = new model.Resultado[longitud];
                model.Resultado resultado;
                for(int i=0; i<longitud; i++)
                {
                    resultado = new model.Resultado();
                    clientSocket.Receive(buffer);
                    resultado.Grupo = bytesToInt(buffer);

                    clientSocket.Receive(buffer);
                    resultado.Ronda = bytesToInt(buffer);

                    clientSocket.Receive(buffer);
                    int longString = bytesToInt(buffer);
                    bufferString = new byte[longString];
                    clientSocket.Receive(bufferString);
                    resultado.Usuario = System.Text.Encoding.ASCII.GetString(bufferString);

                    clientSocket.Receive(buffer);
                    longString = bytesToInt(buffer);
                    bufferString = new byte[longString];
                    clientSocket.Receive(bufferString);
                    resultado.Etiqueta = System.Text.Encoding.ASCII.GetString(bufferString);

                    clientSocket.Receive(buffer);
                    longString = bytesToInt(buffer);
                    bufferString = new byte[longString];
                    clientSocket.Receive(bufferString);
                    resultado.ValorTexto = System.Text.Encoding.ASCII.GetString(bufferString);

                    clientSocket.Receive(buffer);
                    resultado.ValorNumerico = bytesToFloat(buffer);

                    resultados[i] = resultado;
                }
                //Cerramos la conexion
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

                return resultados;
            }
            catch (Exception) { }

            return null;
        }

        public static float[] getRatiosExperimento(int idExperimento)
        {
            float[] ratios = new float[2];

            //Creacion del socket
            IPAddress ip = IPAddress.Parse(ipServer);
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //Conectamos al servidor
                clientSocket.Connect(endPoint);

                //Trabajamos con el socket
                clientSocket.Send(BitConverter.GetBytes(3)); //Codigo de la operacion

                //Enviamos
                clientSocket.Send(BitConverter.GetBytes(idExperimento));

                //Recibimos
                byte[] buffer = new byte[4];
                clientSocket.Receive(buffer);
                ratios[0] = bytesToFloat(buffer);

                buffer = new byte[4];
                clientSocket.Receive(buffer);
                ratios[1] = bytesToFloat(buffer);

                //Cerramos la conexion
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }catch(Exception) { }

            return ratios;
        }

        public static int getTotalParticipantes(int idExperimento)
        {
            int numResultados = -1;
            //Creacion del socket
            IPAddress ip = IPAddress.Parse(ipServer);
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //Conectamos al servidor
                clientSocket.Connect(endPoint);

                //Trabajamos con el socket
                clientSocket.Send(BitConverter.GetBytes(4)); //Codigo de la operacion

                //Enviamos
                clientSocket.Send(BitConverter.GetBytes(idExperimento));

                //Recibimos
                byte[] buffer = new byte[4];
                clientSocket.Receive(buffer);
                numResultados = bytesToInt(buffer);

                //Cerramos la conexion
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception) { }

            return numResultados;
        }

        private static int bytesToInt(byte[] buffer)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            return BitConverter.ToInt32(buffer, 0);
        }

        private static float bytesToFloat(byte[] buffer)
        {
            return System.BitConverter.ToSingle(buffer, 0);
        }

        private static string getServerIp()
        {
            StreamReader sr = new StreamReader(Application.Context.Assets.Open("serverip.txt"));
            return sr.ReadToEnd();
        }
    }
}