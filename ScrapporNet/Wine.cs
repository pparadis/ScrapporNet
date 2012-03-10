namespace ScrapporNet
{
    public class Wine
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Format { get; set; }
        public string Nature { get; set; }

        public override string ToString()
        {
            return Name + "-" + Id;
        }
    }
}