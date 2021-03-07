using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Classes
{
    public class Script
    {
        //Название скрипта
        public string NameScript;
        //Все теги скрипта
        public List<string> Tags = new List<string>();
        //Расположение и тип файлов
        public List<FilePath> PlaceFiles = new List<FilePath>();
        //Расположение аудио файла
        public string PlaceAudio;
        //Расположение скрипта
        public string Scripting;
        //Расположение физуального кода
        public string VisualCode;
        //Дата изменения скрипта
        public DateTime TimeSave;
    }
}
