namespace ScrapporNet.Entities
{
    public class Product
    {
        public string Name { get; set; }

        public string Id { get; set; }
        public string Cup { get; set; }

        public string Category { get; set; }
        public string Color { get; set; }
        public string Nature { get; set; }
        public string Format { get; set; }

        public string Price { get; set; }

        public string Url { get; set; }
    }

    public interface IProduct
    {
        string Name{get;set;}
        string Id { get; set; }
        string Cup { get; set; }
        string Category { get; set; }
        string Color { get; set; }
        string Nature { get; set; }
        string Format { get; set; }
    }
}
