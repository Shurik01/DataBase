using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class Book : INotifyPropertyChanged  // для уведомлений
    {
        /// <summary>
        /// поле Id 
        /// </summary>
        private int _id { get; set; }
        /// <summary>
        /// поле Title
        /// </summary>
        private string _title { get; set; }
        /// <summary>
        /// поле Author
        /// </summary>
        private string _author { get; set; }
        /// <summary>
        /// конструктор класса Book
        /// </summary>
        /// <param name="_Id"> Id книги</param>
        /// <param name="_Title">Название книги</param>
        /// <param name="_Author">Автор книги</param>
        public Book(int _Id, string _Title, string _Author) { Id = _Id; Title = _Title; Author = _Author; }
        /// <summary>
        /// публичное свойство 
        /// </summary>
        public int Id
        {
            get => _id;    // значение поля _id
            // value - параметр сеттера, который содержит значение нового свойства
            set { _id = value; OnPropertyChanged(); }   // меняет значение, уведомляет об изменении 
        }
        /// <summary>
        /// публичное свойство
        /// </summary>
        public string Title
        {
            get => _title;  // значение поля _title
            set { _title = value; OnPropertyChanged(); }    // меняет значение, уведомляет об изменении
        }
        /// <summary>
        /// публчиное свойство
        /// </summary>
        public string Author
        {
            get => _author; //
            set { _author = value; OnPropertyChanged(); }
        }

        // Событие, которое требует интерфейс
        public event PropertyChangedEventHandler PropertyChanged;

        // Метод, который будет уведомлять систему об изменениях
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
