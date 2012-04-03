namespace ScrapporNet.Entities
{
    public class Wine
    {
        public string Name { get; set; }

        public string Id { get; set; }
        public string Cup { get; set; }

        public string Category { get; set; }
        public string Color { get; set; }
        public string Nature { get; set; }
        public string Format { get; set; }
        
        public string Url { get; set; }
        

        public override string ToString()
        {
            return Name + "-" + Id;
        }
    }
}