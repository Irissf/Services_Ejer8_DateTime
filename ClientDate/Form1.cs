using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    Haz un cliente simple con cuatro botones (uno por comando) y un TextBoxz o Label para mostrar el 
    resultado y probarlo. También tendrá la posibilidad de indicarle la IP y puerto de conexión (aunque venga 
    con unos predefinidos) en un formulario de diálogo.
 */

namespace ClientDate
{
    public partial class Form1 : Form
    {
        Form2 form2;
        public Form1()
        {
            InitializeComponent();
            dialogScreen();
        }

        private void click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            //label1.Text = (string)btn.Tag;
            string command = (string)btn.Tag;
            int port = 0;
            IPAddress ip;

            if (command == "change")
            {
                label1.Text = "";
                dialogScreen();
            }
            else
            {
                //Indicamos la ip y puerto al que queremos conectarnos
                try
                {
                    port = Convert.ToInt32(form2.textBoxPort.Text);
                    ip = IPAddress.Parse(form2.textBoxIp.Text);

                    IPEndPoint iPEndPoint = new IPEndPoint(ip, port);
                    Socket socketConnect = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        //El cliente inicia una conexión haciendo una petición con connect
                        socketConnect.Connect(iPEndPoint);
                        using (NetworkStream ns = new NetworkStream(socketConnect))
                        using (StreamReader sr = new StreamReader(ns))
                        using (StreamWriter sw = new StreamWriter(ns))
                        {
                            label1.Text = sr.ReadLine();
                            if (command == "turnoff")
                            {
                                labelIp.Text = "Ip:";
                                labelPort.Text = "Port:";
                                button1.Enabled = false;
                                button2.Enabled = false;
                                button3.Enabled = false;
                                button4.Enabled = false;

                            }
                            sw.WriteLine(command);
                            sw.Flush();
                            label1.Text = sr.ReadLine();


                        }
                    }
                    catch (SocketException ex)
                    {
                        label1.Text = "Error conection";

                    }
                }
                catch (FormatException)
                {
                    label1.Text = "Incorrect values";
                }catch (ArgumentOutOfRangeException)
                {
                    label1.Text = "Incorrect values";
                }


               
            }
        }

        private void dialogScreen()
        {
            form2 = new Form2();

            DialogResult result = form2.ShowDialog();
            if (result == DialogResult.OK)
            {
                labelIp.Text = "Ip: " + form2.textBoxIp.Text;
                labelPort.Text = "Port: " + form2.textBoxPort.Text;
            }
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }
    }
}
