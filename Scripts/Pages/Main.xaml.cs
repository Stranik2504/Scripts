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

namespace Scripts.Pages
{
    public partial class Main : Page
    {
        //События перезагрузки скриптов
        private event Action reloadScripts;

        //Конструктор создания страницы
        public Main()
        {
            try
            {
                InitializeComponent();

                //Добавление фильтров в список отображения
                if (FiltersClass.filters.Count > 0) { Reload_Filters(); }

                reloadScripts += () => { Classes.Scripter.Sort(); LoadScripts(); };

                reloadScripts();
            }
            catch (Exception ex) { $"[Main]: error initialize Main({ex.Message})".Log(); }
        }

        //Закрытие окна при нажатие кнопки
        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            try { CloseClass.CloseMain(); } catch (Exception ex) { $"[Main]: error close window({ex.Message})".Log(); }
        }

        //Изменение размеров окна при нажатие кнопки
        private void Full_Click(object sender, MouseButtonEventArgs e)
        {
            try { CloseClass.ChangeSize(); } catch (Exception ex) { $"[Main]: error change size window({ex.Message})".Log(); }
        }

        //Скрытие окна при нажатие кнопки
        private void Hide_Click(object sender, MouseButtonEventArgs e)
        {
            try { CloseClass.Hide(); } catch (Exception ex) { $"[Main]: error hide window({ex.Message})".Log(); }
        }

        //Отчистка текста, при помощьи выподающего списка
        private void MenuItem_Click_Clear(object sender, RoutedEventArgs e)
        {
            try { Search.Text = ""; } catch (Exception ex) { $"[Main]: error clear Search text({ex.Message})".Log(); }
        }

        //Добавление текста из буфер обмена в TextBox, при помощьи выподающего списка
        private void MenuItem_Click_Add(object sender, RoutedEventArgs e)
        {
            try { Search.Text = Clipboard.GetText(); } catch (Exception ex) { $"[Main]: error add text to search({ex.Message})".Log(); }
        }

        //Добавление текста из TextBox в буфер обмена, при помощьи выподающего списка
        private void MenuItem_Click_Copy(object sender, RoutedEventArgs e)
        {
            try { Clipboard.SetText(Search.Text); } catch (Exception ex) { $"[Main]: error copy text from search({ex.Message})".Log(); }
        }

        //Отчистка фильтров, при помощьи выподающего списка
        private void MenuItem_Click_Filters_Clear(object sender, RoutedEventArgs e)
        {
            try
            {
                FiltersClass.filters.Clear();

                Filters.Children.Clear();
            }
            catch (Exception ex) { $"[Main]: error clear filters({ex.Message})".Log(); }
        }

        //Метод нажатия для добваления фильтра 
        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Search.Text != null && Search.Text != "" && Search.Text != " ")
                    {
                        //Добавление филтра
                        if (Cheak(Search.Text)) { FiltersClass.filters.Add(Search.Text); }

                        Reload_Filters();

                        reloadScripts();

                        Search.Text = "";
                    }
                }
            }
            catch (Exception ex) { $"[Main]: error to add filter({ex.Message})".Log(); }
        }

        //Проверка на оригинальность нового фильтра
        private bool Cheak(string filter)
        {
            try { if (FiltersClass.filters.Where(thisFilter => filter == thisFilter).Count() > 0) { return false; } else { return true; } } catch (Exception ex) { $"[Main]: error cheak filter({ex.Message})".Log(); return true; }
        }

        //Перезагрузка фильтров
        private void Reload_Filters()
        {
            try
            {
                //Добавление фильтра
                string name = Grid_Filter_0.Name.Remove(10);

                //Отчиска предыдущих
                Filters.Children.Clear();

                for (int i = 0; i < FiltersClass.filters.Count; i++)
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

                    TextBlock textBlock = new TextBlock() { AllowDrop = childTBlock.AllowDrop, Visibility = childTBlock.Visibility, CacheMode = childTBlock.CacheMode, Clip = childTBlock.Clip, ClipToBounds = childTBlock.ClipToBounds, Effect = childTBlock.Effect, Focusable = childTBlock.Focusable, IsEnabled = childTBlock.IsEnabled, IsHitTestVisible = childTBlock.IsHitTestVisible, IsManipulationEnabled = childTBlock.IsManipulationEnabled, Opacity = childTBlock.Opacity, OpacityMask = childTBlock.OpacityMask, RenderSize = childTBlock.RenderSize, RenderTransform = childTBlock.RenderTransform, RenderTransformOrigin = childTBlock.RenderTransformOrigin, SnapsToDevicePixels = childTBlock.SnapsToDevicePixels, Uid = childTBlock.Uid, BindingGroup = childTBlock.BindingGroup, ContextMenu = childTBlock.ContextMenu, Cursor = childTBlock.Cursor, DataContext = childTBlock.DataContext, Background = childTBlock.Background, FlowDirection = childTBlock.FlowDirection, FocusVisualStyle = childTBlock.FocusVisualStyle, ForceCursor = childTBlock.ForceCursor, Height = childTBlock.Height, HorizontalAlignment = childTBlock.HorizontalAlignment, InputScope = childTBlock.InputScope, Language = childTBlock.Language, LayoutTransform = childTBlock.LayoutTransform, Margin = childTBlock.Margin, MaxHeight = childTBlock.MaxHeight, MaxWidth = childTBlock.MaxWidth, MinHeight = childTBlock.MinHeight, MinWidth = childTBlock.MinWidth, Name = childTBlock.Name.Remove(childTBlock.Name.Length - 4) + "_" + i, OverridesDefaultStyle = childTBlock.OverridesDefaultStyle, Resources = childTBlock.Resources, Style = childTBlock.Style, Tag = childTBlock.Tag, ToolTip = childTBlock.ToolTip, UseLayoutRounding = childTBlock.UseLayoutRounding, VerticalAlignment = childTBlock.VerticalAlignment, Width = childTBlock.Width, Text = FiltersClass.filters[i], FontFamily = childTBlock.FontFamily, FontSize = childTBlock.FontSize, FontStretch = childTBlock.FontStretch, FontStyle = childTBlock.FontStyle, FontWeight = childTBlock.FontWeight, Foreground = childTBlock.Foreground, Padding = childTBlock.Padding, BaselineOffset = childTBlock.BaselineOffset, IsHyphenationEnabled = childTBlock.IsHyphenationEnabled, LineHeight = childTBlock.LineHeight, LineStackingStrategy = childTBlock.LineStackingStrategy, TextAlignment = childTBlock.TextAlignment, TextDecorations = childTBlock.TextDecorations, TextEffects = childTBlock.TextEffects, TextTrimming = childTBlock.TextTrimming, TextWrapping = childTBlock.TextWrapping };

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
            catch (Exception ex) { $"[Main]: error to reload filters({ex.Message})".Log(); }
        }

        //Перезагрузка скриптов
        private void LoadScripts()
        {
            try
            {
                //Добавление фильтра
                string name = Grid_Script_0.Name.Remove(10);

                //Отчиска предыдущих
                Scripting.Children.Clear();

                for (int i = 0; i < Classes.Scripter.FullSortScripts.Count; i++)
                {
                    //Создание контейнера компонентов
                    Grid grid = new Grid();
                    grid.Name = name + i.ToString();

                    //Создание и копирование свойст(задний фон)
                    Rectangle child = Rectangle_Script_1;

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

                    rectangle.MouseDown += Rectangle_Script_MouseDown;
                    rectangle.MouseEnter += Rectangle_Script_MouseEnter;
                    rectangle.MouseLeave += Rectangle_Script_MouseLeave;

                    //Добавление элемента в контейнер
                    grid.Children.Add(rectangle);


                    //Создание и копирование свойст(текст)
                    TextBlock childTBlock = TextBlock_Script_1;

                    TextBlock textBlock = new TextBlock() { AllowDrop = childTBlock.AllowDrop, Visibility = childTBlock.Visibility, CacheMode = childTBlock.CacheMode, Clip = childTBlock.Clip, ClipToBounds = childTBlock.ClipToBounds, Effect = childTBlock.Effect, Focusable = childTBlock.Focusable, IsEnabled = childTBlock.IsEnabled, IsHitTestVisible = childTBlock.IsHitTestVisible, IsManipulationEnabled = childTBlock.IsManipulationEnabled, Opacity = childTBlock.Opacity, OpacityMask = childTBlock.OpacityMask, RenderSize = childTBlock.RenderSize, RenderTransform = childTBlock.RenderTransform, RenderTransformOrigin = childTBlock.RenderTransformOrigin, SnapsToDevicePixels = childTBlock.SnapsToDevicePixels, Uid = childTBlock.Uid, BindingGroup = childTBlock.BindingGroup, ContextMenu = childTBlock.ContextMenu, Cursor = childTBlock.Cursor, DataContext = childTBlock.DataContext, Background = childTBlock.Background, FlowDirection = childTBlock.FlowDirection, FocusVisualStyle = childTBlock.FocusVisualStyle, ForceCursor = childTBlock.ForceCursor, Height = childTBlock.Height, HorizontalAlignment = childTBlock.HorizontalAlignment, InputScope = childTBlock.InputScope, Language = childTBlock.Language, LayoutTransform = childTBlock.LayoutTransform, Margin = childTBlock.Margin, MaxHeight = childTBlock.MaxHeight, MaxWidth = childTBlock.MaxWidth, MinHeight = childTBlock.MinHeight, MinWidth = childTBlock.MinWidth, Name = childTBlock.Name.Remove(childTBlock.Name.Length - 4) + "_" + i, OverridesDefaultStyle = childTBlock.OverridesDefaultStyle, Resources = childTBlock.Resources, Style = childTBlock.Style, Tag = childTBlock.Tag, ToolTip = childTBlock.ToolTip, UseLayoutRounding = childTBlock.UseLayoutRounding, VerticalAlignment = childTBlock.VerticalAlignment, Width = childTBlock.Width, Text = Classes.Scripter.FullScripts[i].NameScript, FontFamily = childTBlock.FontFamily, FontSize = childTBlock.FontSize, FontStretch = childTBlock.FontStretch, FontStyle = childTBlock.FontStyle, FontWeight = childTBlock.FontWeight, Foreground = childTBlock.Foreground, Padding = childTBlock.Padding, BaselineOffset = childTBlock.BaselineOffset, IsHyphenationEnabled = childTBlock.IsHyphenationEnabled, LineHeight = childTBlock.LineHeight, LineStackingStrategy = childTBlock.LineStackingStrategy, TextAlignment = childTBlock.TextAlignment, TextDecorations = childTBlock.TextDecorations, TextEffects = childTBlock.TextEffects, TextTrimming = childTBlock.TextTrimming, TextWrapping = childTBlock.TextWrapping };

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

                    textBlock.MouseDown += TextBlock_Script_MouseDown;
                    textBlock.MouseEnter += TextBlock_Script_MouseEnter;
                    textBlock.MouseLeave += TextBlock_Script_MouseLeave;

                    //Добавление элемента в контейнер
                    grid.Children.Add(textBlock);

                    //Создание и копирование свойст(Удаление)
                    Ellipse childElip = Ellipse_Script_1;

                    Ellipse ellipse = new Ellipse() { Fill = childElip.Fill, AllowDrop = childElip.AllowDrop, Visibility = childElip.Visibility, CacheMode = childElip.CacheMode, Clip = childElip.Clip, ClipToBounds = childElip.ClipToBounds, Effect = childElip.Effect, Focusable = childElip.Focusable, IsEnabled = childElip.IsEnabled, IsHitTestVisible = childElip.IsHitTestVisible, IsManipulationEnabled = childElip.IsManipulationEnabled, Opacity = childElip.Opacity, OpacityMask = childElip.OpacityMask, RenderSize = childElip.RenderSize, RenderTransform = childElip.RenderTransform, RenderTransformOrigin = childElip.RenderTransformOrigin, SnapsToDevicePixels = childElip.SnapsToDevicePixels, Uid = childElip.Uid, BindingGroup = childElip.BindingGroup, ContextMenu = childElip.ContextMenu, Cursor = childElip.Cursor, DataContext = childElip.DataContext, FlowDirection = childElip.FlowDirection, FocusVisualStyle = childElip.FocusVisualStyle, ForceCursor = childElip.ForceCursor, Height = childElip.Height, HorizontalAlignment = childElip.HorizontalAlignment, InputScope = childElip.InputScope, Language = childElip.Language, LayoutTransform = childElip.LayoutTransform, Margin = childElip.Margin, MaxHeight = childElip.MaxHeight, MaxWidth = childElip.MaxWidth, MinHeight = childElip.MinHeight, MinWidth = childElip.MinWidth, Name = childElip.Name.Remove(childElip.Name.Length - 4) + "_" + i, OverridesDefaultStyle = childElip.OverridesDefaultStyle, Resources = childElip.Resources, Stretch = childElip.Stretch, Stroke = childElip.Stroke, StrokeDashArray = childElip.StrokeDashArray, StrokeDashCap = childElip.StrokeDashCap, StrokeDashOffset = childElip.StrokeDashOffset, StrokeEndLineCap = childElip.StrokeEndLineCap, StrokeLineJoin = childElip.StrokeLineJoin, StrokeMiterLimit = childElip.StrokeMiterLimit, StrokeStartLineCap = childElip.StrokeStartLineCap, StrokeThickness = childElip.StrokeThickness, Style = childElip.Style, Tag = childElip.Tag, ToolTip = childElip.ToolTip, UseLayoutRounding = childElip.UseLayoutRounding, VerticalAlignment = childElip.VerticalAlignment, Width = childElip.Width };

                    ellipse.MouseEnter += (object sender, MouseEventArgs e) =>
                    {
                        if (Scripting.Children.Count > 0)
                        {
                            int number = Convert.ToInt32((sender as Ellipse).Name.ToString().Remove(0, (sender as Ellipse).Name.ToString().Length - 1));
                            try { if (Scripting.Children[number] != null) { ((Scripting.Children[number] as Grid).Children[2] as Ellipse).Fill = (Brush)new BrushConverter().ConvertFromString("#FFBEBE"); } } catch { }
                        }
                    };

                    ellipse.MouseLeave += (object sender, MouseEventArgs e) =>
                    {
                        if (Scripting.Children.Count > 0)
                        {
                            int number = Convert.ToInt32((sender as Ellipse).Name.ToString().Remove(0, (sender as Ellipse).Name.ToString().Length - 1));
                            try { if (Scripting.Children[number] != null) { ((Scripting.Children[number] as Grid).Children[2] as Ellipse).Fill = (Brush)new BrushConverter().ConvertFromString("#F47B7B"); } } catch { }
                        }
                    };

                    foreach (Trigger item in childElip.Triggers)
                    {
                        ellipse.Triggers.Add(item);
                    }

                    foreach (InputBinding item in childElip.InputBindings)
                    {
                        ellipse.InputBindings.Add(item);
                    }

                    foreach (CommandBinding item in childElip.CommandBindings)
                    {
                        ellipse.CommandBindings.Add(item);
                    }

                    ellipse.MouseDown += Delete_Script_MouseDown;

                    //Добавление элемента в контейнер
                    grid.Children.Add(ellipse);

                    grid.Margin = Grid_Script_0.Margin;

                    grid.Visibility = Visibility.Visible;

                    grid.Height = Grid_Script_0.Height;

                    //Добавление контйнера фильтра 
                    Scripting.Children.Add(grid);
                }
            }
            catch (Exception ex) { $"[Main]: error to relaod scripts({ex.Message})".Log(); }
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
            catch (Exception ex) { $"[Main]: error to change filter color enter(Rectangle)({ex.Message})".Log(); }
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
            catch (Exception ex) { $"[Main]: error to change filter color leave(Rectangle)({ex.Message})".Log(); }
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
                        FiltersClass.filters.RemoveAt(number);
                        Reload_Filters();
                        reloadScripts();
                    }
                    catch { }
                }
            }
            catch (Exception ex) { $"[Main]: error to enter filter(Rectangle)({ex.Message})".Log(); }
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
            catch (Exception ex) { $"[Main]: error to change filter color enter(TextBlock)({ex.Message})".Log(); }
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
            catch (Exception ex) { $"[Main]: error to change filter color leave(TextBlock)({ex.Message})".Log(); }
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
                        FiltersClass.filters.RemoveAt(number);
                        Reload_Filters();
                        reloadScripts();
                    }
                    catch { }
                }
            }
            catch (Exception ex) { $"[Main]: error to enter filter(TextBlock)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки(Add_Script)
        private void Add_Script_MouseEnter(object sender, MouseEventArgs e)
        {
            try { Add_Script.Fill = (Brush)new BrushConverter().ConvertFromString("#ECFFDD"); }
            catch (Exception ex) { $"[Main]: error to change add script color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки(Add_Script)
        private void Add_Script_MouseLeave(object sender, MouseEventArgs e)
        {
            try { Add_Script.Fill = (Brush)new BrushConverter().ConvertFromString("#D9F3C4"); }
            catch (Exception ex) { $"[Main]: error to change add script color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Метод для перехода на страницу создания скрипта
        private void Add_Script_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Classes.Scripter.IsSaves = false;

                MoveClass.MoveToEdit();
            }
            catch (Exception ex) { $"[Main]: error to enter add script(Rectangle)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки
        private void Rectangle_Script_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (Scripting.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as Rectangle).Name.ToString().Remove(0, (sender as Rectangle).Name.ToString().Length - 1));
                    try { if (Scripting.Children[number] != null) { ((Scripting.Children[number] as Grid).Children[0] as Rectangle).Fill = (Brush)new BrushConverter().ConvertFromString("#C0D7F9"); } } catch { }
                }
            }
            catch (Exception ex) { $"[Main]: error to change script color enter(Rectangle)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки
        private void Rectangle_Script_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (Scripting.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as Rectangle).Name.ToString().Remove(0, (sender as Rectangle).Name.ToString().Length - 1));
                    try { if (Scripting.Children[number] != null) { ((Scripting.Children[number] as Grid).Children[0] as Rectangle).Fill = (Brush)new BrushConverter().ConvertFromString("#A2BDE4"); } } catch { }
                }
            }
            catch (Exception ex) { $"[Main]: error to change script color leave(Rectangle)({ex.Message})".Log(); }
        }

        //Изменение фона фильтра при наведение мышки
        private void TextBlock_Script_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (Scripting.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as TextBlock).Name.ToString().Remove(0, (sender as TextBlock).Name.ToString().Length - 1));
                    try { if (Scripting.Children[number] != null) { ((Scripting.Children[number] as Grid).Children[0] as Rectangle).Fill = (Brush)new BrushConverter().ConvertFromString("#C0D7F9"); } } catch { }
                }
            }
            catch (Exception ex) { $"[Main]: error to enter script(Rectangle)({ex.Message})".Log(); }
        }

        //Возврат в исходное состояние фона фильтра при отсутствии мышки
        private void TextBlock_Script_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (Scripting.Children.Count > 0)
                {
                    int number = Convert.ToInt32((sender as TextBlock).Name.ToString().Remove(0, (sender as TextBlock).Name.ToString().Length - 1));
                    try { if (Scripting.Children[number] != null) { ((Scripting.Children[number] as Grid).Children[0] as Rectangle).Fill = (Brush)new BrushConverter().ConvertFromString("#A2BDE4"); } } catch { }
                }
            }
            catch (Exception ex) { $"[Main]: error to change script color leave(TextBlock)({ex.Message})".Log(); }
        }

        //Метод выбора скрипт для просмотра(Rectangle)
        private void Rectangle_Script_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int number = Convert.ToInt32((sender as Rectangle).Name.ToString().Remove(0, (sender as Rectangle).Name.ToString().Length - 1));

                Classes.Scripter.SelectedScript = Classes.Scripter.All.Scripts[number];
                Classes.Scripter.NumberSelectedScript = number;

                MoveClass.MoveToScript();
            }
            catch (Exception ex) { $"[Main]: error to enter script enter(Rectangle)({ex.Message})".Log(); }
        }

        //Метод выбора скрипт для просмотра(TextBlock)
        private void TextBlock_Script_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int number = Convert.ToInt32((sender as TextBlock).Name.ToString().Remove(0, (sender as TextBlock).Name.ToString().Length - 1));

                Classes.Scripter.SelectedScript = Classes.Scripter.All.Scripts[number];
                Classes.Scripter.NumberSelectedScript = number;

                MoveClass.MoveToScript();
            }
            catch (Exception ex) { $"[Main]: error to enter script enter(TextBlock)({ex.Message})".Log(); }
        }

        //метдо для удаления скрипта
        private void Delete_Script_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int number = Convert.ToInt32((sender as Ellipse).Name.ToString().Remove(0, (sender as Ellipse).Name.ToString().Length - 1));

                if (MessageBox.Show("Вы точно хотите удалить скрипт", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Classes.Scripter.DeleteScript(number);
                    Classes.Scripter.SortMainScripts();
                    Classes.Scripter.All.MaxNum--;
                    Classes.Scripter.Save();
                    reloadScripts();
                }
            }
            catch (Exception ex) { $"[Main]: error to enter script delete({ex.Message})".Log(); }
        }
    }
}
