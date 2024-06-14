using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

class Program
{
    // Структура для хранения информации о компьютере в сети
    [StructLayout(LayoutKind.Sequential)]
    public struct NETRESOURCE
    {
        public uint dwScope;
        public uint dwType;
        public uint dwDisplayType;
        public uint dwUsage;
        public string lpLocalName;
        public string lpRemoteName;
        public string lpComment;
        public string lpProvider;
    }

    // Подключение к ресурсам сети
    [DllImport("mpr.dll")]
    public static extern int WNetOpenEnum(
        uint dwScope,
        uint dwType,
        uint dwUsage,
        ref NETRESOURCE lpNetResource,
        out IntPtr lphEnum
    );

    // Получение информации о ресурсах сети
    [DllImport("mpr.dll")]
    public static extern int WNetEnumResource(
        IntPtr hEnum,
        ref uint lpcCount,  
        IntPtr lpBuffer,
        ref uint lpBufferSize
    );

    // Закрытие подключения к ресурсам сети
    [DllImport("mpr.dll")]
    public static extern int WNetCloseEnum(IntPtr hEnum);
    // Функция для получения MAC-адреса компьютера
    static string GetMacAddress()
    {
        string macAddress = "";
        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
           // if (nic.OperationalStatus == OperationalStatus.Up)
            {
                
                macAddress += nic.Description;
                Console.WriteLine(nic.Description);
                Console.WriteLine(nic.GetPhysicalAddress().ToString());
               // macAddress += nic.GetPhysicalAddress().ToString() + " ";
                

                //break;
            }
        }
        return macAddress;
    }

    static void ListNetworkResources()
    {
        // Создание структуры NETRESOURCE
        NETRESOURCE resource = new NETRESOURCE
        {
            dwScope = 2, // Скоп сети RESOURCE_GLOBALNET
            dwType = 1, // Доступный ресурс RESOURCETYPE_ANY
            dwDisplayType= 3, // Общий ресурс
            dwUsage = 0,//All resources
            lpLocalName = null,
            lpRemoteName = null,
            lpComment = null,
            lpProvider = null
        };

        IntPtr resourceHandle;
        uint entriesCount = 0xFFFFFFFF; // Получить все ресурсы

        // Открытие перечисления ресурсов сети
        int result = WNetOpenEnum(resource.dwScope, resource.dwType, resource.dwUsage, ref resource, out resourceHandle);
        if (result == 0)
        {
            IntPtr buffer = Marshal.AllocHGlobal(4096); // Размер буфера для информации о ресурсах
            uint bufferSize = 4096;

            // Получение информации о ресурсах
            result = WNetEnumResource(resourceHandle, ref entriesCount, buffer, ref bufferSize);
            if (result == 0)
            {
                IntPtr currentBuffer = buffer;
                for (int i = 0; i < entriesCount; i++)
                {
                    NETRESOURCE currentResource = (NETRESOURCE)Marshal.PtrToStructure(currentBuffer, typeof(NETRESOURCE));
                    Console.WriteLine($"");
                    Console.WriteLine($"Удаленное имя: {currentResource.lpRemoteName}");
                    Console.WriteLine($"Комментарий: {currentResource.lpComment}");
                    Console.WriteLine($"использование: {currentResource.dwUsage}");
                    Console.WriteLine($"Локальное имя: {currentResource.lpLocalName}");
                    Console.WriteLine($"провайдер: {currentResource.lpProvider}");
                    currentBuffer = (IntPtr)((int)currentBuffer + Marshal.SizeOf(typeof(NETRESOURCE)));
                }
            }

            // Освобождение памяти и закрытие перечисления
            Marshal.FreeHGlobal(buffer);
            WNetCloseEnum(resourceHandle);
        }
    }
   
    static void Main(string[] args)
    {
      
        string macAddress = GetMacAddress();
       // Console.WriteLine("MAC-адрес компьютера: " + macAddress);
        // Отображение всех рабочих групп, компьютеров и их ресурсов
        ListNetworkResources();
    }


}
