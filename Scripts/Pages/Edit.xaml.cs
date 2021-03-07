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

using Microsoft.Win32;

namespace Scripts.Pages
{
    public partial class Edit : Page
    {
        //Key скрипта
        private long numberScript;
        //Все фильтры скрипта(0 фильтр - название скрипта)
        private List<string> filters = new List<string>();
        //Расположение файлов
        private List<FilePath> paths = new List<FilePath>();
        //Расположение аудио
        private string audioPath = "";
        //Расположение скрипта
        private string scriptPath = "";
        //Расположение физуального кода
        private string visualCodePath = "";

        //Конструктор создания страницы
        public Edit()
        {
            try
            {
                InitializeComponent();

                if (Scripter.IsSaves == true) { LoadScript(); }
            }
            catch (Exception ex) { $"[Edit]: error initialize Edit({ex.Message})".Log(); }
        }

        //Закрытие окна при нажатие кнопки
        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            try { CloseClass.CloseMain(); } catch (Exception ex) { $"[Edit]: error close window({ex.Message})".Log(); }
        }

        //Изменение размеров окна при нажатие кнопки
        private void Full_Click(object sender, MouseButtonEventArgs e)
        {
            try { CloseClass.ChangeSize(); } catch (Exception ex) { $"[Edit]: error change size window({ex.Message})".Log(); }
        }

        //Скрытие окна при нажатие кнопки
        private void Hide_Click(object sender, MouseButtonEventArgs e)
        {
            try { CloseClass.Hide(); } catch (Exception ex) { $"[Edit]: error hide window({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки(Exit)
        private void Exit_MouseEnter(object sender, MouseEventArgs e)
        {
            try { Exit.Fill = (Brush)new BrushConverter().ConvertFromString("#F69393"); } catch (Exception ex) { $"[Edit]: error to change exit color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки(Exit)
        private void Exit_MouseLeave(object sender, MouseEventArgs e)
        {
            try { Exit.Fill = (Brush)new BrushConverter().ConvertFromString("#F47B7B"); } catch (Exception ex) { $"[Edit]: error to change exit color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат на главную страницу
        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы точно хотите выйти и не сохранить скрипт?", "Сохранение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    MoveClass.MoveToMain();
                }
                else
                {
                    Classes.Script script = new Classes.Script();

                    if (filters.Count > 0) { script.NameScript = filters[0]; } else { script.NameScript = DateTime.Now.ToString(); }

                    script.Tags = filters;

                    script.PlaceFiles = paths;

                    script.PlaceAudio = audioPath;

                    if (scriptPath != null && scriptPath != "" && scriptPath != " ") { script.Scripting = Script_File_Editor.Text; }

                    if (visualCodePath != null && visualCodePath != "" && visualCodePath != " ") { script.VisualCode = Visual_Code_File_Editor.Text; }

                    script.TimeSave = DateTime.Now;

                    Scripter.AddScript(script);

                    Scripter.Save();

                    MoveClass.MoveToMain();
                }
            }
            catch (Exception ex) { $"[Edit]: error back to main page({ex.Message})".Log(); }
        }

        //Отчистка текста, при помощьи выподающего списка
        private void MenuItem_Click_Clear(object sender, RoutedEventArgs e)
        {
            try { Name_Script.Text = ""; } catch (Exception ex) { $"[Edit]: error clear name script text({ex.Message})".Log(); }
        }

        //Добавление текста из буфер обмена в TextBox, при помощьи выподающего списка
        private void MenuItem_Click_Add(object sender, RoutedEventArgs e)
        {
            try { Name_Script.Text = Clipboard.GetText(); } catch (Exception ex) { $"[Edit]: error add text to name script({ex.Message})".Log(); }
        }

        //Добавление текста из TextBox в буфер обмена, при помощьи выподающего списка
        private void MenuItem_Click_Copy(object sender, RoutedEventArgs e)
        {
            try { Clipboard.SetText(Name_Script.Text); } catch (Exception ex) { $"[Edit]: error copy text from name script({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки(Audio)
        private void Audio_MouseEnter(object sender, MouseEventArgs e)
        {
            try { Audio.Fill = (Brush)new BrushConverter().ConvertFromString("#EDF5FF"); } catch (Exception ex) { $"[Edit]: error to change audio color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки(Audio)
        private void Audio_MouseLeave(object sender, MouseEventArgs e)
        {
            try { Audio.Fill = (Brush)new BrushConverter().ConvertFromString("#DFEEFF"); } catch (Exception ex) { $"[Script]: error to change audio color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки(Save_Script)
        private void Save_Script_MouseEnter(object sender, MouseEventArgs e)
        {
            try { Save_Script.Fill = (Brush)new BrushConverter().ConvertFromString("#ECFFDD"); } catch (Exception ex) { $"[Edit]: error to change save script color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки(Save_Script)
        private void Save_Script_MouseLeave(object sender, MouseEventArgs e)
        {
            try { Save_Script.Fill = (Brush)new BrushConverter().ConvertFromString("#D9F3C4"); } catch (Exception ex) { $"[Edit]: error to change save script color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Метод добавление фильтра
        private void Name_Script_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Name_Script.Text != null && Name_Script.Text != "" && Name_Script.Text != " ")
                    {
                        //Добавление филтра
                        if (Cheak(Name_Script.Text)) { filters.Add(Name_Script.Text); Filters.Visibility = Visibility.Visible; }

                        Reload_Filters();

                        Name_Script.Text = "";
                    }
                }
            }
            catch (Exception ex) { $"[Edit]: error to add name script(Rectangle)({ex.Message})".Log(); }
        }

        //Проверка на оригинальность нового фильтра
        private bool Cheak(string filter)
        {
            try { if (filters.Where(thisFilter => filter == thisFilter).Count() > 0) { return false; } else { return true; } } catch (Exception ex) { $"[Edit]: error to cheak filter({ex.Message})".Log(); return true; }
        }

        //Проверка на оригинальность нового фильтра
        private bool CheakFile(string path)
        {
            try { if (paths.Where(thisPath => path == thisPath.path).Count() > 0) { return false; } else { return true; } } catch (Exception ex) { $"[Edit]: error to cheak file({ex.Message})".Log(); return true; }
        }

        //Перезагрузка фильтра
        private void Reload_Filters()
        {
            try
            {
                //Добавление фильтра
                string name = Grid_Filter_0.Name.Remove(10);

                //Отчиска предыдущих
                Filters.Children.Clear();

                for (int i = 0; i < filters.Count; i++)
                {
                    //Создание контейнера компонентов
                    Grid grid = new Grid();
                    grid.Name = name + i.ToString();

                    //Создание и копирование свойст(задний фон)
                    Rectangle child = Grid_Filter_0.Children[0] as Rectangle;

                    Rectangle rectangle = new Rectangle() { Fill = child.Fill, AllowDrop = child.AllowDrop, Visibility = child.Visibility, CacheMode = child.CacheMode, Clip = child.Clip, ClipToBounds = child.ClipToBounds, Effect = child.Effect, Focusable = child.Focusable, IsEnabled = child.IsEnabled, IsHitTestVisible = child.IsHitTestVisible, IsManipulationEnabled = child.IsManipulationEnabled, Opacity = child.Opacity, OpacityMask = child.OpacityMask, RenderSize = child.RenderSize, RenderTransform = child.RenderTransform, RenderTransformOrigin = child.RenderTransformOrigin, SnapsToDevicePixels = child.SnapsToDevicePixels, Uid = child.Uid, BindingGroup = child.BindingGroup, ContextMenu = child.ContextMenu, Cursor = child.Cursor, DataContext = child.DataContext, FlowDirection = child.FlowDirection, FocusVisualStyle = child.FocusVisualStyle, ForceCursor = child.ForceCursor, Height = child.Height, HorizontalAlignment = child.HorizontalAlignment, InputScope = child.InputScope, Language = child.Language, LayoutTransform = child.LayoutTransform, Margin = child.Margin, MaxHeight = child.MaxHeight, MaxWidth = child.MaxWidth, MinHeight = child.MinHeight, MinWidth = child.MinWidth, Name = child.Name.Remove(child.Name.Length - 4) + "_" + i, OverridesDefaultStyle = child.OverridesDefaultStyle, RadiusX = child.RadiusX, RadiusY = child.RadiusY, Resources = child.Resources, Stretch = child.Stretch, Stroke = child.Stroke, StrokeDashArray = child.StrokeDashArray, StrokeDashCap = child.StrokeDashCap, StrokeDashOffset = child.StrokeDashOffset, StrokeEndLineCap = child.StrokeEndLineCap, StrokeLineJoin = child.StrokeLineJoin, StrokeMiterLimit = child.StrokeMiterLimit, StrokeStartLineCap = child.StrokeStartLineCap, StrokeThickness = child.StrokeThickness, Style = child.Style, Tag = child.Tag, ToolTip = child.ToolTip, UseLayoutRounding = child.UseLayoutRounding, VerticalAlignment = child.VerticalAlignment, Width = child.Width };

                    foreach (Trigger item in child.Triggers)
                    {
                        rectangle.Triggers.Add(item);
                    }

                    foreach (InputBinding item in child.InputBindings)
                    {
                        rectangle.InputBindings.Add(item);
                    }

                    foreach (CommandBinding item in child.CommandBindings)
                    {
                        rectangle.CommandBindings.Add(item);
                    }

                    rectangle.MouseDown += Rectangle_Filter_MouseDown;
                    rectangle.MouseEnter += Rectangle_Filter_MouseEnter;
                    rectangle.MouseLeave += Rectangle_Filter_MouseLeave;

                    //Добавление элемента в контейнер
                    grid.Children.Add(rectangle);


                    //Создание и копирование свойст(текст)
                    TextBlock childTBlock = Grid_Filter_0.Children[1] as TextBlock;

                    TextBlock textBlock = new TextBlock() { AllowDrop = childTBlock.AllowDrop, Visibility = childTBlock.Visibility, CacheMode = childTBlock.CacheMode, Clip = childTBlock.Clip, ClipToBounds = childTBlock.ClipToBounds, Effect = childTBlock.Effect, Focusable = childTBlock.Focusable, IsEnabled = childTBlock.IsEnabled, IsHitTestVisible = childTBlock.IsHitTestVisible, IsManipulationEnabled = childTBlock.IsManipulationEnabled, Opacity = childTBlock.Opacity, OpacityMask = childTBlock.OpacityMask, RenderSize = childTBlock.RenderSize, RenderTransform = childTBlock.RenderTransform, RenderTransformOrigin = childTBlock.RenderTransformOrigin, SnapsToDevicePixels = childTBlock.SnapsToDevicePixels, Uid = childTBlock.Uid, BindingGroup = childTBlock.BindingGroup, ContextMenu = childTBlock.ContextMenu, Cursor = childTBlock.Cursor, DataContext = childTBlock.DataContext, Background = childTBlock.Background, FlowDirection = childTBlock.FlowDirection, FocusVisualStyle = childTBlock.FocusVisualStyle, ForceCursor = childTBlock.ForceCursor, Height = childTBlock.Height, HorizontalAlignment = childTBlock.HorizontalAlignment, InputScope = childTBlock.InputScope, Language = childTBlock.Language, LayoutTransform = childTBlock.LayoutTransform, Margin = childTBlock.Margin, MaxHeight = childTBlock.MaxHeight, MaxWidth = childTBlock.MaxWidth, MinHeight = childTBlock.MinHeight, MinWidth = childTBlock.MinWidth, Name = childTBlock.Name.Remove(childTBlock.Name.Length - 4) + "_" + i, OverridesDefaultStyle = childTBlock.OverridesDefaultStyle, Resources = childTBlock.Resources, Style = childTBlock.Style, Tag = childTBlock.Tag, ToolTip = childTBlock.ToolTip, UseLayoutRounding = childTBlock.UseLayoutRounding, VerticalAlignment = childTBlock.VerticalAlignment, Width = childTBlock.Width, Text = filters[i], FontFamily = childTBlock.FontFamily, FontSize = childTBlock.FontSize, FontStretch = childTBlock.FontStretch, FontStyle = childTBlock.FontStyle, FontWeight = childTBlock.FontWeight, Foreground = childTBlock.Foreground, Padding = childTBlock.Padding, BaselineOffset = childTBlock.BaselineOffset, IsHyphenationEnabled = childTBlock.IsHyphenationEnabled, LineHeight = childTBlock.LineHeight, LineStackingStrategy = childTBlock.LineStackingStrategy, TextAlignment = childTBlock.TextAlignment, TextDecorations = childTBlock.TextDecorations, TextEffects = childTBlock.TextEffects, TextTrimming = childTBlock.TextTrimming, TextWrapping = childTBlock.TextWrapping };

                    foreach (Trigger item in childTBlock.Triggers)
                    {
                        textBlock.Triggers.Add(item);
                    }

                    foreach (InputBinding item in childTBlock.InputBindings)
                    {
                        textBlock.InputBindings.Add(item);
                    }

                    foreach (CommandBinding item in childTBlock.CommandBindings)
                    {
                        textBlock.CommandBindings.Add(item);
                    }

                    textBlock.MouseDown += TextBlock_Filter_MouseDown;
                    textBlock.MouseEnter += TextBlock_Filter_MouseEnter;
                    textBlock.MouseLeave += TextBlock_Filter_MouseLeave;

                    //Добавление элемента в контейнер
                    grid.Children.Add(textBlock);

                    grid.Margin = Grid_Filter_0.Margin;

                    grid.Visibility = Visibility.Visible;

                    //Добавление контйнера фильтра 
                    Filters.Children.Add(grid);
                }
            }
            catch (Exception ex) { $"[Edit]: error to reload filters({ex.Message})".Log(); }
        }

        //Перезагрузка файлов
        private void Reload_Files()
        {
            try
            {
                Files.Children.Clear();

                for (int i = 0; i < paths.Count; i++)
                {
                    //Создание и копирование свойст(задний фон)
                    Rectangle child = File_Copy_1 as Rectangle;

                    Rectangle rectangle = new Rectangle() { Fill = child.Fill, AllowDrop = child.AllowDrop, Visibility = child.Visibility, CacheMode = child.CacheMode, Clip = child.Clip, ClipToBounds = child.ClipToBounds, Effect = child.Effect, Focusable = child.Focusable, IsEnabled = child.IsEnabled, IsHitTestVisible = child.IsHitTestVisible, IsManipulationEnabled = child.IsManipulationEnabled, Opacity = child.Opacity, OpacityMask = child.OpacityMask, RenderSize = child.RenderSize, RenderTransform = child.RenderTransform, RenderTransformOrigin = child.RenderTransformOrigin, SnapsToDevicePixels = child.SnapsToDevicePixels, Uid = child.Uid, BindingGroup = child.BindingGroup, ContextMenu = child.ContextMenu, Cursor = child.Cursor, DataContext = child.DataContext, FlowDirection = child.FlowDirection, FocusVisualStyle = child.FocusVisualStyle, ForceCursor = child.ForceCursor, Height = child.Height, HorizontalAlignment = child.HorizontalAlignment, InputScope = child.InputScope, Language = child.Language, LayoutTransform = child.LayoutTransform, Margin = child.Margin, MaxHeight = child.MaxHeight, MaxWidth = child.MaxWidth, MinHeight = child.MinHeight, MinWidth = child.MinWidth, Name = child.Name.Remove(child.Name.Length - 4) + "_" + i, OverridesDefaultStyle = child.OverridesDefaultStyle, RadiusX = child.RadiusX, RadiusY = child.RadiusY, Resources = child.Resources, Stretch = child.Stretch, Stroke = child.Stroke, StrokeDashArray = child.StrokeDashArray, StrokeDashCap = child.StrokeDashCap, StrokeDashOffset = child.StrokeDashOffset, StrokeEndLineCap = child.StrokeEndLineCap, StrokeLineJoin = child.StrokeLineJoin, StrokeMiterLimit = child.StrokeMiterLimit, StrokeStartLineCap = child.StrokeStartLineCap, StrokeThickness = child.StrokeThickness, Style = child.Style, Tag = child.Tag, ToolTip = child.ToolTip, UseLayoutRounding = child.UseLayoutRounding, VerticalAlignment = child.VerticalAlignment, Width = child.Width };

                    if (paths[i].type == FileType.Photo) { rectangle.Fill = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(paths[i].path))); } else { rectangle.MouseEnter += Rectangle_File_MouseEnter; rectangle.MouseLeave += Rectangle_File_MouseLeave; }

                    rectangle.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(paths[i].path) };

                    foreach (Trigger item in child.Triggers)
                    {
                        rectangle.Triggers.Add(item);
                    }

                    foreach (InputBinding item in child.InputBindings)
                    {
                        rectangle.InputBindings.Add(item);
                    }

                    foreach (CommandBinding item in child.CommandBindings)
                    {
                        rectangle.CommandBindings.Add(item);
                    }

                    rectangle.MouseDown += Rectangle_File_MouseDown;

                    rectangle.Visibility = Visibility.Visible;
                    //Добавление элемента в контейнер
                    Files.Children.Add(rectangle);
                }
            }
            catch (Exception ex) { $"[Edit]: error to relaod files({ex.Message})".Log(); }
        }

        //Загрузка скрипта(Происходит при переходе из Script)
        private void LoadScript()
        {
            try
            {
                if (Scripter.SelectedScript.Tags != null)
                {
                    if (Scripter.SelectedScript.Tags.Count > 0)
                    {
                        filters = Scripter.SelectedScript.Tags.Where((x) => { return true; }).ToList();
                        Filters.Visibility = Visibility.Visible;
                        Reload_Filters();
                    }
                }

                if (Scripter.SelectedScript.PlaceFiles != null)
                {
                    if (Scripter.SelectedScript.PlaceFiles.Count > 0)
                    {
                        Scripter.SelectedScript.PlaceFiles.ForEach(x => paths.Add(new FilePath() { path = x.path, type = x.type }));
                        Files.Visibility = Visibility.Visible;
                        Reload_Files();
                    }
                }

                if (Scripter.SelectedScript.PlaceAudio != null && Scripter.SelectedScript.PlaceAudio != "" && Scripter.SelectedScript.PlaceAudio != " ")
                {
                    audioPath = Scripter.SelectedScript.PlaceAudio;
                    Grid_Audio.Visibility = Visibility.Visible;
                    Audio.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(audioPath) };
                    Name_Audio.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(audioPath) };
                    Name_Audio.Content = System.IO.Path.GetFileName(audioPath);
                }

                if (Scripter.SelectedScript.Scripting != null && Scripter.SelectedScript.Scripting != "" && Scripter.SelectedScript.Scripting != " ")
                {
                    scriptPath = Scripter.SelectedScript.Scripting;

                    using (var reader = new StreamReader(scriptPath))
                    {
                        Script_File_Editor.Text = reader.ReadToEnd();
                        Script_File_Editor.IsEnabled = true;
                        Script_File_Editor.VerticalContentAlignment = VerticalAlignment.Top;
                        Script_File_Editor.HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                }

                if (Scripter.SelectedScript.VisualCode != null && Scripter.SelectedScript.VisualCode != "" && Scripter.SelectedScript.VisualCode != " ")
                {
                    visualCodePath = Scripter.SelectedScript.VisualCode;

                    using (var reader = new StreamReader(visualCodePath))
                    {
                        Visual_Code_File_Editor.Text = reader.ReadToEnd();
                        Visual_Code_File_Editor.IsEnabled = true;
                        Visual_Code_File_Editor.VerticalContentAlignment = VerticalAlignment.Top;
                        Visual_Code_File_Editor.HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                }

                numberScript = Scripter.NumberSelectedScript;
            }
            catch (Exception ex) { $"[Edit]: error to load script({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки
        private void Rectangle_Filter_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (Filters.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as Rectangle).Name.ToString().Remove(0, (sender as Rectangle).Name.ToString().Length - 1));
                    try { if (Filters.Children[number] != null) { ((Filters.Children[number] as Grid).Children[0] as Rectangle).Fill = (Brush)new BrushConverter().ConvertFromString("#F2EDF3"); } } catch { }
                }
            }
            catch (Exception ex) { $"[Edit]: error to change filter color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки
        private void Rectangle_Filter_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (Filters.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as Rectangle).Name.ToString().Remove(0, (sender as Rectangle).Name.ToString().Length - 1));
                    try { if (Filters.Children[number] != null) { ((Filters.Children[number] as Grid).Children[0] as Rectangle).Fill = (Brush)new BrushConverter().ConvertFromString("#F9ECFE"); } } catch { }
                }
            }
            catch (Exception ex) { $"[Edit]: error to change filter color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Удаление фильтра, при нажатии на него
        private void Rectangle_Filter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (Filters.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as Rectangle).Name.ToString().Remove(0, (sender as Rectangle).Name.ToString().Length - 1));
                    try
                    {
                        filters.Remove(((Filters.Children[number] as Grid).Children[1] as Label).Content.ToString());
                        if (filters.Count == 0) { Filters.Visibility = Visibility.Collapsed; }
                        Reload_Filters();
                    }
                    catch { }
                }
            }
            catch (Exception ex) { $"[Edit]: error to enter filter(Rectangle)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки
        private void TextBlock_Filter_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (Filters.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as TextBlock).Name.ToString().Remove(0, (sender as TextBlock).Name.ToString().Length - 1));
                    try { if (Filters.Children[number] != null) { ((Filters.Children[number] as Grid).Children[0] as Rectangle).Fill = (Brush)new BrushConverter().ConvertFromString("#F2EDF3"); } } catch { }
                }
            }
            catch (Exception ex) { $"[Edit]: error to change filter color enter(TextBlock)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки
        private void TextBlock_Filter_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (Filters.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as TextBlock).Name.ToString().Remove(0, (sender as TextBlock).Name.ToString().Length - 1));
                    try { if (Filters.Children[number] != null) { ((Filters.Children[number] as Grid).Children[0] as Rectangle).Fill = (Brush)new BrushConverter().ConvertFromString("#F9ECFE"); } } catch { }
                }
            }
            catch (Exception ex) { $"[Edit]: error to change filter color leave(TextBlock)({ex.Message})".Log(); }
        }

        //Удаление фильтра, при нажатии на него
        private void TextBlock_Filter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (Filters.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as TextBlock).Name.ToString().Remove(0, (sender as TextBlock).Name.ToString().Length - 1));

                    try
                    {
                        filters.Remove(((Filters.Children[number] as Grid).Children[1] as TextBlock).Text.ToString());
                        if (filters.Count == 0) { Filters.Visibility = Visibility.Collapsed; }
                        Reload_Filters();
                    }
                    catch { }
                }
            }
            catch (Exception ex) { $"[Edit]: error to enter filter(TextBlock)({ex.Message})".Log(); }
        }

        //Метод перетаскивания файлов
        private void File_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string pathFile = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                if (CheakFile(pathFile))
                {
                    if (e.Data.GetDataPresent(DataFormats.Bitmap) || e.Data.GetDataPresent(DataFormats.CommaSeparatedValue) || e.Data.GetDataPresent(DataFormats.Dib) || e.Data.GetDataPresent(DataFormats.Dif) || e.Data.GetDataPresent(DataFormats.EnhancedMetafile) || e.Data.GetDataPresent(DataFormats.Html) || e.Data.GetDataPresent(DataFormats.Locale) || e.Data.GetDataPresent(DataFormats.MetafilePicture)
                    || e.Data.GetDataPresent(DataFormats.OemText) || e.Data.GetDataPresent(DataFormats.Palette) || e.Data.GetDataPresent(DataFormats.PenData) || e.Data.GetDataPresent(DataFormats.Riff) || e.Data.GetDataPresent(DataFormats.Rtf) || e.Data.GetDataPresent(DataFormats.Serializable) || e.Data.GetDataPresent(DataFormats.StringFormat) || e.Data.GetDataPresent(DataFormats.SymbolicLink)
                    || e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetDataPresent(DataFormats.Tiff) || e.Data.GetDataPresent(DataFormats.UnicodeText) || e.Data.GetDataPresent(DataFormats.WaveAudio) || e.Data.GetDataPresent(DataFormats.Xaml) || e.Data.GetDataPresent(DataFormats.XamlPackage))
                    {
                        paths.Add(new FilePath() { path = pathFile, type = FileType.File });
                    }
                    else
                    {
                        if (pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "jpg" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "jpeg" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "tiff" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "raw" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "dng" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "png" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "gif" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "bmp" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "psd")
                        {
                            paths.Add(new FilePath() { path = pathFile, type = FileType.Photo });
                        }
                        else { paths.Add(new FilePath() { path = pathFile, type = FileType.File }); }
                    }
                }

                if (paths.Count > 0) { Files.Visibility = Visibility.Visible; }
                Reload_Files();
            }
            catch (Exception ex) { $"[Edit]: error to file drop({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки
        private void Rectangle_File_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (Files.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as Rectangle).Name.ToString().Remove(0, (sender as Rectangle).Name.ToString().Length - 1));
                    try { if (Files.Children[number] != null) { (Files.Children[number] as Rectangle).Fill = (Brush)new BrushConverter().ConvertFromString("#FFC3C3"); } } catch { }
                }
            }
            catch (Exception ex) { $"[Edit]: error to change file color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки
        private void Rectangle_File_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (Files.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as Rectangle).Name.ToString().Remove(0, (sender as Rectangle).Name.ToString().Length - 1));
                    try { if (Files.Children[number] != null) { (Files.Children[number] as Rectangle).Fill = (Brush)new BrushConverter().ConvertFromString("#FF8686"); } } catch { }
                }
            }
            catch (Exception ex) { $"[Edit]: error to change file color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Удаление фильтра, при нажатии на него
        private void Rectangle_File_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (Files.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as Rectangle).Name.ToString().Remove(0, (sender as Rectangle).Name.ToString().Length - 1));
                    try
                    {
                        paths.RemoveAt(number);
                        if (paths.Count == 0) { Files.Visibility = Visibility.Collapsed; }
                        Reload_Files();
                    }
                    catch { }
                }
            }
            catch (Exception ex) { $"[Edit]: error to enter file(Rectangle)({ex.Message})".Log(); }
        }

        //Метод перетаскивания аудио
        private void Audio_Drop(object sender, DragEventArgs e)
        {
            try
            {
                audioPath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                if (audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "wav" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "flac" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "ape" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "alac" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "mp3" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "wma" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "ogg" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "acc" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "aiff")
                {
                    Grid_Audio.Visibility = Visibility.Visible;

                    Audio.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(audioPath) };
                    Name_Audio.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(audioPath) };

                    Name_Audio.Content = System.IO.Path.GetFileName(audioPath);
                }
            }
            catch (Exception ex) { $"[Edit]: error to audio drop({ex.Message})".Log(); }
        }

        //Проишрование аудио
        private void Player_Audio_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                MediaPlayer player = new MediaPlayer();
                player.Open(new Uri(audioPath, UriKind.Relative));

                player.Play();
            }
            catch (Exception ex) { $"[Edit]: error to play audio({ex.Message})".Log(); }
        }

        //Удаление аудио
        private void Audio_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Grid_Audio.Visibility = Visibility.Collapsed;
                audioPath = "";
            }
            catch (Exception ex) { $"[Edit]: error to delete audio({ex.Message})".Log(); }
        }

        //Добавление файла через FileDialog
        private void Files_Open_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "";
                openFileDialog.FilterIndex = 4;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    string pathFile = openFileDialog.FileName;

                    if (CheakFile(pathFile))
                    {
                        if (pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "jpg" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "jpeg" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "tiff" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "raw" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "dng" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "png" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "gif" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "bmp" || pathFile.Split('.')[pathFile.Split('.').Length - 1].ToLower() == "psd")
                        {
                            paths.Add(new FilePath() { path = pathFile, type = FileType.Photo });
                        }
                        else { paths.Add(new FilePath() { path = pathFile, type = FileType.File }); }

                        if (paths.Count > 0) { Files.Visibility = Visibility.Visible; }
                        Reload_Files();
                    }
                }
            }
            catch (Exception ex) { $"[Edit]: error to open file dialog({ex.Message})".Log(); }
        }

        //Добавление аудио через FileDialog
        private void Audio_Open_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "";
                openFileDialog.FilterIndex = 4;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    audioPath = openFileDialog.FileName;

                    if (audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "wav" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "flac" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "ape" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "alac" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "mp3" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "wma" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "ogg" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "acc" || audioPath.Split('.')[audioPath.Split('.').Length - 1].ToLower() == "aiff")
                    {
                        Grid_Audio.Visibility = Visibility.Visible;

                        Audio.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(audioPath) };
                        Name_Audio.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(audioPath) };

                        Name_Audio.Content = System.IO.Path.GetFileName(audioPath);
                    }
                }
            }
            catch (Exception ex) { $"[Edit]: error to open audio dialog({ex.Message})".Log(); }
        }

        //Метод перетаскивания скрипта
        private void Script_Drop(object sender, DragEventArgs e)
        {
            try
            {
                scriptPath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                try
                {
                    using (var reader = new StreamReader(scriptPath))
                    {
                        Script_File_Editor.Text = reader.ReadToEnd();
                        Script_File_Editor.IsEnabled = true;
                        Script_File_Editor.VerticalContentAlignment = VerticalAlignment.Top;
                        Script_File_Editor.HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                }
                catch { }
            }
            catch (Exception ex) { $"[Edit]: error to script drop({ex.Message})".Log(); }
        }

        //Метод перетаскивания визуального кода
        private void Visual_Code_Drop(object sender, DragEventArgs e)
        {
            try
            {
                visualCodePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                try
                {
                    using (var reader = new StreamReader(visualCodePath))
                    {
                        Visual_Code_File_Editor.Text = reader.ReadToEnd();
                        Visual_Code_File_Editor.IsEnabled = true;
                        Visual_Code_File_Editor.VerticalContentAlignment = VerticalAlignment.Top;
                        Visual_Code_File_Editor.HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                }
                catch { }
            }
            catch (Exception ex) { $"[Edit]: error to visual code drop({ex.Message})".Log(); }
        }

        //Добавление скрипта через FileDialog и его отчистка
        private void Script_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //Отчистка
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Script_File_Editor.Text = "";
                    Script_File_Editor.IsEnabled = true;
                    Script_File_Editor.VerticalContentAlignment = VerticalAlignment.Top;
                    Script_File_Editor.HorizontalContentAlignment = HorizontalAlignment.Left;
                }
                //Добавление скрипта через FileDialog
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.InitialDirectory = "C:\\";
                    openFileDialog.Filter = "";
                    openFileDialog.FilterIndex = 4;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == true)
                    {
                        scriptPath = openFileDialog.FileName;

                        Script_File_Editor.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(audioPath) };

                        using (var reader = new StreamReader(scriptPath))
                        {
                            Script_File_Editor.Text = reader.ReadToEnd();
                            Script_File_Editor.IsEnabled = true;
                            Script_File_Editor.VerticalContentAlignment = VerticalAlignment.Top;
                            Script_File_Editor.HorizontalContentAlignment = HorizontalAlignment.Left;
                        }
                    }
                }
            }
            catch (Exception ex) { $"[Edit]: error to open or clear script({ex.Message})".Log(); } 
        }

        //Добавление визуального кода через FileDialog и его отчистка
        private void Visual_Code_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //Отчистка
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Visual_Code_File_Editor.Text = "";
                    Visual_Code_File_Editor.IsEnabled = true;
                    Visual_Code_File_Editor.VerticalContentAlignment = VerticalAlignment.Top;
                    Visual_Code_File_Editor.HorizontalContentAlignment = HorizontalAlignment.Left;
                }
                //Добавление визуального кода через FileDialog
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.InitialDirectory = "C:\\";
                    openFileDialog.Filter = "";
                    openFileDialog.FilterIndex = 4;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == true)
                    {
                        visualCodePath = openFileDialog.FileName;

                        Visual_Code_File_Editor.ToolTip = new Label() { FontFamily = new FontFamily("Arial"), FontSize = 16, Content = System.IO.Path.GetFileName(audioPath) };

                        using (var reader = new StreamReader(visualCodePath))
                        {
                            Visual_Code_File_Editor.Text = reader.ReadToEnd();
                            Visual_Code_File_Editor.IsEnabled = true;
                            Visual_Code_File_Editor.VerticalContentAlignment = VerticalAlignment.Top;
                            Visual_Code_File_Editor.HorizontalContentAlignment = HorizontalAlignment.Left;
                        }
                    }
                }
            }
            catch (Exception ex) { $"[Edit]: error to open or clear visual code({ex.Message})".Log(); }
        }

        //Отчитка и перенос строки скрипта
        private void Script_File_Editor_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape)
                {
                    scriptPath = "";

                    Script_File_Editor.Text = "Перетащите сюда файл, или нажмите и отредактируйте текст";
                    Script_File_Editor.IsEnabled = false;
                    Script_File_Editor.VerticalContentAlignment = VerticalAlignment.Center;
                    Script_File_Editor.HorizontalContentAlignment = HorizontalAlignment.Center;
                }

                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    Script_File_Editor.Text += "\r\n";
                    Script_File_Editor.SelectionStart = Script_File_Editor.Text.Length;
                }
            }
            catch (Exception ex) { $"[Edit]: error to click script({ex.Message})".Log(); }
        }

        //Отчитка и перенос строки визуального кода
        private void Visual_Code_File_Editor_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape)
                {
                    visualCodePath = "";

                    Visual_Code_File_Editor.Text = "Перетащите сюда файл, или нажмите для редактировния визуальной части";
                    Visual_Code_File_Editor.IsEnabled = false;
                    Visual_Code_File_Editor.VerticalContentAlignment = VerticalAlignment.Center;
                    Visual_Code_File_Editor.HorizontalContentAlignment = HorizontalAlignment.Center;
                }

                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    Visual_Code_File_Editor.Text += "\r\n";
                    Visual_Code_File_Editor.SelectionStart = Visual_Code_File_Editor.Text.Length;
                }
            }
            catch (Exception ex) { $"[Edit]: error to click visual code({ex.Message})".Log(); }
        }

        //Сохранение и пересохранение скрипта
        private void Save_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы точно хотите сохранить скрипт", "Сохранение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (Scripter.IsSaves == false)
                    {
                        Classes.Script script = new Classes.Script();

                        if (filters.Count > 0) { script.NameScript = filters[0]; }

                        script.Tags = filters;

                        script.PlaceFiles = paths;

                        script.PlaceAudio = audioPath;

                        if (Script_File_Editor.Text != null && Script_File_Editor.Text != "" && Script_File_Editor.Text != " " && Script_File_Editor.Text != "Перетащите сюда файл, или нажмите и отредактируйте текст") { script.Scripting = Script_File_Editor.Text; }

                        if (Visual_Code_File_Editor.Text != null && Visual_Code_File_Editor.Text != "" && Visual_Code_File_Editor.Text != " " && Visual_Code_File_Editor.Text != "Перетащите сюда файл, или нажмите для редактировния визуальной части") { script.VisualCode = Visual_Code_File_Editor.Text; }

                        script.TimeSave = DateTime.Now;

                        numberScript = Scripter.AddScript(script);

                        Scripter.Save();

                        Scripter.IsSaves = true;
                    }
                    else
                    {
                        Classes.Script newScript = new Classes.Script();

                        if (filters.Count > 0) { newScript.NameScript = filters[0]; } else { newScript.NameScript = DateTime.Now.ToString(); }

                        newScript.Tags = filters;

                        newScript.PlaceFiles = paths;

                        newScript.PlaceAudio = audioPath;

                        if (Script_File_Editor.Text != null && Script_File_Editor.Text != "" && Script_File_Editor.Text != " " && Script_File_Editor.Text != "Перетащите сюда файл, или нажмите и отредактируйте текст") { newScript.Scripting = Script_File_Editor.Text; }

                        if (Visual_Code_File_Editor.Text != null && Visual_Code_File_Editor.Text != "" && Visual_Code_File_Editor.Text != " " && Visual_Code_File_Editor.Text != "Перетащите сюда файл, или нажмите для редактировния визуальной части") { newScript.VisualCode = Visual_Code_File_Editor.Text; }

                        newScript.TimeSave = DateTime.Now;

                        Scripter.ReSaveScript(newScript, numberScript);

                        Scripter.Save();
                    }
                }
            }
            catch (Exception ex) { $"[Edit]: error to save script({ex.Message})".Log(); }
        }
    }
}
