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
            DataTravel travelPerson = new DataTravel(person, "Authorizat");
            // преобразуем сообщение в массив байтов
            byte[] data = travelPerson.Serialization();
             // отправка сообщения

                stream.Write(data, 0, data.Length);

            // получаем ответ
            data = new byte[1024]; // буфер для получаемых данных
 
                do
                {
                    Console.WriteLine("Получение потока");
                    stream.Read(data, 0, data.Length);
                }
                while (stream.DataAvailable);

            if (data[0] == 1)
            {
                person.Id = data[1];
                Console.WriteLine("Удачно");
                flag = true;
            }
            else Console.WriteLine("Неудача");

            return flag;
        }
        public static void AddRequest(Person person, NetworkStream stream)
        {
            Console.WriteLine("Введите запрос");
            string request = Console.ReadLine();

            DataTravel reqTravel = new DataTravel(new Request(person, request), "addRequest");

            byte[] data= reqTravel.Serialization();

                stream.Write(data, 0, data.Length);


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
                        Console.Write("Ошибка авторизации:");
                        Console.Write("Введите свое имя:");
                      
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

                if (client != null)
                    client.Close();
                
            }
        }
    }
}