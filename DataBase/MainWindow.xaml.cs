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
        private readonly DataBaseWorking db;
        private ObservableCollection<Book> books;
        public MainWindow()
        {
            InitializeComponent();

            db = new DataBaseWorking();
            LoadData("Id");

            booksGrid.ItemsSource = books;
        }
        private void LoadData(string orderBy)
        {
            var list = db.Sort(orderBy);
            books = new ObservableCollection<Book>(list);
        }
        private void Sort_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return; // Защита от преждевременного вызова при старте приложения

            if (IdSort.IsChecked == true)
            {
                LoadData("Id");   
            }
            else if (NameSort.IsChecked == true)
            {
                LoadData("Title");
            }
            else if (AuthorSort.IsChecked == true)
            {
                LoadData("Author");
            }
            booksGrid.ItemsSource = books;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(title.Text != "" && author.Text != "")
            {
                Book book = new Book(0, title.Text, author.Text);
                book.Id = db.AddBook(book);
                books.Add(book);
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
