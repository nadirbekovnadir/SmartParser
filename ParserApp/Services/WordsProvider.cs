using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp.Services
{
    public class WordsProvider : IWordsProvider
    {
        public List<string> Load(string path)
        {
            //Читаю вес текст, чтобы можно было манипулировать его конвертацией
            var text = File.ReadAllText(path);
            return Convert(text);
        }

        public List<string> Convert(string value)
        {
            var result = value.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            return result.ToList();
        }
    }
}
