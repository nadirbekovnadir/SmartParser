using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParser.MVVM.Services.Common
{
    public interface IDialogService
    {
        void ShowMessage(string message);   // показ сообщения

        string FilePath { get; set; }   // путь к выбранному файлу
        bool OpenFileDialog();  // открытие файла
        bool SaveFileDialog();  // сохранение файла

        string FolderPath { get; set; }
        bool OpenFolderDialog();
    }
}
