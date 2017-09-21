using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using DataBase;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SocketClient
{
    class Clnt
    {
        
        static void Main(string[] args)
        {
            try
            {
                SendMessageFromSocket(11000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static void SendMessageFromSocket(int port)
        {

            var tmp= new Person
           {
               FirstName = "123a",
               LastName = "4124123i"
           };

            var tmp2 = new Request
            {
                PersonId = 2,
                Time = DateTime.Today,
                Req = "Vasyaaaa"
            };

            //сериализация объекта 
            var bytes = tmp.Serialization();
            var bytes2 = tmp2.Serialization();

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);

            Console.Write("Введите сообщение: ");
         //   string message = Console.ReadLine();

            Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());
            byte[] msg = bytes;
            byte[] msg2 = bytes2;
            //Отправляем данные через сокет
            int bytesSent = sender.Send(msg2);
            bytesSent = sender.Send(msg2);


            // Получаем ответ от сервера
            //   int bytesRec = sender.Receive(bytes);

            //    Console.WriteLine("\nОтвет от сервера: {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

            // Используем рекурсию для неоднократного вызова SendMessageFromSocket()
            //   if (message.IndexOf("<TheEnd>") == -1)
            //       SendMessageFromSocket(port);

            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}