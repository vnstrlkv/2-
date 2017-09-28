using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using Path = System.IO.Path;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SQLite;
using System.Globalization;
namespace DataBase
{
    //0 байт тип команды/ 1 байт подтип команды / 3 байт тип объекта


    [Serializable]
    public class DBCommand 
    {
        public DBCommand(string firstName, string lastName, int phoneNumber) { }
        public DBCommand(Person per, string str) { }
        public DBCommand() { }

        // Десериализация 
        public dynamic DeSerialization (byte[] bytes)
        {      
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(1, SeekOrigin.Begin);
            //определение типа принятого объекта // 0 - person,  1- Request 
            if (bytes[3] == 0)
                return (Person)formatter.Deserialize(stream);
            else if (bytes[3] == 1)
                return (Request)formatter.Deserialize(stream);
            else return null;
        }

        //Сериализация объекта для передачи
        public byte[] Serialization()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, this);
            byte[] tmp = stream.ToArray();
            //добавление флага для определения типа переданного объекта
            byte[] msg = new byte[tmp.Length + 3];
            msg[0] = 9;
            msg[1] = 9;
            if (this is Person)
                msg[2] = 0;
            else if (this is Request)
                msg[2] = 1;

            for (int i = 3; i <= tmp.Length; i++)
                msg[i] = tmp[i - 3];
            return msg;
        }


        //создание полей


        //Добавление полей в базуданных
        public void AddRequestToDB (Database db, Request req)
        {
            if (req!=null)
            db.Insert(req);
        }
        public void AddPersonToDB(Database db, Person per)
        {
            if (per != null)
                db.Insert(per);
        }
        //вывод в консоле 





    }



    [Serializable]
    public class Person :DBCommand
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }

        public override string ToString()
        {
            return string.Format("{0:} | {1:} | {2:} | {3:}", Id, FirstName, LastName, PhoneNumber);
        }


        public Person() { }

        public Person( string firstName, string lastName, int phoneNumber) : base (firstName, lastName, phoneNumber)
        {
            FirstName = firstName;
            LastName = LastName;
            PhoneNumber = phoneNumber;
        }
        
    }

    [Serializable]
    public class Request :DBCommand
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int PersonId { get; set; }

        public DateTime Time { get; set; }
        public string Req { get; set; }

        public override string ToString()
        {
            return string.Format("{0:} {1:MMM dd yy}    {2:}", Id, Time, Req);
        }


        public Request() { }
        public Request( Person per, string req) : base (per, req)
        {
            Time = DateTime.Today;
            PersonId = per.Id;
            Req = req;
        }

    }

    public class Database : SQLiteConnection
    {
        public Database(string path) : base(new SQLitePlatformWin32(), path)
        {
            CreateTable<Person>();
            CreateTable<Request>();
        }

        public void ViewTable()
        {
            Console.WriteLine("Person_______");
            foreach (var il in this.Table<Person>())
            {
                Console.WriteLine(il);
            }
            Console.WriteLine("Request_______");
            foreach (var il in this.Table<Request>())
            {
                Console.WriteLine(il);
            }
        }
    }
}
