using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TCPClient
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Получить время от сервера");
            Console.WriteLine("2. Получить время от NTP-сервера");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    GetTimeFromServer();
                    break;
                case 2:
                    GetTimeFromNTPServer();
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }

    
    static void GetTimeFromServer()
    {
        Console.WriteLine("Введите IP-адрес сервера:");
        string serverIP = Console.ReadLine();
        int port = 37;

        try
        {
            TcpClient client = new TcpClient(serverIP, port);
            Console.WriteLine("Подключение к серверу...");

            NetworkStream stream = client.GetStream();

            byte[] data = new byte[4];
            stream.Read(data, 0, data.Length);
            uint ntpTime = BitConverter.ToUInt32(data, 0);

            // Переводим время в формат DateTime с учетом часового пояса
            DateTime networkDateTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(ntpTime), TimeZoneInfo.Local);

            Console.WriteLine("Время от сервера: " + networkDateTime);

            // Закрываем соединение
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при подключении к серверу: " + ex.Message);
        }
    }

    static void GetTimeFromNTPServer()
    {
        const string ntpServer = "time.windows.com";

        // NTP message size - 16 bytes of the digest (RFC 2030)
        var ntpData = new byte[48];

        ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

        var addresses = Dns.GetHostEntry(ntpServer).AddressList;

        var ipEndPoint = new IPEndPoint(addresses[0], 123);

        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Connect(ipEndPoint);

            socket.ReceiveTimeout = 3000;

            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();
        }

        const byte serverReplyTime = 40;

        //Get the seconds part
        ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

        //Get the seconds fraction
        ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

        intPart = SwapEndianness(intPart);
        fractPart = SwapEndianness(fractPart);

        var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

        //**UTC** time
        var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

        Console.WriteLine( networkDateTime.ToLocalTime());
    }

    static uint SwapEndianness(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
                       ((x & 0x0000ff00) << 8) +
                       ((x & 0x00ff0000) >> 8) +
                       ((x & 0xff000000) >> 24));
    }

}
