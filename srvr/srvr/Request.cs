using System;
using SQLite;


namespace srvr
{
    class Request
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        [MaxLength(30), NotNull]
        public string FirstName { get; set; }

        [MaxLength(30), NotNull]
        public string LastName { get; set; }

        [NotNull]
        public DateTime BirthDate { get; set; }

        [Ignore]
        public string FullName
        {
            get
            {
                return string.Format(
                    "{0} {1}",
                    LastName,
                    FirstName
                );
            }
        }

        public override string ToString()
        {
            return string.Format(
                "{0}: {1} {2}",
                Id,
                FullName,
                BirthDate.ToString("dd-MM-yyyy")
            );
        }
    }
}
