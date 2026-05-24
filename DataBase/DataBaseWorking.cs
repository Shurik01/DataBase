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
    // работа осуществляется с базой данных Books, полями которой являются:
    // Id(первичный ключ, автоматически назначающийся),
    // Title(название книги, not null)
    // Author(автор книги, not null)
    internal class DataBaseWorking
    {
        /// поле можно назначить только во время обхявления или конструктор в классе DataBaseWorking;
        private readonly string _connection;
        /// <summary>
        /// конструктор класса DataBaseWorking
        /// </summary>
        public DataBaseWorking()
        {
            _connection = "Data Source=Books.db";   // куда подключаться, файл находится в папке с проектом
            InitializeDatabase();   // инициализирует базу данных
        }
        /// <summary>
        /// инциализация баззы данных
        /// </summary>
        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connection))
            {
                connection.Open();  // открытие подключения
                // создает новую sql-команду с подключением к бд
                SqliteCommand command = connection.CreateCommand();
                // запрос к базе данных(создает таблицу Books, если такой не существует) 
                string commandText = "CREATE TABLE IF NOT EXISTS Books(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Title TEXT NOT NULL, Author TEXT NOT NULL)";
                command.CommandText = commandText;
                // выполняет sql-выражение и возвращает количество измененных записей
                command.ExecuteNonQuery();
            }        
        }
        /// <summary>
        /// функция Sort для сортировки
        /// </summary>
        /// <param name="orderBy">то, по какому параметру сортируем(Id, Title или Author)</param>
        /// <returns>возвращает отсортированный список значений из нашей БД</returns>
        public List<Book> Sort(string orderBy)
        {
            var books = new List<Book>();   //список, где будут хранится отсортированные данные
            using (var connection = new SqliteConnection(_connection))
            {
                connection.Open();  // открывает подключение к БД
                string commandText = $"SELECT * FROM Books ORDER BY {orderBy}"; // запрос к БД
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                using (SqliteDataReader reader = command.ExecuteReader()){ ;    // переменная reader используется для чтения, ExecuteReader возвращает объект типа SqliteDataReader
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // читаем пока есть данные
                        {
                            books.Add(new Book(reader.GetInt32(0), reader.GetString(1), reader.GetString(2))); // добавляем прочитанное в список
                        }
                    }
                }
            }
            return books;
        }
        /// <summary>
        /// добавление данных в базу данных
        /// </summary>
        /// <param name="book">данные, которые хотим добавить</param>
        /// <returns>id последнего добавленного объекта</returns>
        public int AddBook(Book book)
        {
            using (var connection = new SqliteConnection(_connection))
            {
                connection.Open(); // открываем подключение
                SqliteCommand command = connection.CreateCommand();
                // Sql запрос
                string commandText = $"INSERT INTO Books(Title, Author) VALUES('{book.Title}', '{book.Author}') RETURNING Id";
                command.CommandText = commandText;  
                int lastId = Convert.ToInt32(command.ExecuteScalar());
                return lastId;
            }
        }
        /// <summary>
        /// изменяет данные в базе данных
        /// </summary>
        /// <param name="book">объект, который изменяем</param>
        public void UpdateBook(Book book)
        {
            using (var connection = new SqliteConnection(_connection))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand(); 
                // sql-запрос
                string commandText = $"UPDATE Books SET Title = '{book.Title}', Author = '{book.Author}' WHERE Id = '{book.Id}'";
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// удаление объекта из базы данных
        /// </summary>
        /// <param name="id">id объекта, который удаляем</param>
        public void DeleteBook(int id)
        {
            using (var connection = new SqliteConnection(_connection))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                // sql-запрос
                string commandText = $"DELETE FROM Books WHERE Id = {id}";
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }

        }

    }
}
