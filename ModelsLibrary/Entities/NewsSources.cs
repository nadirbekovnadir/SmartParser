namespace SmartParser.Domain.Entities
{
	public class NewsSources
    {
        private string name = string.Empty;
        private string url = string.Empty;
        private int entitesCount = 0;
        private List<string> missedColumns = new();
        private bool isLoaded = false;
        private bool isParsed = false;

        public string Name { get => name; set => name = value; }
        public string Url { get => url; set => url = value; }
        public int EntitesCount { get => entitesCount; set => entitesCount = value; }
        public List<string> MissedColumns { get => missedColumns; set => missedColumns = value; }
        public bool IsLoaded { get => isLoaded; set => isLoaded = value; }
        public bool IsParsed { get => isParsed; set => isParsed = value; }

        public NewsSources() { }

        public NewsSources(NewsSources ns)
        {
            Name = ns.Name;
            Url = ns.Url;
            EntitesCount = ns.entitesCount;
            MissedColumns = ns.MissedColumns.Select(col => new string(col)).ToList();
            IsLoaded = ns.isLoaded;
            IsParsed = ns.isParsed;

        }
    }
}
