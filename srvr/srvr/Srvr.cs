using System;
using SQLite;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DataBase;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//byte [0]=0 - авторизация
//byte [0]=1 - добавление request
namespace srvr
{
    class Srvr
    {
       static Database db=new Database("test.db");


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
                case "addPerson":
                    {

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
            
            var persons = db.ExecuteScalar<String>("SELECT * FROM Person WHERE FirstName = ?",dataTravel.travelPerson.FirstName);
            Console.WriteLine(persons);
            byte[] data = new byte[2];
            if (persons != null)
            {
                Console.WriteLine("ОТправка");
                // отправляем обратно, что аторизация успешна
              
                data[0] = 1;
                data[1] = Convert.ToByte(persons);
                if (stream.CanWrite)
                    stream.Write(data, 0, data.Length);
                else Console.WriteLine("Занято");

                flag = true;
            }
            else
            {
                data[0] = 0;
                if (stream.CanWrite)
                {
                    Console.WriteLine("Отправка fail");
                    stream.Write(data, 0, data.Length);
                }
                else Console.WriteLine("Занято");
             
            }
            return flag;
        }
        public static bool AddRequest(DataTravel dataTravel, NetworkStream stream)
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
                        StringBuilder builder = new StringBuilder();
                        if (stream.CanRead)
                            do
                            {
                                Console.WriteLine("Получение потока");
                                stream.Read(data, 0, data.Length);
                            }
                            while (stream.DataAvailable);
                        else Console.WriteLine("Не может быть считан");
                       
                        while (stream.DataAvailable);
                        OperationWithData(data, stream);                     
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
            Person person = new Person("asd", "asd", 123);
            Person person2 = new Person("555", "555", 123);
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