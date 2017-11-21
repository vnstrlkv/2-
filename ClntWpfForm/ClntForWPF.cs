using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using DataBase;

using Xceed.Wpf.Toolkit;
using Xceed.Wpf.DataGrid;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace SocketClient
{

   public class Client 
    {
        const int port = 8888;
        const string address = "127.0.0.1";
        bool connected = false;
        public NetworkStream stream;
        public Person person = new Person();

        TcpClient client;

        public  bool Connected
        {
            get { return connected; }
            set {
                if (connected != value)
                {
                    connected = value;
                    if (Connected)
                    {
                        ConnectionTrue();
                        AutorizationChange();
                    }
                    else
                    {
                        AutorizationChange();
                        ConnectionFalse();
                    }
                }
            }
        }

        public void Connect ()
            {
            try
            {
                if (!Connected)
                {
                    client = new TcpClient(address, port);
                    stream = client.GetStream();
                    Connected = true;

                }
            }
            catch (Exception)
            {
         //       Connected = false;
            }
        }

        public void Disconnect()
        {
            if (Connected)
            {
               
                    stream.Close();//отключение потока                  
                    client.Close();//отключение клиента       
                    Connected = false;
            }
        }

        public bool Authorizat(string userName, string password)
        {
            person.UserName = userName;
            person.Password = password;
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
        public bool Register()
        {

            return false;
        }

        public void AddRequest(string request)
        {

            DataTravel reqTravel = new DataTravel(new Request(person, request), "addRequest");

            byte[] data = reqTravel.Serialization();
            if (stream.CanWrite)
                stream.Write(data, 0, data.Length);
            else Console.WriteLine("Занято");

        }


        //события
        // Подключение к серверу
        public delegate void ConnectionTrueHandler();
        public event ConnectionTrueHandler ConnectionTrue;

        public delegate void ConnectionFalseHandler();
        public event ConnectionFalseHandler ConnectionFalse;

        public delegate void ConnectionAutorizationHandler();
        public event ConnectionAutorizationHandler AutorizationChange;


    }

    public class ClientView
    {
        public static Client client = new Client();

    }

}