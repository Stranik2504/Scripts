using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.IO;

namespace Scripts.Classes
{
    public static class Scripter
    {
        //Все параметры
        public static All All { get; private set; }
        //Хранение скриптов
        private static Dictionary<long, Script> Scripts = new Dictionary<long, Script>();
        //Отсоритрованные скрипты
        private static Dictionary<long, Script> SortScripts = new Dictionary<long, Script>();

        //Все отсоритрованные скрипты, если таких нет возвращает все
        public static Dictionary<long, Script> FullScripts { get { if (SortScripts.Values.Count > 0) { return SortScripts; } else { return Scripts; } } }
        //Все отсоритрованные скрипты
        public static Dictionary<long, Script> FullSortScripts { get { return SortScripts; } }

        //Выбранный скрипт
        public static Script SelectedScript { get; set; }
        //Номер выбраного скрипта
        public static long NumberSelectedScript;

        //Нужно ли пересохранять(для Edit)
        public static bool IsSaves = false;

        //Загрузка параметров
        public static void Load()
        {
            try
            {
                using (var reader = new StreamReader(@"Saves\Info.inf"))
                {
                    All = JsonConvert.DeserializeObject<All>(reader.ReadLine());

                    Scripts = All.Scripts;

                    reader.Close();
                }
            }
            catch (Exception ex) { $"[Scripter]: error to load script({ex.Message})".Log(); }
        }

        //Сохранение парасетров
        public static void Save()
        {
            try
            {

                using (var writer = new StreamWriter(@"Saves\Info.inf"))
                {
                    All.Scripts = Scripts;

                    writer.Write(JsonConvert.SerializeObject(All));

                    writer.Close();
                }
            }
            catch (Exception ex) { $"[Scripter]: error to save script({ex.Message})".Log(); }
        }

        //Добавление скрипта
        public static long AddScript(Script script)
        {
            try
            {
                if (AddDirect(script))
                {
                    Scripts.Add(All.MaxNum, script);

                    All.MaxNum++;

                    return All.MaxNum - 1;
                }
                else { return -1; }
            }
            catch (Exception ex) { $"[Scripter]: error to add script({ex.Message})".Log(); return -1; }
        }

        //Перемещение скриптов
        public static bool AddDirect(Script script)
        {
            try
            {
                if (script.NameScript == null || script.NameScript == "" || script.NameScript == " ") { script.NameScript = All.MaxNum.ToString(); Directory.CreateDirectory($@"Saves\{script.NameScript}"); }

                Directory.CreateDirectory($@"Saves\{script.NameScript}");

                if (script.PlaceAudio != null && script.PlaceAudio != "" && script.PlaceAudio != " ") { File.Copy(script.PlaceAudio, $@"Saves\{script.NameScript}\{Path.GetFileName(script.PlaceAudio)}"); script.PlaceAudio = $@"Saves\{script.NameScript}\{Path.GetFileName(script.PlaceAudio)}"; }
                if (script.Scripting != null && script.Scripting != "" && script.Scripting != " ") { using (var writer = new StreamWriter($@"Saves\{script.NameScript}\Script.cs")) { writer.WriteLine(script.Scripting); script.Scripting = $@"Saves\{script.NameScript}\Script.cs"; } }
                if (script.VisualCode != null && script.VisualCode != "" && script.VisualCode != " ") { using (var writer = new StreamWriter($@"Saves\{script.NameScript}\VisualCode.xaml")) { writer.WriteLine(script.VisualCode); script.VisualCode = $@"Saves\{script.NameScript}\VisualCode.xaml"; } }

                if (script.PlaceFiles.Count > 0) { if (Directory.Exists($@"Saves\{script.NameScript}\Files") == false) { Directory.CreateDirectory($@"Saves\{script.NameScript}\Files"); } }

                for (int i = 0; i < script.PlaceFiles.Count; i++)
                {
                    if (script.PlaceFiles[i].path != null && script.PlaceFiles[i].path != "" && script.PlaceFiles[i].path != " ") { File.Copy(script.PlaceFiles[i].path, $@"Saves\{script.NameScript}\Files\{Path.GetFileName(script.PlaceFiles[i].path)}"); }
                    script.PlaceFiles[i].path = $@"Saves\{script.NameScript}\Files\{Path.GetFileName(script.PlaceFiles[i].path)}";
                }

                return true;
            }
            catch (Exception ex) { $"[Scripter]: error to add directiory({ex.Message})".Log(); return false; }
        }

        //Удаление файлов скрипта
        public static bool DeleteDirect(long numberScript)
        {
            try
            {
                if (File.Exists(Scripts[numberScript].PlaceAudio) == true) { File.Move(Scripts[numberScript].PlaceAudio, $@"Saves\{Path.GetFileName(Scripts[numberScript].PlaceAudio)}"); File.Delete($@"Saves\{Path.GetFileName(Scripts[numberScript].PlaceAudio)}"); }
                if (File.Exists(Scripts[numberScript].Scripting) == true) { File.Delete(Scripts[numberScript].Scripting); }
                if (File.Exists(Scripts[numberScript].VisualCode) == true) { File.Delete(Scripts[numberScript].VisualCode); }

                for (int i = 0; i < Scripts[numberScript].PlaceFiles.Count; i++)
                {
                    if (File.Exists(Scripts[numberScript].PlaceFiles[i].path)) { File.Delete(Scripts[numberScript].PlaceFiles[i].path); }
                }

                if (Directory.Exists($@"Saves\{Scripts[numberScript].NameScript}\Files")) { Directory.Delete($@"Saves\{Scripts[numberScript].NameScript}\Files"); }

                System.Threading.Thread.Sleep(100);

                Directory.Delete(@"Saves\" + Scripts[numberScript].NameScript);

                return true;
            }
            catch (Exception ex) { $"[Scripter]: error to delete directory({ex.Message})".Log(); return false; }
        }

        //Удаление скрипта
        public static bool DeleteScript(long numberScript)
        {
            try
            {
                if (DeleteDirect(numberScript))
                {
                    Scripts.Remove(numberScript);

                    return true;
                }
                else { return false; }
            }
            catch (Exception ex) { $"[Scripter]: error to delete script({ex.Message})".Log(); return false; }
        }

        //Пересохранение скрипта
        public static bool ReSaveScript(Script script, long numberScript)
        {
            try
            {
                if (Scripts[numberScript].NameScript != script.NameScript)
                {
                    if (Directory.Exists($@"Saves\{script.NameScript}") == false) { Directory.CreateDirectory($@"Saves\{script.NameScript}"); }

                    if (script.PlaceAudio != null && script.PlaceAudio != "" && script.PlaceAudio != " ")
                    {
                        if (Scripts[numberScript].PlaceAudio != script.PlaceAudio) { File.Copy(script.PlaceAudio, $@"Saves\{script.NameScript}\{Path.GetFileName(script.PlaceAudio)}"); script.PlaceAudio = $@"Saves\{script.NameScript}\{Path.GetFileName(script.PlaceAudio)}"; }
                        else { if (Scripts[numberScript].PlaceAudio != null && Scripts[numberScript].PlaceAudio != "" && Scripts[numberScript].PlaceAudio != " ") { File.Copy(Scripts[numberScript].PlaceAudio, $@"Saves\{script.NameScript}\{Path.GetFileName(script.PlaceAudio)}"); File.Delete(Scripts[numberScript].PlaceAudio); script.PlaceAudio = $@"Saves\{script.NameScript}\{Path.GetFileName(script.PlaceAudio)}"; } }
                    } else { if (Scripts[numberScript].PlaceAudio != null && Scripts[numberScript].PlaceAudio != "" && Scripts[numberScript].PlaceAudio != " ") { File.Delete(Scripts[numberScript].PlaceAudio); } }

                    if (script.Scripting != null && script.Scripting != "" && script.Scripting != " ") { using (var writer = new StreamWriter($@"Saves\{script.NameScript}\Script.cs")) { writer.WriteLine(script.Scripting); script.Scripting = $@"Saves\{script.NameScript}\Script.cs"; } }
                    if (script.VisualCode != null && script.VisualCode != "" && script.VisualCode != " ") { using (var writer = new StreamWriter($@"Saves\{script.NameScript}\VisualCode.xaml")) { writer.WriteLine(script.VisualCode); script.VisualCode = $@"Saves\{script.NameScript}\VisualCode.xaml"; } }

                    if (script.PlaceFiles.Count > 0) { if (Directory.Exists($@"Saves\{script.NameScript}\Files") == false) { Directory.CreateDirectory($@"Saves\{script.NameScript}\Files"); } }

                    for (int i = 0; i < Scripts[numberScript].PlaceFiles.Count; i++)
                    {
                        if (script.PlaceFiles.Where(x => x.path == Scripts[numberScript].PlaceFiles[i].path).Count() > 0)
                        {
                            if (File.Exists(Scripts[numberScript].PlaceFiles[i].path)) { File.Copy(Scripts[numberScript].PlaceFiles[i].path, $@"Saves\{script.NameScript}\Files\{Path.GetFileName(Scripts[numberScript].PlaceFiles[i].path)}"); }
                        }
                    }

                    for (int i = 0; i < script.PlaceFiles.Count; i++)
                    {
                        if (Scripts[numberScript].PlaceFiles.Where(x => x.path == script.PlaceFiles[i].path).Count() == 0)
                        {
                            File.Copy(script.PlaceFiles[i].path, $@"Saves\{script.NameScript}\Files\{Path.GetFileName(script.PlaceFiles[i].path)}");
                            script.PlaceFiles[i].path = $@"Saves\{script.NameScript}\Files\{Path.GetFileName(script.PlaceFiles[i].path)}";
                        }
                    }

                    for (int i = 0; i < Scripts[numberScript].PlaceFiles.Count; i++)
                    {
                        if (script.PlaceFiles.Where(x => x.path == Scripts[numberScript].PlaceFiles[i].path).Count() > 0)
                        {
                            script.PlaceFiles[i].path = $@"Saves\{script.NameScript}\Files\{Path.GetFileName(Scripts[numberScript].PlaceFiles[i].path)}";
                        }
                    }

                    DeleteDirect(numberScript);

                    script.TimeSave = DateTime.Now;
                }
                else
                {
                    if (script.PlaceAudio != null && script.PlaceAudio != "" && script.PlaceAudio != " ") { if (Scripts[numberScript].PlaceAudio != script.PlaceAudio) { File.Copy(script.PlaceAudio, $@"Saves\{script.NameScript}\{Path.GetFileName(script.PlaceAudio)}"); script.PlaceAudio = $@"Saves\{script.NameScript}\{Path.GetFileName(script.PlaceAudio)}"; } }
                    if (script.Scripting != null && script.Scripting != "" && script.Scripting != " ") { using (var writer = new StreamWriter($@"Saves\{script.NameScript}\Script.cs")) { writer.WriteLine(script.Scripting); script.Scripting = $@"Saves\{script.NameScript}\Script.cs"; } }
                    if (script.VisualCode != null && script.VisualCode != "" && script.VisualCode != " ") { using (var writer = new StreamWriter($@"Saves\{script.NameScript}\VisualCode.xaml")) { writer.WriteLine(script.VisualCode); script.VisualCode = $@"Saves\{script.NameScript}\VisualCode.xaml"; } }

                    if (script.PlaceFiles.Count > 0) { if (Directory.Exists($@"Saves\{script.NameScript}\Files") == false) { Directory.CreateDirectory($@"Saves\{script.NameScript}\Files"); } }

                    for (int i = 0; i < Scripts[numberScript].PlaceFiles.Count; i++)
                    {
                        if (script.PlaceFiles.Where(x => x.path == Scripts[numberScript].PlaceFiles[i].path).Count() == 0 || script.PlaceFiles.Count == 0)
                        {
                            if (File.Exists(Scripts[numberScript].PlaceFiles[i].path)) { File.Delete(Scripts[numberScript].PlaceFiles[i].path); }
                        }
                    }

                    for (int i = 0; i < script.PlaceFiles.Count; i++)
                    {
                        if (Scripts[numberScript].PlaceFiles.Where(x => x.path == script.PlaceFiles[i].path).Count() == 0 || Scripts[numberScript].PlaceFiles.Count == 0)
                        {
                            File.Copy(script.PlaceFiles[i].path, $@"Saves\{script.NameScript}\Files\{Path.GetFileName(script.PlaceFiles[i].path)}");
                            script.PlaceFiles[i].path = $@"Saves\{script.NameScript}\Files\{Path.GetFileName(script.PlaceFiles[i].path)}";
                        }
                    }

                    script.TimeSave = DateTime.Now;
                }

                Scripts[numberScript] = script;

                return true;
            }
            catch (Exception ex) { $"[Scripter]: error to resave script({ex.Message})".Log(); return false; }
        }

        //Сортировка скрипта
        public static void Sort()
        {
            try
            {
                SortScripts.Clear();

                long number = -1;

                if (FiltersClass.filters.Count > 0) { SortScripts = Scripts.Where(script => script.Value.Tags.FindAll((x) => { return x == FiltersClass.filters[0] || x.Contains(FiltersClass.filters[0]); }).Count > 0 || Convert.ToDateTime(script.Value.TimeSave.ToShortDateString()) == (DateTime.TryParse(FiltersClass.filters[0], out DateTime res1) == true ? Convert.ToDateTime(Convert.ToDateTime(FiltersClass.filters[0]).ToShortDateString()) : DateTime.MinValue)).ToDictionary((x) => { number++; return number; }, x => x.Value); } else { SortScripts = Scripts.Where(x => x.Key == x.Key).ToDictionary(x => x.Key, x => x.Value); }

                for (int i = 1; i < FiltersClass.filters.Count; i++)
                {
                    number = -1;
                    SortScripts = SortScripts.Where(script => script.Value.Tags.FindAll((x) => { return x == FiltersClass.filters[i] || x.Contains(FiltersClass.filters[i]); }).Count > 0 || Convert.ToDateTime(script.Value.TimeSave.ToShortDateString()) == (DateTime.TryParse(FiltersClass.filters[i], out DateTime res1) == true ? Convert.ToDateTime(Convert.ToDateTime(FiltersClass.filters[i]).ToShortDateString()) : DateTime.MinValue)).ToDictionary((x) => { number++; return number; }, x => x.Value);
                }

                SortScripts = SortScripts.OrderBy(x => x.Value.NameScript).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex) { $"[Scripter]: error to sort script({ex.Message})".Log(); }
        }

        //Сортировка Dictionary, чтобы скрипты шли по порядку
        public static void SortMainScripts()
        {
            try
            {
                SortScripts.Clear();

                long number = -1;

                Scripts = Scripts.Select(x => x).ToDictionary((x) => { number++; return number; }, x => x.Value);
            }
            catch (Exception ex) { $"[Scripter]: error to sort Dictionary({ex.Message})".Log(); }
        }
    }

    //Класс для хранения параметров
    public class All
    {
        //Размер окна(Высота), при старте
        public int Height;
        //Размер окна(Ширана), при старте
        public int Width;
        //Последний номер в Dictionary
        public long MaxNum;
        //Все скрипты
        public Dictionary<long, Script> Scripts;
    }
}
