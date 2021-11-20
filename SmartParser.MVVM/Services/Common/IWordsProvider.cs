using System.Collections.Generic;

namespace SmartParser.MVVM.Services.Common
{
    public interface IWordsProvider
    {
        List<string> Convert(string value);
        List<string> Load(string path);
    }
}