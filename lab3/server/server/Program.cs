using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TCPServer
{
    static void Main(string[] args)
 
    {
        int port = 37;

        try
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Клиент подключен.");

                NetworkStream stream = client.GetStream();

                // Отправляем текущее время в секундах с 1900 года, учитывая часовой пояс
                DateTime currentTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                uint ntpTime = (uint)((currentTime - new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
                byte[] data = BitConverter.GetBytes(ntpTime);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Отправлено время: " + currentTime);

                // Закрываем соединение
                client.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }

}
