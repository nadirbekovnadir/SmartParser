namespace SmartParser.Domain.Entities
{
	public partial class NewsEntity : IEquatable<NewsEntity>
    {
        public string Name { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Link { get; set; } = "";

        //[Format(@"yyyy-MM-dd HH:mm:ss")]
        public DateTime? Date {  get; set; } = DateTime.Now;

        public NewsEntity()
        {

        }

        public NewsEntity(NewsEntity other)
        {
            Name = other.Name;
            Title = other.Title;
            Description = other.Description;
            Link = other.Link;
            Date = other.Date;
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
