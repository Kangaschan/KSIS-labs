using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        private const int PACKAGE_AMOUNT = 50000;
        private const int PACKAGE_SIZE = 200;
        private const string END_STR = "FFFFFFFFFFFFFFFF";
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        public class RandSeq
        {
            private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private char startletter;
            public RandSeq(char startletter)
            {
                this.startletter = startletter;
            }
            public string RetStr(int len, int j)
            {
                string result = "";
                for (int i = 0; i < len; i++)
                {
                    result += Alphabet[((int)startletter * (i % 3) + i + j) % Alphabet.Length];
                    if (i % 5 ==  4)
                    {
                        result += ' ';
                    }
                }
                return result;
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            int TCPLOSS = 0, UDPLOSS=0;
            double TCPTIME, UDPTIME;
            RandSeq SeqRand = new RandSeq('k');
            Stopwatch stopwatch = new Stopwatch();

            //TCP PART
            if (checkBox1.Checked)
            {
                // Серверная часть
                int port = 8888;
                Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    IPAddress localAddr = IPAddress.Parse(txtBoxIP.Text);
                    tcpServer.Bind(new IPEndPoint(localAddr, port));
                    tcpServer.Listen(10);

                    while (true)
                    {
                        Socket clientSocket = tcpServer.Accept();
                        NetworkStream stream = new NetworkStream(clientSocket);

                        byte[] data = new byte[1024];
                        StringBuilder responseData = new StringBuilder();

                        bool flag = false;
                        while (true)
                        {
                            int bytesRead = stream.Read(data, 0, data.Length);
                            responseData.Clear();
                            responseData.Append(Encoding.UTF8.GetString(data, 0, bytesRead));

                            byte[] msg = Encoding.UTF8.GetBytes(responseData.ToString());
                            stream.Write(msg, 0, msg.Length);

                            if (responseData.ToString() == END_STR)
                            {
                                flag = true;
                                break;
                            }
                        }

                        stream.Close();
                        clientSocket.Close();

                        if (flag)
                            break;
                    }
                }
                finally
                {
                    tcpServer.Close();
                }
            }
            else
            {
                // Клиентская часть
                try
                {
                    int port = 8888;
                    Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress serverAddress = IPAddress.Parse(txtBoxIP.Text);
                    IPEndPoint serverEP = new IPEndPoint(serverAddress, port);
                    tcpClient.Connect(serverEP);
                    NetworkStream stream = new NetworkStream(tcpClient);

                    byte[] data = new byte[1024];
                    StringBuilder responseData = new StringBuilder();
                    stopwatch.Start();

                    for (int i = 0; i < PACKAGE_AMOUNT; i++)
                    {
                        string message = SeqRand.RetStr(PACKAGE_SIZE, i);
                        data = Encoding.UTF8.GetBytes(message);
                        stream.Write(data, 0, data.Length);

                        int bytesRead = stream.Read(data, 0, data.Length);
                        responseData.Clear();
                        responseData.Append(Encoding.UTF8.GetString(data, 0, bytesRead));

                        if (message != responseData.ToString())
                        {
                            TCPLOSS++;
                        }
                    }
                    stopwatch.Stop();
                    data = Encoding.UTF8.GetBytes(END_STR);
                    stream.Write(data, 0, data.Length);

                    
                    TimeSpan elapsedTime = stopwatch.Elapsed;
                    stopwatch.Reset();
                    TCPTIME = elapsedTime.TotalMilliseconds;
                    txtBoxTCPLost.Text = TCPLOSS.ToString();
                    txtBoxTCPtime.Text = TCPTIME.ToString();
                    txtBoxTCPSPEED.Text = ((PACKAGE_SIZE * PACKAGE_AMOUNT / TCPTIME).ToString());

                    stream.Close();
                    tcpClient.Close();
                }
                finally
                {
                    Console.WriteLine("Завершение работы клиента.");
                }
            }


            //UDP PART
            if (checkBox1.Checked)
            {
                // Серверная часть
                int port = 8888;
                Socket udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    
                udpServer.Bind(new IPEndPoint(IPAddress.Any, port));

                // Ловим все сообщения, которые приходят на указанный порт
                EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                try
                {
                    while (true)
                    {
                        byte[] receivedData = new byte[4096];
                        int bytesRead = udpServer.ReceiveFrom(receivedData, ref remoteEP);
                        string receivedMessage = Encoding.UTF8.GetString(receivedData, 0, bytesRead);
                        udpServer.SendTo(receivedData, bytesRead, SocketFlags.None, remoteEP);
                        if (receivedMessage == END_STR)
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    udpServer.Close();
                }
            }
            else
            {
                // Клиентская часть
                try
                {
                    // Указываем IP-адрес и порт сервера
                    int port = 8888;
                    Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    IPAddress serverAddress = IPAddress.Parse(txtBoxIP.Text);
                    IPEndPoint serverEP = new IPEndPoint(serverAddress, port);

                    stopwatch.Start();
                    for (int i = 0; i < PACKAGE_AMOUNT; i++)
                    {
                        string message = SeqRand.RetStr(PACKAGE_SIZE, i);
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        udpClient.SendTo(data, data.Length, SocketFlags.None, serverEP);

                        byte[] receivedData = new byte[4096];
                        EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                        int bytesReceived = udpClient.ReceiveFrom(receivedData, ref remoteEP);
                        string receivedMessage = Encoding.UTF8.GetString(receivedData, 0, bytesReceived);
                        if (receivedMessage != message)
                        {
                            UDPLOSS++;
                        }
                    }
                    stopwatch.Stop();
                    TimeSpan elapsedTime = stopwatch.Elapsed;
                    stopwatch.Reset();
                    UDPTIME = elapsedTime.TotalMilliseconds;
                    txtBoxUDPLost.Text = UDPLOSS.ToString();
                    txtBoxUPDtime.Text = UDPTIME.ToString();
                    txtBoxUDPSPEED.Text = ((PACKAGE_SIZE * PACKAGE_AMOUNT) / UDPTIME).ToString();
                    byte[] enddata = Encoding.UTF8.GetBytes(END_STR);
                    udpClient.SendTo(enddata, enddata.Length, SocketFlags.None, serverEP);

                    udpClient.Close();
                }
                catch (Exception ex)
                {
                    // Обработка ошибок
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }  

            }
    }
}
