using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Models.Entities;

namespace Models
{
    public static class DataHelper
    {

        public static List<NewsEntity> Match(List<NewsEntity> entities, params string[] patterns)
        {

            var regexes = (from p in patterns select new Regex(p, RegexOptions.Compiled | RegexOptions.IgnoreCase)).ToList();

            var result = from e in entities
                         where (from r in regexes 
                                select r.Match(e.Name).Success || r.Match(e.Title).Success || r.Match(e.Description).Success)
                                .All(u => u)
                         select e;

            return result.ToList();
        }

        public static List<NewsEntity> Except(List<NewsEntity> f, List<NewsEntity> s)
        {
            return f.Except(s).ToList();
        }
    }
}
