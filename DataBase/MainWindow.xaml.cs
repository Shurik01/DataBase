using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataBase
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// переменная для работы с базой данных типа DataBaseWorking, объявление переменной
        /// </summary>
        private readonly DataBaseWorking db;
        /// <summary>
        /// коллекция книг с поддержкой уведомления об изменениях в коллекции, объявление переменной
        /// </summary>
        private ObservableCollection<Book> books;
        /// <summary>
        /// Конструктор главного окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent(); // загрузка интерфейса

            db = new DataBaseWorking(); // инициализация объекта для работы с базой данных
            books = new ObservableCollection<Book>();   // инциализация списка, где во время работы программы находятся объекты из базы данных
            LoadData("Id"); // загружаем данные из базы данных, отсортированные по Id
            booksGrid.ItemsSource = books;  // привязка данных к DataGrid
        }
        /// <summary>
        /// Загружает данные из базы данных в коллекцию books
        /// </summary>
        /// <param name="orderBy">то, в каком порядке сортируем(по Id, Title или Author)</param>
        private void LoadData(string orderBy)
        {
            books.Clear();  // очищаем коллекцию перед загрузкой новых данные
            var list = db.Sort(orderBy);    // записываем 
            foreach (var book in list)  
            {
                // подписываемся на событие
                book.PropertyChanged += Book_PropertyChanged;
                // добвляем в базу данных
                books.Add(book);
            }
        }
        /// <summary>
        /// метод для сортировки данных(событие RadioButton)
        /// </summary>
        /// <param name="sender">Источник события(RadioButton)</param>
        /// <param name="e">Параметры события</param>
        private void Sort_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return; // Защита от преждевременного вызова при старте приложения
            // если выбрана сортировка по Id
            if (IdSort.IsChecked == true)
            {
                // сортирует по Id
                LoadData("Id");   
            }
            // если выбрана сортировка по названию
            else if (NameSort.IsChecked == true)
            {
                // сортирует по Title
                LoadData("Title");
            }
            // если выбрана сортировка по автору
            else if (AuthorSort.IsChecked == true)
            {
                // сортирует по Author
                LoadData("Author");
            }
        }
        /// <summary>
        /// метод для кнопки добавить
        /// </summary>
        /// <param name="sender">Источник события(кнопка)</param>
        /// <param name="e">Параметры события</param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(title.Text != "" && author.Text != "")
            {
                Book book = new Book(0, title.Text, author.Text);
                book.Id = db.AddBook(book);
                books.Add(book);
            }

        }
        /// <summary>
        /// событие изменения свойств книги
        /// реагирует на редактирование полей Title и Author в DataGrid
        /// </summary>
        /// <param name="sender">Источник события(объект книги, свойства которой изменились)</param>
        /// <param name="e">Параметры события(какое свойство изменилось)</param>
        private void Book_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // преобразование типа
            Book changedBook = sender as Book;

            // Проверяем, какое именно свойство изменилось
            if (e.PropertyName == nameof(changedBook.Title) || e.PropertyName == nameof(changedBook.Author))
            {
                // обновляем базу данных
                db.UpdateBook(changedBook);
            }
        }
        /// <summary>
        /// Обработчик события нажатия кнопки удалить
        /// </summary>
        /// <param name="sender">Источник события(кнопка)</param>
        /// <param name="e">Параметры события</param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // выделенное поле как книгу
            Book selectedBook = booksGrid.SelectedItem as Book;
            // если выбрана
            if (selectedBook != null)
            {
                // подтверждение удаления
                 MessageBoxResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить этот элемент?",
                    "Подтверждение действия",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                // если хотят удалить
                if (result == MessageBoxResult.Yes)
                {
                    // удаление элемента из базы данных
                    db.DeleteBook(selectedBook.Id);

                    // Отписываемся от события, чтобы избежать утечек памяти
                    selectedBook.PropertyChanged -= Book_PropertyChanged;

                    // Удаляем из коллекции
                    books.Remove(selectedBook);
                }
            }
        }
    }
}
