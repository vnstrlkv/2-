using System;
using SQLite;
using System.Text;
using System.Net;
using System.Net.Sockets;
using DataBase;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace srvr
{
    class Srvr
    {
 
        static void Main(string[] args)
        {
            Database db;
            db = new Database("test.db");
        /*    db.CreateTable<Person>();

            db.CreateTable<Request>();

            var person = new Person
            {
                FirstName = "asfsdfasf",
                LastName = "adasdad"               
            };
            db.Insert(person);
            var Val = new Request
            {
                PersonId = 1,
                Time = DateTime.Today,
                Req = "123123"
            };
            db.Insert(Val);
            foreach (var il in db.Table<Request>())
            {
                Console.WriteLine(il);
            }
            foreach (var il in db.Table<Person>())
            {
                Console.WriteLine(il);
            }

            var persons = db.Query<Request>("SELECT * FROM Request WHERE PersonId = 1");
            foreach (var il in persons)
            Console.WriteLine(il);
        
    */
            
                        // Устанавливаем для сокета локальную конечную точку
                        IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                        IPAddress ipAddr = ipHost.AddressList[0];
                        IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

                        // Создаем сокет Tcp/Ip
                        Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
                        try
                        {
                            sListener.Bind(ipEndPoint);
                            sListener.Listen(10);

                            // Начинаем слушать соединения
                            while (true)
                            {
                                Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                                // Программа приостанавливается, ожидая входящее соединение
                                Socket handler = sListener.Accept();
                                byte[] data =new byte [1024];

                    // Мы дождались клиента, пытающегося с нами соединиться
                    var obj = new Serial();
                    byte[] bytes = new byte[1024];
                    handler.Receive(bytes);

                    var tmp=obj.DeSerialization(bytes);
                    

                    // Показываем данные на консоли

                    db.Insert(tmp);
                   



                    foreach (var il in db.Table<Person>())
                    {
                        Console.WriteLine(il);
                    }
                    foreach (var il in db.Table<Request>())
                    {
                        Console.WriteLine(il);
                    }

                    // Отправляем ответ клиенту\
                    /*         string reply = "Спасибо за запрос в " + data.Length.ToString()
                                                 + " символов";
                                         byte[] msg = Encoding.UTF8.GetBytes(reply);
                                         handler.Send(msg);
                                         */
                    //  if (data.IndexOf("<TheEnd>") > -1)
                    //   {
                    ///        Console.WriteLine("Сервер завершил соединение с клиентом.");
                    ///       break;
                    ///    }

                    handler.Shutdown(SocketShutdown.Both);
                                handler.Close();
                            }
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
    }
}