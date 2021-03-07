using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Classes
{
    //Виды файлов(создано для отображения фотографий)
    public enum FileType { File, Photo }
    public class FilePath
    {
        //Путь файла
        public string path;
        //Вид файла
        public FileType type;
    }
}
