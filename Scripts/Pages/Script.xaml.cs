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

using Scripts.Classes;
using System.IO;
using System.Diagnostics;

namespace Scripts.Pages
{
    public partial class Script : Page
    {
        //Свойство наличия аудиа
        private bool HaveAudio = false;
        //Номер 1-го файла отображения
        private int numberMinFile = -1;
        //проиграватель аудио
        private MediaPlayer player = new MediaPlayer();

        //Конструктор создания страницы
        public Script()
        {
            try
            {
                InitializeComponent();

                LoadScript();
            }
            catch (Exception ex) { $"[Script]: error initialize Script({ex.Message})".Log(); }
        }

        //Закрытие окна при нажатие кнопки
        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            try { CloseClass.CloseMain(); } catch (Exception ex) { $"[Script]: error close window({ex.Message})".Log(); }
        }

        //Изменение размеров окна при нажатие кнопки
        private void Full_Click(object sender, MouseButtonEventArgs e)
        {
            try { CloseClass.ChangeSize(); } catch (Exception ex) { $"[Script]: error change size window({ex.Message})".Log(); }
        }

        //Скрытие окна при нажатие кнопки
        private void Hide_Click(object sender, MouseButtonEventArgs e)
        {
            try { CloseClass.Hide(); } catch (Exception ex) { $"[Script]: error hide window({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки(Полный список...)
        private void All_more_MouseEnter(object sender, MouseEventArgs e)
        {
            try { All_more.Fill = (Brush)new BrushConverter().ConvertFromString("#FFD3CA"); } catch (Exception ex) { $"[Script]: error to change all more color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки(Полный список...)
        private void All_more_MouseLeave(object sender, MouseEventArgs e)
        {
            try { All_more.Fill = (Brush)new BrushConverter().ConvertFromString("#FFA08B"); } catch (Exception ex) { $"[Script]: error to change all more color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки(Left_Tab)
        private void Left_Tab_MouseEnter(object sender, MouseEventArgs e)
        {
            try { Left_Tab.Fill = (Brush)new BrushConverter().ConvertFromString("#FFEDE9"); } catch (Exception ex) { $"[Script]: error to change left tab color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки(Left_Tab)
        private void Left_Tab_MouseLeave(object sender, MouseEventArgs e)
        {
            try { Left_Tab.Fill = (Brush)new BrushConverter().ConvertFromString("#FFC3B6"); } catch (Exception ex) { $"[Script]: error to change left tab color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки(Right_Tab)
        private void Right_Tab_MouseEnter(object sender, MouseEventArgs e)
        {
            try { Right_Tab.Fill = (Brush)new BrushConverter().ConvertFromString("#FFEDE9"); } catch (Exception ex) { $"[Script]: error to change right tab color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки(Right_Tab)
        private void Right_Tab_MouseLeave(object sender, MouseEventArgs e)
        {
            try { Right_Tab.Fill = (Brush)new BrushConverter().ConvertFromString("#FFC3B6"); } catch (Exception ex) { $"[Script]: error to change right tab color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки(Audio)
        private void Audio_MouseEnter(object sender, MouseEventArgs e)
        {
            try { Audio.Fill = (Brush)new BrushConverter().ConvertFromString("#EDF5FF"); } catch (Exception ex) { $"[Script]: error to change audio color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки(Audio)
        private void Audio_MouseLeave(object sender, MouseEventArgs e)
        {
            try { Audio.Fill = (Brush)new BrushConverter().ConvertFromString("#DFEEFF"); } catch (Exception ex) { $"[Script]: error to change audio color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки(Delet)
        private void Delet_MouseEnter(object sender, MouseEventArgs e)
        {
            try { Delet.Fill = (Brush)new BrushConverter().ConvertFromString("#F69393"); } catch (Exception ex) { $"[Script]: error to change delet color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки(Delet)
        private void Delet_MouseLeave(object sender, MouseEventArgs e)
        {
            try { Delet.Fill = (Brush)new BrushConverter().ConvertFromString("#F47B7B"); } catch (Exception ex) { $"[Script]: error to change delet color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки(Exit)
        private void Exit_MouseEnter(object sender, MouseEventArgs e)
        {
            try { Exit.Fill = (Brush)new BrushConverter().ConvertFromString("#F69393"); } catch (Exception ex) { $"[Script]: error to change exit color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки(Exit)
        private void Exit_MouseLeave(object sender, MouseEventArgs e)
        {
            try { Exit.Fill = (Brush)new BrushConverter().ConvertFromString("#F47B7B"); } catch (Exception ex) { $"[Script]: error to change exit color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат на главную страницу(Exit)
        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try { MoveClass.MoveToMain(); } catch (Exception ex) { $"[Script]: error to enter exit(Rectangle)({ex.Message})".Log(); }
        }

        //Загрузка скрипта
        private void LoadScript()
        {
            try
            {
                NameScript.Text = Scripter.SelectedScript.NameScript;

                if (Scripter.SelectedScript.PlaceFiles.Count > 0)
                {
                    OpenFiles.Visibility = Visibility.Visible;

                    Files.Visibility = Visibility.Visible;

                    LeftTab.Visibility = Visibility.Visible;
                    RightTab.Visibility = Visibility.Visible;

                    LeftTab.IsEnabled = false;
                    RightTab.IsEnabled = false;

                    switch (Scripter.SelectedScript.PlaceFiles.Count)
                    {
                        case 1:
                            if (Scripter.SelectedScript.PlaceFiles[0].type == FileType.Photo) { Image_1.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[0].path))); } else { Image_1.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                            Image_1.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[0].path) };
                            break;
                        case 2:
                            if (Scripter.SelectedScript.PlaceFiles[0].type == FileType.Photo) { Image_1.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[0].path))); } else { Image_1.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                            Image_1.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[0].path) };
                            if (Scripter.SelectedScript.PlaceFiles[1].type == FileType.Photo) { Image_2.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[1].path))); } else { Image_2.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                            Image_2.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[1].path) };
                            break;
                        case 3:
                            if (Scripter.SelectedScript.PlaceFiles[0].type == FileType.Photo) { Image_1.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[0].path))); } else { Image_1.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                            Image_1.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[0].path) };
                            if (Scripter.SelectedScript.PlaceFiles[1].type == FileType.Photo) { Image_2.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[1].path))); } else { Image_2.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                            Image_2.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[1].path) };
                            if (Scripter.SelectedScript.PlaceFiles[2].type == FileType.Photo) { Image_3.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[2].path))); } else { Image_3.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                            Image_3.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[2].path) };
                            break;
                        default:
                            if (Scripter.SelectedScript.PlaceFiles[0].type == FileType.Photo) { Image_1.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[0].path))); } else { Image_1.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                            Image_1.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[0].path) };
                            if (Scripter.SelectedScript.PlaceFiles[1].type == FileType.Photo) { Image_2.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[1].path))); } else { Image_2.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                            Image_2.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[1].path) };
                            if (Scripter.SelectedScript.PlaceFiles[2].type == FileType.Photo) { Image_3.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[2].path))); } else { Image_3.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                            Image_3.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[2].path) };

                            LeftTab.Visibility = Visibility.Hidden;
                            RightTab.Visibility = Visibility.Visible;

                            LeftTab.IsEnabled = true;
                            RightTab.IsEnabled = true;

                            numberMinFile = 0;
                            break;
                    }
                }

                if (Scripter.SelectedScript.PlaceAudio != null && Scripter.SelectedScript.PlaceAudio != "" && Scripter.SelectedScript.PlaceAudio != " ") { Grid_Audio.Visibility = Visibility.Visible; player.Open(new Uri(Scripter.SelectedScript.PlaceAudio, UriKind.Relative)); Name_Audio.Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceAudio); HaveAudio = true; }

                bool isHaveScript = false;

                if (Scripter.SelectedScript.Scripting != null && Scripter.SelectedScript.Scripting != "" && Scripter.SelectedScript.Scripting != " ")
                {
                    isHaveScript = true;

                    Script_Code.Visibility = Visibility.Visible;

                    using (var reader = new StreamReader(Scripter.SelectedScript.Scripting))
                    {
                        Text_script.Text = reader.ReadToEnd();
                    }
                }

                bool isHaveVisualCode = false;

                if (Scripter.SelectedScript.VisualCode != null && Scripter.SelectedScript.VisualCode != "" && Scripter.SelectedScript.VisualCode != " ")
                {
                    isHaveVisualCode = true;

                    Visual_Code.Visibility = Visibility.Visible;

                    using (var reader = new StreamReader(Scripter.SelectedScript.VisualCode))
                    {
                        Text_Visual_Code.Text = reader.ReadToEnd();
                    }
                }

                if ((isHaveVisualCode == false && isHaveScript == true) || (isHaveVisualCode == true && isHaveScript == false))
                {
                    if (isHaveVisualCode == false)
                    {
                        Grid.SetRowSpan(Script_Code, 2);
                    }
                    else
                    {
                        Grid.SetRowSpan(Visual_Code, 2);
                        Grid.SetRow(Visual_Code, 0);
                    }
                }
            }
            catch (Exception ex) { $"[Script]: error to relaod scripts({ex.Message})".Log(); }
        }

        //Запуск аудио
        private void Audio_Play_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try { if (HaveAudio == true) { try { player.Position = TimeSpan.MinValue; player.Play(); } catch { } } } catch (Exception ex) { $"[Script]: error to play audio({ex.Message})".Log(); }
        }

        //Открытие папки с файлами
        private void OpenFiles_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try { if (Directory.Exists($@"Saves\{Scripter.SelectedScript.NameScript}\Files")) { Process.Start($@"Saves\{Scripter.SelectedScript.NameScript}\Files"); } } catch (Exception ex) { $"[Script]: error to open direcrory Files({ex.Message})".Log(); }
        }

        //Копирование текста скрипта в буфер обмена
        private void Text_Script_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Clipboard.SetText(Text_script.Text);

                MessageBox.Show("Скрипт скопирован");
            }
            catch (Exception ex) { $"[Script]: error to copy text script({ex.Message})".Log(); }
        }

        //Копирование текста визуального кода в буфер обмена
        private void Text_Visual_Code_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Clipboard.SetText(Text_Visual_Code.Text);

                MessageBox.Show("Визуальный код скопирован");
            }
            catch (Exception ex) { $"[Script]: error to copy text visual code({ex.Message})".Log(); }
        }

        //Удаление скрипта
        private void Delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы точно хотите удалить скрипт", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Scripter.DeleteScript(Scripter.NumberSelectedScript);
                    Scripter.SortMainScripts();
                    Scripter.All.MaxNum--;
                    Scripter.Save();
                    Scripter.Sort();

                    MoveClass.MoveToMain();
                }
            }
            catch (Exception ex) { $"[Script]: error to delete script({ex.Message})".Log(); }
        }

        //Прокрутка файла влево
        private void LeftTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (numberMinFile > -1)
                {
                    numberMinFile--;

                    if (numberMinFile == 0) { LeftTab.Visibility = Visibility.Hidden; }
                    if (numberMinFile + 2 < Scripter.SelectedScript.PlaceFiles.Count) { RightTab.Visibility = Visibility.Visible; }

                    if (Scripter.SelectedScript.PlaceFiles[numberMinFile].type == FileType.Photo) { Image_1.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[numberMinFile].path))); } else { Image_1.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                    Image_1.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[numberMinFile].path) };
                    if (Scripter.SelectedScript.PlaceFiles[numberMinFile + 1].type == FileType.Photo) { Image_2.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[numberMinFile + 1].path))); } else { Image_2.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                    Image_2.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[numberMinFile + 1].path) };
                    if (Scripter.SelectedScript.PlaceFiles[numberMinFile + 2].type == FileType.Photo) { Image_3.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[numberMinFile + 2].path))); } else { Image_3.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                    Image_3.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[numberMinFile + 2].path) };
                }
            }
            catch (Exception ex) { $"[Script]: error to svap to left(Left_Tab)({ex.Message})".Log(); }
        }

        //Прокрутка файла вправо
        private void RightTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (numberMinFile > -1)
                {
                    numberMinFile++;

                    if (numberMinFile > 0) { LeftTab.Visibility = Visibility.Visible; }
                    if (numberMinFile + 3 >= Scripter.SelectedScript.PlaceFiles.Count) { RightTab.Visibility = Visibility.Hidden; }

                    if (Scripter.SelectedScript.PlaceFiles[numberMinFile].type == FileType.Photo) { Image_1.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[numberMinFile].path))); } else { Image_1.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                    Image_1.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[numberMinFile].path) };
                    if (Scripter.SelectedScript.PlaceFiles[numberMinFile + 1].type == FileType.Photo) { Image_2.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[numberMinFile + 1].path))); } else { Image_2.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                    Image_2.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[numberMinFile + 1].path) };
                    if (Scripter.SelectedScript.PlaceFiles[numberMinFile + 2].type == FileType.Photo) { Image_3.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(Scripter.SelectedScript.PlaceFiles[numberMinFile + 2].path))); } else { Image_3.Fill = (Brush)new BrushConverter().ConvertFromString("#DC4646"); }
                    Image_3.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(Scripter.SelectedScript.PlaceFiles[numberMinFile + 2].path) };
                }
            }
            catch (Exception ex) { $"[Script]: error to svap to right(Right_Tab)({ex.Message})".Log(); }
        }

        //Открытие изменения скрипта
        private void Script_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.F5)
                {
                    Scripter.IsSaves = true;

                    MoveClass.MoveToEdit();
                }
            }
            catch (Exception ex) { $"[Script]: error to open Edit script({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки(Edit)
        private void Edit_MouseEnter(object sender, MouseEventArgs e)
        {
            try { Edit.Fill = (Brush)new BrushConverter().ConvertFromString("#ECFFDD"); } catch (Exception ex) { $"[Script]: error to change edit color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки(Edit)
        private void Edit_MouseLeave(object sender, MouseEventArgs e)
        {
            try { Edit.Fill = (Brush)new BrushConverter().ConvertFromString("#D3EAC5"); } catch (Exception ex) { $"[Script]: error to change edit color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Открытие изменения скрипта
        private void Edit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try { Scripter.IsSaves = true; MoveClass.MoveToEdit(); } catch (Exception ex) { $"[Script]: error to enter edit(Rectangle)({ex.Message})".Log(); }
        }
    }
}
