namespace ScrapporNet.Entities
{
    public class Wine : Product, IProduct
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string Appellation { get; set; }
        public string Fournisseur { get; set; }
        public string AlcoholRate { get; set; }

        public string TastingYear { get; set; }
        public string WineFamily { get; set; }
        public string Evolution { get; set; }
        public string Potential { get; set; }

        public string TastingNotes { get; set; }

        public override string ToString()
        {
            return Name + "-" + Id;
        }
    }

    
}