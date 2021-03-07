using System;
using System.Collections.Generic;
using System.Linq;
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

using Scripts.Pages;

namespace Scripts
{
    public partial class MainWindow : Window
    {
        //Конструктор создания окна
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                //Событие для закрытия окна
                CloseClass.CloseMainWindow += () => { Close(); };

                //События для изменения размеров окна
                CloseClass.ChangeMaxWin += () => { WindowState = WindowState.Maximized; };
                CloseClass.ChangeNormalWin += () => { WindowState = WindowState.Normal; };

                //Событие для скрытия окна(Не работает)
                CloseClass.HideWin += () => { Visibility = Visibility.Hidden; };

                MoveClass.MainPage += () => { if (myFrame.NavigationService.Source.OriginalString != "Pages/Main.xaml") { myFrame.NavigationService.Navigate(new Uri("Pages/Main.xaml", UriKind.Relative)); } };
                MoveClass.ScriptPage += () => { if (myFrame.NavigationService.Source.OriginalString != "Pages/Script.xaml") { myFrame.NavigationService.Navigate(new Uri("Pages/Script.xaml", UriKind.Relative)); } };
                MoveClass.EditPage += () => { if (myFrame.NavigationService.Source.OriginalString != "Pages/Edit.xaml") { myFrame.NavigationService.Navigate(new Uri("Pages/Edit.xaml", UriKind.Relative)); } };

                //Загрузка скриптов
                Classes.Scripter.Load();

                if (Classes.Scripter.All.Width > 0) { Width = Classes.Scripter.All.Width; }
                if (Classes.Scripter.All.Height > 0) { Height = Classes.Scripter.All.Height; }

                //Передача начальной страницы(Main)
                myFrame.NavigationService.Navigate(new Uri("Pages/Main.xaml", UriKind.Relative));
            }
            catch (Exception ex) { $"[MainWindow]: error initialize MainWindow({ex.Message})".Log(); }
        }

        //Для передвижения окна
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    //Метод для передвижения окна
                    DragMove();
                }
            }
            catch (Exception ex) { $"[MainWindow]: error move window({ex.Message})".Log(); }
        }

        //Изменения свойства при изменении размеров окна
        private void State_StateChanged(object sender, EventArgs e)
        {
            try { CloseClass.IsFullScreen = !CloseClass.IsFullScreen; } catch (Exception ex) { $"[MainWindow]: error change state window({ex.Message})".Log(); }
        }

        //Отчистка фильтров при закрытии окна
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try { FiltersClass.filters.Clear(); } catch (Exception ex) { $"[MainWindow]: error closing window({ex.Message})".Log(); }
        }
    }

    //Класс для меню
    public static class CloseClass
    {
        //Событие для закрытия окна
        public static event Action CloseMainWindow;

        //Событи для изменения размеров окна
        public static event Action ChangeMaxWin;
        public static event Action ChangeNormalWin;

        //Событие для скрытия окна
        public static event Action HideWin;

        //Определения размеров окна
        public static bool IsFullScreen = false;

        //Метод для закрытия окна
        public static void CloseMain()
        {
            try { CloseMainWindow(); } catch { }
        }

        //Метод для увеличения окна
        public static void ChangeMax()
        {
            try { ChangeMaxWin(); } catch { }
        }

        //Метод для возврата в нормальное положения окна
        public static void ChangeNormal()
        {
            try { ChangeNormalWin(); } catch { }
        }

        //Метод для скрытия окна
        public static void Hide()
        {
            try { HideWin(); } catch { }
        }

        //Метод для изменения окна
        public static void ChangeSize()
        {
            if (!IsFullScreen) { try { ChangeMaxWin(); } catch { } } else { try { ChangeNormalWin(); } catch { } }
        }
    }

    //Класс для пермещения между страницами
    public static class MoveClass
    {
        //События для перхода между страницами
        public static event Action MainPage;
        public static event Action ScriptPage;
        public static event Action EditPage;

        //Метод для перехода на главную страницу 
        public static void MoveToMain()
        {
            try { MainPage(); } catch { }
        }

        //Метод для перехода на страницу просмотра
        public static void MoveToScript()
        {
            try { ScriptPage(); } catch { }
        }

        //Метод для перехода на страницу редактирования
        public static void MoveToEdit()
        {
            try { EditPage(); } catch { }
        }
    }

    //Класс для хранения фильтров
    public static class FiltersClass
    {
        //Для хранения фильтров, при смене страницы
        public static List<string> filters = new List<string>();
    }

    //Класс для дополнения
    public static class Addition
    {
        public static void Log(this string log)
        {
            try { System.IO.File.AppendAllText(@"Saves\Error.log", "[" + DateTime.Now.ToString() + "]" + log + " \n"); } catch { }
        }
    }
}
