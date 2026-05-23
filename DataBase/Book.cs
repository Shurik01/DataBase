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
    public class Book : INotifyPropertyChanged
    {
        public Book(int _Id, string _Title, string _Author) { Id = _Id; Title = _Title; Author = _Author; }
        private int _id { get; set; }
        private string _title { get; set; }
        private string _author { get; set; }
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Author
        {
            get => _author;
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
