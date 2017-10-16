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

        public static bool Authorizat(string userName, NetworkStream stream)
        {
            Person person = new Person(userName, userName, 000);
            bool flag = false;
            DataTravel travelPerson = new DataTravel(person, "Authorizat");
            // преобразуем сообщение в массив байтов
            byte[] data = travelPerson.Serialization();
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

            byte[] data = reqTravel.Serialization();
            if (stream.CanWrite)
                stream.Write(data, 0, data.Length);
            else Console.WriteLine("Занято");

        }

        
    }
}