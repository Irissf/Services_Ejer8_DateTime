using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/*
    Realiza un servidor de fecha y hora. Aceptará los comandos: HORA (hora, minutos y segundos)), FECHA (día,
    mes y año), TODO (hora y fecha), APAGAR (El servidor se cierra). Dependiendo del comando que reciba 
    enviará la información correspondiente.
    Por la brevedad de los mensajes transmitidos, no es necesario que sea mutihilo. .
 */

namespace Services_Ejer8_DateTime
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = "";
            bool close = false;

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 31416);
            Socket socketServe = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //Enlazamos el socket con el puerto
                socketServe.Bind(ipEndPoint);

                //El socket permanece a la escucha de un cliente, pues aceptará hasta 3 clientes, indicado entre los paréntesis
                socketServe.Listen(3);

                //si no lo ponemos en un while el programa se cierra al acabar con el cliente conectado

                while (!close)
                {

                    //aceptamos la conexión
                    Socket socketClient = socketServe.Accept();
                    Console.WriteLine("client acepted");

                    using (NetworkStream ns = new NetworkStream(socketClient))
                    using (StreamReader sr = new StreamReader(ns))
                    using (StreamWriter sw = new StreamWriter(ns))
                    {
                        sw.WriteLine("Wellcome");
                        //sw.WriteLine("Commands: date, time, datetime, turnoff");//Turn off=>apagar
                        sw.Flush();

                        command = sr.ReadLine();
                        switch (command)
                        {
                            case "date":
                                sw.WriteLine(DateTime.Now.ToString("hh:mm:ss"));
                                sw.Flush();
                                break;
                            case "time":
                                sw.WriteLine(DateTime.Now.ToString("dddd, dd-MM-yyyy"));
                                sw.Flush();
                                break;
                            case "datetime":
                                sw.WriteLine(DateTime.Now.ToString("dddd, dd:MM:yyyy hh:mm:ss"));
                                sw.Flush();
                                break;
                            case "turnoff":
                                //cerramos el servidor
                                close = true;
                                socketClient.Close();
                                socketServe.Close();
                                break;
                            default:
                                sw.WriteLine("Commands: date, time, datetime, turnoff");//Turn off=>apagar
                                sw.Flush();
                                break;
                        }
                    }
                    //cerramos el socket del cliente que hizo la petición, pero nos quedamos a la escucha de nuevos clientes
                    socketClient.Close();
                }
            }
            catch (SocketException e) when (e.ErrorCode == (int)SocketError.AddressAlreadyInUse)
            {
                Console.WriteLine("The port is in use");
                Console.WriteLine("Error connect");
                Console.ReadLine();
            }


        }
    }
}
