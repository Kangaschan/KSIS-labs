using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FTP_server
{
    public partial class MainForm : Form
    {
        private Socket listenerSocket;
        private Thread listenerThread;
        private bool isRunning;
        private string dataConnectionIp;
        private int dataConnectionPort;
        private string currentDirectory = "/";
        private List<Socket> dataClients = new List<Socket>();

        public MainForm()
        {
            InitializeComponent();
            InitializeFTPServer();
        }

        private void InitializeFTPServer()
        {
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenerSocket.Bind(new IPEndPoint(IPAddress.Any, 21));
            listenerSocket.Listen(10);

            listenerThread = new Thread(new ThreadStart(StartListening));
            listenerThread.IsBackground = true;
            listenerThread.Start();
            isRunning = true;
            Log("FTP Server started on port 21.");
        }

        private void StartListening()
        {
            while (isRunning)
            {
                if (listenerSocket.Poll(100000, SelectMode.SelectRead))
                {
                    Socket clientSocket = listenerSocket.Accept();
                    Thread clientThread = new Thread(() => HandleClient(clientSocket));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                Thread.Sleep(100);
            }
        }

        private void HandleClient(Socket clientSocket)
        {
            NetworkStream stream = new NetworkStream(clientSocket);
            byte[] buffer = new byte[1024];
            int bytesRead;
            string data = "";
            string userName = "";
            bool isLoggedIn = false;
            string transferType = "A";
            SendResponse(stream, "220 Welcome to My FTP Server");

            while (clientSocket.Connected)
            {
                try
                {
                    if ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        data += Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        if (data.Contains("\r\n"))
                        {
                            string command = data.Trim();
                            Log(command);
                            string[] commandParts = command.Split(' ');

                            switch (commandParts[0].ToUpper())
                            {
                                case "USER":
                                    userName = commandParts[1];
                                    SendResponse(stream, "331 Password required for " + userName);
                                    break;
                                case "PASS":
                                    if (commandParts[1] == "abcd")
                                    {
                                        isLoggedIn = true;
                                        SendResponse(stream, "230 User " + userName + " logged in.");
                                    }
                                    else
                                    {
                                        SendResponse(stream, "530 Login incorrect.");
                                    }
                                    break;
                                case "QUIT":
                                    SendResponse(stream, "221 Goodbye.");
                                    clientSocket.Close();
                                    break;
                                case "PORT":
                                    ParsePortCommand(commandParts[1]);
                                    SendResponse(stream, "200 PORT command successful.");
                                    break;
                                case "SYST":
                                    SendResponse(stream, "215 UNIX Type: L8");
                                    break;
                                case "FEAT":
                                    SendResponse(stream, "211-Features:\r\n PASV\r\n UTF8\r\n211 End");
                                    break;
                                case "PWD":
                                    SendResponse(stream, $"257 \"{currentDirectory}\" is the current directory.");
                                    break;
                                case "TYPE":
                                    if (commandParts.Length > 1 && (commandParts[1] == "A" || commandParts[1] == "I"))
                                    {
                                        transferType = commandParts[1];
                                        SendResponse(stream, "200 Type set to " + transferType);
                                    }
                                    else
                                    {
                                        SendResponse(stream, "501 Syntax error in parameters or arguments.");
                                    }
                                    break;
                                case "CWD":
                                    if (commandParts.Length > 1)
                                    {
                                        ChangeWorkingDirectory(commandParts[1]);
                                        SendResponse(stream, "250 Directory changed to " + currentDirectory);
                                    }
                                    else
                                    {
                                        SendResponse(stream, "501 Syntax error in parameters or arguments.");
                                    }
                                    break;
                                case "CDUP":
                                    ChangeWorkingDirectory("..");
                                    SendResponse(stream, "250 Directory changed to " + currentDirectory);
                                    break;
                                case "MKD":
                                    if (commandParts.Length > 1)
                                    {
                                        Directory.CreateDirectory(Path.Combine(currentDirectory, commandParts[1]));
                                        SendResponse(stream, "257 Directory created.");
                                    }
                                    else
                                    {
                                        SendResponse(stream, "501 Syntax error in parameters or arguments.");
                                    }
                                    break;
                                case "RMD":
                                    if (commandParts.Length > 1)
                                    {
                                        Directory.Delete(Path.Combine(currentDirectory, commandParts[1]));
                                        SendResponse(stream, "250 Directory removed.");
                                    }
                                    else
                                    {
                                        SendResponse(stream, "501 Syntax error in parameters or arguments.");
                                    }
                                    break;
                                case "DELE":
                                    if (commandParts.Length > 1)
                                    {
                                        File.Delete(Path.Combine(currentDirectory, commandParts[1]));
                                        SendResponse(stream, "250 File deleted.");
                                    }
                                    else
                                    {
                                        SendResponse(stream, "501 Syntax error in parameters или arguments.");
                                    }
                                    break;
                                case "RETR":
                                    if (isLoggedIn && commandParts.Length > 1)
                                    {
                                        SendResponse(stream, "150 Opening data connection.");
                                        string filePath = Path.Combine(currentDirectory, commandParts[1]);
                                        SendFile(filePath);
                                        SendResponse(stream, "226 Transfer complete.");
                                    }
                                    else
                                    {
                                        SendResponse(stream, "530 Not logged in.");
                                    }
                                    break;
                                case "STOR":
                                    if (isLoggedIn && commandParts.Length > 1)
                                    {
                                        SendResponse(stream, "150 Opening data connection.");
                                        string filePath = Path.Combine(currentDirectory, commandParts[1]);
                                        ReceiveFile(filePath);
                                        SendResponse(stream, "226 Transfer complete.");
                                    }
                                    else
                                    {
                                        SendResponse(stream, "530 Not logged in.");
                                    }
                                    break;
                                case "PASV":
                                    EnterPassiveMode(stream, clientSocket);
                                    break;
                                case "LIST":
                                    if (isLoggedIn)
                                    {
                                        SendResponse(stream, "150 Opening data connection.");
                                        SendDirectoryListing();
                                        SendResponse(stream, "226 Transfer complete.");
                                    }
                                    else
                                    {
                                        SendResponse(stream, "530 Not logged in.");
                                    }
                                    break;
                                default:
                                    SendResponse(stream, "502 Command not implemented.");
                                    break;
                            }
                            data = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log("Error: " + ex.Message);
                    clientSocket.Close();
                }
            }
        }

        private void ParsePortCommand(string portCommand)
        {
            string[] parts = portCommand.Split(',');
            dataConnectionIp = string.Join(".", parts, 0, 4);
            dataConnectionPort = (int.Parse(parts[4]) << 8) + int.Parse(parts[5]);
        }

        private void EnterPassiveMode(NetworkStream stream, Socket clientSocket)
        {
            try
            {
                Socket passiveListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                passiveListener.Bind(new IPEndPoint(IPAddress.Any, 0));
                passiveListener.Listen(1);
                IPEndPoint localEndpoint = (IPEndPoint)passiveListener.LocalEndPoint;
                int port = localEndpoint.Port;

                string ipAddress = ((IPEndPoint)clientSocket.LocalEndPoint).Address.ToString().Replace(".", ",");
                string portResponse = $"227 Entering Passive Mode ({ipAddress},{port / 256},{port % 256})";

                dataClients.Clear();
                new Thread(() =>
                {
                    Socket dataClient = passiveListener.Accept();
                    dataClients.Add(dataClient);
                    passiveListener.Close();
                }).Start();

                SendResponse(stream, portResponse);
            }
            catch (Exception ex)
            {
                Log("Error entering passive mode: " + ex.Message);
            }
        }

        private void SendFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    byte[] fileData = File.ReadAllBytes(filePath);
                    foreach (var dataClient in dataClients)
                    {
                        NetworkStream dataStream = new NetworkStream(dataClient);
                        dataStream.Write(fileData, 0, fileData.Length);
                        dataStream.Close();
                        dataClient.Close();
                    }
                }
                else
                {
                    Log("File not found: " + filePath);
                }
            }
            catch (Exception ex)
            {
                Log("Error sending file: " + ex.Message);
            }
        }

        private void ReceiveFile(string filePath)
        {
            try
            {
                foreach (var dataClient in dataClients)
                {
                    NetworkStream dataStream = new NetworkStream(dataClient);
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        dataStream.CopyTo(fileStream);
                    }
                    dataStream.Close();
                    dataClient.Close();
                }
            }
            catch (Exception ex)
            {
                Log("Error receiving file: " + ex.Message);
            }
        }

        private void SendDirectoryListing()
        {
            try
            {
                StringBuilder listing = new StringBuilder();
                foreach (string dir in Directory.GetDirectories(currentDirectory))
                {
                    listing.AppendLine($"drwxr-xr-x 2 user group 4096 May 26 10:45 {Path.GetFileName(dir)}");
                }
                foreach (string file in Directory.GetFiles(currentDirectory))
                {
                    listing.AppendLine($"-rw-r--r-- 1 user group {new FileInfo(file).Length} May 26 10:45 {Path.GetFileName(file)}");
                }
                foreach (var dataClient in dataClients)
                {
                    NetworkStream dataStream = new NetworkStream(dataClient);
                    byte[] listingBytes = Encoding.UTF8.GetBytes(listing.ToString());
                    dataStream.Write(listingBytes, 0, listingBytes.Length);
                    dataStream.Close();
                    dataClient.Close();
                }
                dataClients.Clear();
            }
            catch (Exception ex)
            {
                Log("Error sending directory listing: " + ex.Message);
            }
        }

        private void ChangeWorkingDirectory(string newPath)
        {
            if (newPath == "..")
            {
                currentDirectory = Directory.GetParent(currentDirectory)?.FullName ?? currentDirectory;
            }
            else
            {
                string fullPath = Path.Combine(currentDirectory, newPath);
                if (Directory.Exists(fullPath))
                {
                    currentDirectory = fullPath;
                }
            }
        }

        private void SendResponse(NetworkStream stream, string response)
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(response + "\r\n");
            stream.Write(responseBytes, 0, responseBytes.Length);
            Log(response);
        }

        private void Log(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(Log), message);
            }
            else
            {
                textBoxLog.AppendText(message + "\r\n");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
            listenerSocket.Close();
        }
    }
}
