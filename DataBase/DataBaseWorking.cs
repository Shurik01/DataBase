using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
namespace DataBase
{
    // класс для работы с базой данных  
    internal class DataBaseWorking
    {
        private readonly string Path = "C:\\Users\\user\\source\\repos\\DataBase\\DataBase\\Books.db";
        // поле можно назначить только во время обхявления или конструктор в классе DataBaseWorking;
        private readonly string _connection;
        public DataBaseWorking()
        {
            _connection = "Data Source=Books.db";
            InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connection))
            {
                connection.Open();
                // создает новую команду с подключением к бд
                SqliteCommand command = connection.CreateCommand();
                string commandText = "CREATE TABLE IF NOT EXISTS Books(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Title TEXT NOT NULL, Author TEXT NOT NULL)";
                command.CommandText = commandText;
                // выполняет sql-выражение и возвращает количество измененных записей
                command.ExecuteNonQuery();
            }        
        }

        public List<Book> Sort(string orderBy)
        {
            var books = new List<Book>();
            using (var connection = new SqliteConnection(_connection))
            {
                connection.Open();
                string commandText = $"SELECT * FROM Books ORDER BY {orderBy}";
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                using (SqliteDataReader reader = command.ExecuteReader()){ ;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            books.Add(new Book(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                        }
                    }
                }
            }
            return books;
        }
        public int AddBook(Book book)
        {
            using (var connection = new SqliteConnection(_connection))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                string commandText = $"INSERT INTO Books(Title, Author) VALUES('{book.Title}', '{book.Author}') RETURNING Id";
                command.CommandText = commandText;  
                int lastId = Convert.ToInt32(command.ExecuteScalar());
                return lastId;
            }
        }

        public void UpdateBook(Book book)
        {
            using (var connection = new SqliteConnection(_connection))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                string commandText = $"UPDATE Books SET Title = '{book.Title}', Author = '{book.Author}'";
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteBook(int id)
        {
            using (var connection = new SqliteConnection(_connection))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                string commandText = $"DELETE FROM Books WHERE Id = {id}";
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }

        }

    }
}
