using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class NewsEntity : IEquatable<NewsEntity>
    {
        public string Name {  get; set; }
        public string Title {  get; set; }
        public string Description {  get; set; }
        public string Link {  get; set; }

        [Format(@"yyyy-MM-dd HH:mm:ss")]
        public DateTime Date {  get; set; }

        private static CsvConfiguration _csvConf;
        static NewsEntity()
        {
            _csvConf = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine
            };

        }

        public static List<NewsEntity> LoadFromCsv(string path)
        {
            var entities = new List<NewsEntity>();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, _csvConf))
            {
                entities = csv.GetRecords<NewsEntity>().ToList();
            }

            return entities;
        }

        public static void SaveToCsv(List<NewsEntity> entities, string path)
        {
            using var writer = new StreamWriter(path);
            using var csv = new CsvWriter(writer, _csvConf);
            csv.WriteRecords(entities);
        }

        public bool Equals(NewsEntity? other) =>
            other != null &&
            Name == other.Name && 
            Title == other.Title && 
            Description == other.Description &&
            Link == other.Link &&
            Date == other.Date;

        public override bool Equals(object? obj) => Equals(obj as NewsEntity);

        public override int GetHashCode()
        {
            int hCode = 
                Name.GetHashCode() ^ 
                Title.GetHashCode() ^ 
                Description.GetHashCode() ^ 
                Link.GetHashCode() ^ 
                Date.GetHashCode();
            return hCode.GetHashCode();
        }
    }

}
