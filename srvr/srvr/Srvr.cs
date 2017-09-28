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
       
        public static bool Authorizat(byte [] data, NetworkStream stream)
        {
            bool flag = false;
            DBCommand comand=new DBCommand();
            var person = comand.DeSerialization(data);
            
            var persons = db.ExecuteScalar<String>("SELECT * FROM Person WHERE FirstName = ?",person.FirstName);
            if (persons != null)
            {
                Console.WriteLine("ОТправка");
                // отправляем обратно, что аторизация успешна
                data = new byte[1];
                data[0] = 1;
                if (stream.CanWrite)
                    stream.Write(data, 0, data.Length);
                else Console.WriteLine("Занято");
                flag = true;
            }
            else
            {
                data = new byte[1];
                data[0] = 0;
                if (stream.CanWrite)
                {
                    Console.WriteLine("Отправка fail");
                    stream.Write(data, 0, data.Length);
                }
                else Console.WriteLine("Занято");
             
            }
            stream.Flush();
            return flag;
        }
        public static bool AddRequest(byte[] data, NetworkStream stream)
        {
            DBCommand command = new DBCommand();
            Request request = command.DeSerialization(data);
            Console.WriteLine("Десер");
            db.Insert(request);
            db.ViewTable();
            stream.Flush();
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
                    for(int i=0;i>-1;i++)
                    {
                        // получаем сообщение
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0;
                        if (stream.CanRead)
                            do
                            {
                                Console.WriteLine("Получение потока");
                                stream.Read(data, 0, data.Length);
                            }
                            while (stream.DataAvailable);
                        else Console.WriteLine("Не может быть считан");
                       
                        while (stream.DataAvailable);
     
                        switch (data[0])
                        {
                            case 0:
                                {
                                    Authorizat(data, stream);
                                    break;
                                }
                            case 1:
                                {
                                    AddRequest(data, stream);
                                    break;
                                }

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
                        Console.WriteLine("stream != null");
                        stream.Close();
                    }
                    if (client != null)
                    {
                  //      client.Close();
                        Console.WriteLine("client != null");
                    }
                }
            }
        }

        const int port = 8888;
        static TcpListener listener;
        static void Main(string[] args)
        {
            db.Execute("DROP TABLE Person");
            Person person = new Person("asd", "asd", 123);
            db.CreateTable<Person>();
            db.Insert(person);
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