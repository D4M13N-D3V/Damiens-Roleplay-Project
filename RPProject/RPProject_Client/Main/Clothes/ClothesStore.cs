namespace client.Main.Clothes
{
    public enum ClothesStoreTypes { Clothes, Tattoo, Jewlery, Plastic, Barbor, Mask };
    public class ClothesStore
    {
        public string Name = "Clothing Store";
        public float X = 0;
        public float Y = 0;
        public float Z = 0;
        public ClothesStoreTypes Type;

        public ClothesStore(float x, float y, float z, string name, ClothesStoreTypes type)
        {
            Name = name;
            Type = type;
            X = x;
            Y = y;
            Z = z;
        }
    }
}
