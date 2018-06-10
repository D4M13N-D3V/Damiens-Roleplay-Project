namespace roleplay
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public int Weight { get; set; }
        public bool Illegal { get; set; }
    }
}