using Microsoft.Win32;
using SmartParser.MVVM.Services.Common;
using System.Windows;

namespace SmartParser.MVVM.Services
{
	public class DefaultDialogService : IDialogService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public string FilePath { get; set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        public string FolderPath { get ; set; }

        public bool OpenFolderDialog()
        {
            using var openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK;

            if (result == true)
            {
                FolderPath = openFolderDialog.SelectedPath;
                return true;
            }
            return false;
        }
    }
}
