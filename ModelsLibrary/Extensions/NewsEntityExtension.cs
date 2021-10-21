using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Models.Entities
{
    public partial class NewsEntity
    {
        private static CsvConfiguration _csvConf;

        static NewsEntity()
        {
            _csvConf = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine
            };
        }

        public static List<NewsEntity> Match(List<NewsEntity> entities, IEnumerable<string> patterns)
        {

            var regexes = (from p in patterns select new Regex(p, RegexOptions.Compiled | RegexOptions.IgnoreCase)).ToList();

            var result = from e in entities
                         where (from r in regexes
                                select r.Match(e.Name).Success || r.Match(e.Title).Success || r.Match(e.Description).Success)
                                .All(u => u)
                         select e;

            return result.ToList();
        }

        public static List<T> Except<T>(List<T> f, List<T> s)
        {
            return f.Except(s).ToList();
        }

        public static void SaveToCsv(List<NewsEntity> entities, string dir, string fileName)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string path = Path.Combine(dir, fileName) + ".csv";

            SaveToCsv(entities, path);
        }

        public static void SaveToCsv(List<NewsEntity> entities, string path)
        {
            using var writer = new StreamWriter(path);
            using var csv = new CsvWriter(writer, _csvConf);
            csv.WriteRecords(entities);
        }

        public static List<NewsEntity>? LoadFromCsv(string dir, string fileName)
        {
            string path = Path.Combine(dir, fileName) + ".csv";

            return LoadFromCsv(path);
        }

        public static List<NewsEntity>? LoadFromCsv(string path)
        {
            if (!File.Exists(path))
                return null;

            var entities = new List<NewsEntity>();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, _csvConf))
            {
                entities = csv.GetRecords<NewsEntity>().ToList();
            }

            return entities;
        }

        public static void SaveToExcel(List<NewsEntity> entities, string dir, string fileName)
        {
            string path = Path.Combine(dir, fileName) + ".xlsx";
            SaveToExcel(entities, path);
        }

        public static void SaveToExcel(List<NewsEntity> entities, string path)
        {
            
        }
    }
}
