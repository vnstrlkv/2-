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
        public static bool Authorizat(Person person, NetworkStream stream)
        {
            bool flag = false;
    
            // преобразуем сообщение в массив байтов
            byte[] data = person.Serialization();
 

            data[0] = 0;
            data[1] = 0;
            // отправка сообщения
            if (stream.CanWrite)
                stream.Write(data, 0, data.Length);
            else Console.WriteLine("Занято");
            // получаем ответ
            data = new byte[1024]; // буфер для получаемых данных
            if (stream.CanRead)
                do
                {
                    Console.WriteLine("Получение потока");
                    stream.Read(data, 0, data.Length);
                }
                while (stream.DataAvailable);
            else Console.WriteLine("Не может быть считан");
            if (data[0] == 1)
            {
                Console.WriteLine("Удачно");
                flag = true;
            }
            else Console.WriteLine("Неудача");
            stream.Flush();
            return flag;
        }
        public static void AddRequest(Person person, NetworkStream stream)
        {
            Console.WriteLine("Введите запрос");
            string request = Console.ReadLine();

            Request req = new Request(person, request);
            byte[] data;
            data=req.Serialization();
            data[0] = 1;
            if (stream.CanWrite)
                stream.Write(data, 0, data.Length);
            else Console.WriteLine("Занято");

            stream.Flush();

        }
        const int port = 8888;
        const string address = "127.0.0.1";
        static void Main(string[] args)
        {
            //здесь будет вызов формы авторизации
            Console.Write("Введите свое имя:");
            string userName = Console.ReadLine();

            Person person=new Person (userName, userName, 000);


            TcpClient client = null;
            try
            {
                client = new TcpClient(address, port);
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    //Авторизация
                    while (Authorizat(person, stream) == false)
                    {
                        //здесь будет вызов формы авторизации
                        Console.Write("Введите свое имя222:");
                      
                        person.FirstName = Console.ReadLine();
                    }
                    Console.WriteLine("WellDOne");
                  
                    AddRequest(person, stream);
                 
                
                 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}