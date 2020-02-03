using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;


namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // устанавливаем для сокета локальную конечную точку
            IPHostEntry ipHost = Dns.Resolve("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // создаем сокет Tcp/Ip
            Socket sListener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // назначаем сокет локальной конечной точке и 
            // слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);

            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
