using CsvHelper;
using CsvHelper.Configuration;
using Ganss.Excel;
using Npoi.Mapper;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SmartParser.Domain.Entities
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
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    try
                    {
                        entities.Add(csv.GetRecord<NewsEntity>());
                    }
                    catch (Exception)
                    {
                        //Дописать обработку
                        //throw;
                    }
                }    
            }
            return entities;
        }

        public static void SaveToExcel(List<NewsEntity> entities, string dir, string fileName, string sheetName)
        {
            string path = Path.Combine(dir, fileName) + ".xlsx";
            ExcelMapper excelMapper = new ExcelMapper();
            SaveToExcel(entities, excelMapper, path, sheetName);
        }

        public static void SaveToExcel(List<List<NewsEntity>> entities, string dir, string fileName, List<string> sheetNames)
        {
            string path = Path.Combine(dir, fileName) + ".xlsx";
            ExcelMapper excelMapper = new ExcelMapper();
            for (int i = 0; i < entities.Count; i++)
            {
                SaveToExcel(entities[i], excelMapper, path, sheetNames[i]);
            }
        }

        private static void SaveToExcel(List<NewsEntity> entities, ExcelMapper excelMapper, string path, string sheetName)
        {
            var truncate = (int maxLength, string value) =>
            {
                if (value.Length > maxLength) {
                    return value.Substring(0, maxLength);
                }
                return value;
            };

            var mapper = (NewsEntity entity) =>
            {
                const int maxLength = 32767;

                entity.Name = truncate(maxLength, entity.Name);
                entity.Title = truncate(maxLength, entity.Title);
                entity.Description = truncate(maxLength, entity.Description);
                if (entity.Link.Length > maxLength) 
                {
                    entity.Link = "Link is too long!";
                }
            };

            Parallel.ForEach(entities, mapper);

            excelMapper.Save(path, entities, sheetName);
        }
        
        public static void AppendToExcel(List<List<NewsEntity>> entities, string path, List<string> sheetNames)
		{
            var mapper = new Mapper(path);

            for (int i = 0; i < sheetNames.Count; i++)
            {
                mapper.Put(entities[i], sheetNames[i], false);
            }

            mapper.Save(path);
        }

    }
}
