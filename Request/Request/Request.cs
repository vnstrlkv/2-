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
    //0 байт тип команды/ 1 байт подтип команды / 2 байт тип объект
    [Serializable]
    public class DataTravel 
    {
        public Person travelPerson;
        public Request travelRequest;
        public Type typeTravelClass;
        public string command;

        /* addRequest
         * addPerson
         * deletePerson
         * deleteRequest
         * 
         */

//Конструкторы
       public DataTravel(Request travelClass)
        {
            typeTravelClass = typeof(Request);
            travelRequest = travelClass;
            travelPerson = null;
        }
        public DataTravel(Person travelClass)
        {
            typeTravelClass = typeof(Person);
            travelPerson = travelClass;
            travelRequest = null;
        }

        public DataTravel(Person travelClass, string command)
        {
            this.travelPerson = travelClass;
            travelRequest = null;
            this.command = command;
        }
        public DataTravel(Request travelClass, string command)
        {
            this.travelRequest = travelClass;
            travelPerson = null;
            this.command = command;
        }
        public DataTravel(string command)
        {
            this.travelRequest = null;
            travelPerson = null;
            typeTravelClass = null;
            this.command = command;
        }

        public DataTravel ()
        {

        }


        //Сериализация объекта для передачи
        public byte[] Serialization()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, this);
            byte[] travelMsg = stream.ToArray();
            return travelMsg;
        }

        // Десериализация 
        public DataTravel DeSerialization(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return (DataTravel)formatter.Deserialize(stream);                 
        }

    }

  
    [Serializable]
    public class Person
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

        public Person( string firstName, string lastName, int phoneNumber)
        {
            FirstName = firstName;
            LastName = LastName;
            PhoneNumber = phoneNumber;
        }
        
    }

    [Serializable]
    public class Request 
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int PersonId { get; set; }

        public DateTime Time { get; set; }
        public string Req { get; set; }

        public override string ToString()
        {
            return string.Format("{0:} {1:} {2:MMM dd yy}    {3:}", Id, PersonId, Time, Req);
        }


        public Request() { }
        public Request( Person per, string req)
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
