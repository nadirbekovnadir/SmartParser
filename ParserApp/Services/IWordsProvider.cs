using System.Collections.Generic;

namespace ParserApp.Services
{
    public interface IWordsProvider
    {
        List<string> Convert(string value);
        List<string> Load(string path);
    }
}