using System;
using SQLite;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DataBase;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace server
{
    class Server
    {
        static Database db = new Database("test.db");


        public static bool OperationWithData(byte[] data, NetworkStream stream)
        {
            bool flag = false;

            DataTravel dataTravel = new DataTravel();
            dataTravel = dataTravel.DeSerialization(data);
            switch (dataTravel.command)
            {
                case "Authorizat":
                    {
                        Authorizat(dataTravel, stream);
                        break;
                    }
                case "addRequest":
                    {
                        AddRequest(dataTravel, stream);
                        break;
                    }
                case "resgister":
                    {
                        Register(dataTravel, stream);
                        break;
                    }
                case "deletePerson":
                    {

                        break;
                    }
                case "deleteRequest":
                    {

                        break;
                    }

            }

            return flag;
        }

        public static bool Authorizat(DataTravel dataTravel, NetworkStream stream)
        {
            bool flag = false;
            var personID = db.ExecuteScalar<String>("SELECT * FROM Person WHERE UserName = ? AND Password = ?", dataTravel.travelPerson.UserName, dataTravel.travelPerson.Password);
            Console.WriteLine("Per={0}", personID);

            byte[] data = new byte[2];
            data[0] = 0;
            Console.WriteLine("Происходит Авторизация");
            if (personID != null)
            {

                    // отправляем обратно, что аторизация успешна

                    data[0] = 1;
                    data[1] = Convert.ToByte(personID);
                    flag = true;
                }
        
        else 
        {                Console.WriteLine("Невозможно Авторизоваться");}

            if (stream.CanWrite)
            {
                stream.Write(data, 0, data.Length);
            }
            else Console.WriteLine("Занято");

            return flag;
        }
        public static bool AddRequest(DataTravel dataTravel, NetworkStream stream)
        {
            db.Insert(dataTravel.travelRequest);
            db.ViewTable();
            return true;
        }
        public static bool Register(DataTravel dataTravel, NetworkStream stream)
        {
            db.Insert(dataTravel.travelRequest);
            db.ViewTable();
            return true;
        }


        public class ClientObject
        {
            public TcpClient client;
            public ClientObject(TcpClient tcpClient)
            {
                Console.WriteLine("Новое успешное подключение");
                client = tcpClient;
            }

            public void Process()
            {

                NetworkStream stream = null;
                try
                {

                    stream = client.GetStream();
                    byte[] data = new byte[1024]; // буфер для получаемых данных
                    while (true)
                    {
                        // получаем сообщение
                        //      StringBuilder builder = new StringBuilder();
                        int countbyte;
                        do
                        {

                            countbyte = stream.Read(data, 0, data.Length);
                        }
                        while (stream.DataAvailable);
                        if (countbyte == 0)
                        {
                            stream.Close();
                            client.Close();
                            Console.WriteLine("Отключение прошло успешно");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Получение потока");

                            OperationWithData(data, stream);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {

                    if (stream != null)
                    {
                        stream.Close();
                    }
                    if (client != null)
                    {
                        client.Close();
                    }
                }
            }
        }

        const int port = 8888;
        static TcpListener listener;
        static void Main(string[] args)
        {
            db.Execute("DROP TABLE Person");
            db.Execute("DROP TABLE Request");
            Person person = new Person("asd", "asd1", "asd", "asd", 123);
            Person person2 = new Person("555", "5551", "555", "555", 123);
            db.CreateTable<Person>();
            db.CreateTable<Request>();
            db.Insert(person);
            db.Insert(person2);

            db.ViewTable();




            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                listener.Start();
                Console.WriteLine("Ожидание подключений...");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);

                    // создаем новый поток для обслуживания нового клиента
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }

    }
}