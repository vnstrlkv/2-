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



    [Serializable]
    public class DBCommand
    {
        // Десериализация 
        public dynamic DeSerialization (byte[] bytes)
        {      
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(1, SeekOrigin.Begin);
            //определение типа принятого объекта // 0 - person,  1- Request 
            if (bytes[0] == 0)
                return (Person)formatter.Deserialize(stream);
            else if (bytes[0] == 1)
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
            byte[] msg = new byte[tmp.Length + 1];

            if (this is Person)
                msg[0] = 0;
            else if (this is Request)
                msg[0] = 1;

            for (int i = 1; i <= tmp.Length; i++)
                msg[i] = tmp[i - 1];
            return msg;
        }

        //подключение к серверу для отпарвки/принятия запроса//сервер слушает всегда
        public void AddRequest ()
        {
            
        }
        //вывод в консоле 
        public bool ViewTable()
        {
            bool flag = false;

            return flag;
        }




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

    }

    public class Database : SQLiteConnection
    {
        public Database(string path) : base(new SQLitePlatformWin32(), path)
        {
            CreateTable<Person>();
            CreateTable<Request>();
        }

    }
}
